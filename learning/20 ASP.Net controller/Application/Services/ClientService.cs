using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Models.DTO;
using BankingSystemServices.Services;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class ClientService
{
    private readonly BankingSystemDbContext _bankingSystemDbContext;
    private readonly Currency? _defaultCurrency;

    public ClientService(BankingSystemDbContext bankingSystemDbContext)
    {
        _bankingSystemDbContext = bankingSystemDbContext;
        if (_bankingSystemDbContext.Database.CanConnect())
        {
            if (!_bankingSystemDbContext.Currencies.Any())
            {
                _bankingSystemDbContext.Currencies.AddRange(TestDataGenerator.GenerateListOfCurrencies());
                SaveChanges().GetAwaiter().GetResult();
            }

            _defaultCurrency = _bankingSystemDbContext.Currencies.ToList().Find(currency => currency.Code == "USD");

            if (_defaultCurrency == null)
                throw new ValueNotFoundException("Failed to get default currency!");
        }
        else
        {
            throw new DatabaseNotConnectedException("Failed to establish a connection to the database!");
        }
    }

    public async Task<Client> AddClient(ClientDto clientDto)
    {
        var client = MapDtoToClient(clientDto);

        await ValidateClient(client);

        var defaultAccount = CreateAccount(client, _defaultCurrency);

        if (defaultAccount == null)
            throw new InvalidOperationException("Failed to create default account!");

        await _bankingSystemDbContext.Clients.AddAsync(client);
        await _bankingSystemDbContext.Accounts.AddAsync(defaultAccount);

        await SaveChanges();

        return client;
    }

    public async Task<Client> UpdateClient(Guid clientId, ClientDto clientDto)
    {
        var client =
            await _bankingSystemDbContext.Clients.SingleOrDefaultAsync(client => client.ClientId.Equals(clientId));

        if (client == null)
            throw new ArgumentException($"The client with identifier {clientId} does not exist!",
                nameof(clientId));

        client = MapDtoToClient(clientDto, client);

        await ValidateClient(client, true);

        await SaveChanges();

        return client;
    }

    public async Task DeleteClient(Guid clientId)
    {
        var bankClient =
            await _bankingSystemDbContext.Clients.SingleOrDefaultAsync(client => client.ClientId.Equals(clientId));

        if (bankClient == null)
            throw new ArgumentException($"The client with identifier {clientId} does not exist!", nameof(clientId));

        var clientAccounts = await _bankingSystemDbContext.Accounts.Where(account => account.ClientId.Equals(clientId))
            .ToListAsync();

        _bankingSystemDbContext.Accounts.RemoveRange(clientAccounts);
        _bankingSystemDbContext.Clients.Remove(bankClient);

        await SaveChanges();
    }

    private async Task SaveChanges()
    {
        await _bankingSystemDbContext.SaveChangesAsync();
    }

    public async Task<Client?> GetClientById(Guid clientId)
    {
        return await _bankingSystemDbContext.Clients.SingleOrDefaultAsync(client => client.ClientId.Equals(clientId));
    }

    private static Account? CreateAccount(Client client, Currency? currency, decimal amount = 0)
    {
        return currency != null ? TestDataGenerator.GenerateRandomBankClientAccount(currency, client, amount) : null;
    }

    private async Task ValidateClient(Client client, bool itUpdate = false)
    {
        if (!itUpdate && await _bankingSystemDbContext.Clients.ContainsAsync(client))
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

        var age = TestDataGenerator.CalculateAge(client.DateOfBirth);

        if (age < 18)
            throw new PropertyValidationException("The client is under 18 years old!", nameof(client.Age),
                nameof(Client));

        if (age != client.Age || client.Age <= 0) client.Age = TestDataGenerator.CalculateAge(client.DateOfBirth);
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