using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Models.DTO;
using BankingSystemServices.Services;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services;

public class ClientService : IClientService
{
    private readonly BankingSystemDbContext _bankingSystemDbContext;
    private Currency? _defaultCurrency;

    public ClientService(BankingSystemDbContext bankingSystemDbContext)
    {
        _bankingSystemDbContext = bankingSystemDbContext;
    }

    public async Task<Client> AddClientAsync(ClientDto clientDto)
    {
        var client = MapDtoToClient(clientDto);

        await ValidateClientAsync(client);

        _defaultCurrency ??= await GetDefaultCurrencyAsync();

        var defaultAccount = await CreateAccountAsync(client, _defaultCurrency);
        
        await _bankingSystemDbContext.Clients.AddAsync(client);
        await _bankingSystemDbContext.Accounts.AddAsync(defaultAccount);

        await _bankingSystemDbContext.SaveChangesAsync();

        return client;
    }

    public async Task<Client> UpdateClientAsync(Guid clientId, ClientDto newClientDto)
    {
        var client = await GetClientByIdAsync(clientId);

        client = MapDtoToClient(newClientDto, client);

        await ValidateClientAsync(client, true);

        await _bankingSystemDbContext.SaveChangesAsync();

        return client;
    }

    public async Task DeleteClientAsync(Guid clientId)
    {
        var bankClient = await GetClientByIdAsync(clientId);

        var clientAccounts = await _bankingSystemDbContext.Accounts.Where(account => account.ClientId.Equals(clientId))
            .ToListAsync();

        _bankingSystemDbContext.Accounts.RemoveRange(clientAccounts);
        _bankingSystemDbContext.Clients.Remove(bankClient);

        await _bankingSystemDbContext.SaveChangesAsync();
    }

    public async Task<Client> GetClientByIdAsync(Guid clientId)
    {
        var client =
            await _bankingSystemDbContext.Clients.SingleOrDefaultAsync(client => client.ClientId.Equals(clientId));

        if (client == null)
            throw new ValueNotFoundException($"The client with identifier {clientId} does not exist!");

        return client;
    }

    private async Task<Currency> GetDefaultCurrencyAsync()
    {
        var currency =
            await _bankingSystemDbContext.Currencies.SingleOrDefaultAsync(currency => currency.Code == "USD");

        if (currency == null)
            throw new ValueNotFoundException("Default currency USD not found.");

        return currency;
    }

    private static async Task<Account> CreateAccountAsync(Client client, Currency currency, decimal amount = 0)
    {
        return await Task.Run(() => TestDataGenerator.GenerateBankClientAccount(currency, client, amount));
    }

    private async Task ValidateClientAsync(Client client, bool itUpdate = false)
    {
        if (!itUpdate 
            && await _bankingSystemDbContext.Clients.AnyAsync(c => c.ClientId.Equals(client.ClientId))
            && !await ClientContainsInDatabase(client))
            throw new ArgumentException("This client has already been added to the banking system!", nameof(client));

        if (string.IsNullOrWhiteSpace(client.FirstName))
            throw new PropertyValidationException("Client first name not specified!", nameof(client.FirstName),
                nameof(Client));

        if (string.IsNullOrWhiteSpace(client.LastName))
            throw new PropertyValidationException("The client last name not specified!", nameof(client.LastName),
                nameof(Client));

        if (string.IsNullOrWhiteSpace(client.PhoneNumber))
            throw new PropertyValidationException("Client number not specified!", nameof(client.PhoneNumber),
                nameof(Client));

        if (string.IsNullOrWhiteSpace(client.Email))
            throw new PropertyValidationException("Client e-mail not specified!", nameof(client.Email), nameof(Client));

        if (string.IsNullOrWhiteSpace(client.Address))
            throw new PropertyValidationException("Client address not specified!", nameof(client.Address),
                nameof(Client));

        if (client.DateOfBirth > DateTime.Now || client.DateOfBirth.Equals(DateTime.MinValue) ||
            client.DateOfBirth.Equals(DateTime.MaxValue))
            throw new PropertyValidationException("The client date of birth is incorrect!", nameof(client.DateOfBirth),
                nameof(Client));

        var age = await Task.Run(() => TestDataGenerator.CalculateAge(client.DateOfBirth));

        if (age < 18)
            throw new PropertyValidationException("The client is under 18 years old!", nameof(client.Age),
                nameof(Client));

        if (age != client.Age || client.Age <= 0) client.Age = TestDataGenerator.CalculateAge(client.DateOfBirth);
    }

    private async Task<bool> ClientContainsInDatabase(Client client)
    {
        var clientContains = await (from clients in _bankingSystemDbContext.Clients
            where clients.FirstName == client.FirstName &&
                  clients.LastName == client.LastName &&
                  clients.PhoneNumber == client.PhoneNumber &&
                  clients.Address == client.Address &&
                  clients.Email == client.Email &&
                  clients.DateOfBirth.Equals(client.DateOfBirth) &&
                  clients.Age == client.Age
            select 0).AnyAsync();

        return clientContains;
    }
    
    private static Client MapDtoToClient(ClientDto clientDto, Client? client = null)
    {
        var mappedClient = client ?? new Client();

        mappedClient.ClientId = client is not null ? mappedClient.ClientId : Guid.NewGuid();
        mappedClient.FirstName = clientDto.FirstName;
        mappedClient.LastName = clientDto.LastName;
        mappedClient.DateOfBirth = clientDto.DateOfBirth.ToUniversalTime();
        mappedClient.Age = clientDto.Age;
        mappedClient.Address = clientDto.Address;
        mappedClient.Bonus = clientDto.Bonus;
        mappedClient.Email = clientDto.Email;
        mappedClient.PhoneNumber = clientDto.PhoneNumber;

        return mappedClient;
    }
}