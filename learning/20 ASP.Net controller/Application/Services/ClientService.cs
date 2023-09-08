using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Models.DTO;
using BankingSystemServices.Services;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class ClientService
{
    private BankingSystemDbContext _bankingSystemDbContext;
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
                throw new CustomException("Failed to get default currency!", nameof(_defaultCurrency));
        }
        else
        {
            throw new CustomException("Failed to establish a connection to the database!");
        }
    }

    public async Task<Client> AddClient(ClientDto clientDto)
    {
        var client = MapDtoToClient(clientDto);

        await ValidateClient(client);

        var defaultAccount = CreateAccount(client, _defaultCurrency);

        if (defaultAccount == null)
            throw new CustomException("Failed to create default account!", nameof(defaultAccount));

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
            throw new CustomException($"The client with identifier {clientId} does not exist!",
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
            throw new CustomException($"The client with identifier {clientId} does not exist!", nameof(clientId));

        var clientAccounts = await _bankingSystemDbContext.Accounts.Where(account => account.ClientId == clientId)
            .ToListAsync();

        _bankingSystemDbContext.Accounts.RemoveRange(clientAccounts);
        _bankingSystemDbContext.Clients.Remove(bankClient);

        await SaveChanges();
    }
    
    private async Task SaveChanges()
    {
        try
        {
            await _bankingSystemDbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            throw new CustomException(exception.Message, nameof(_bankingSystemDbContext));
        }
    }
    
    public async Task<Client?> GetClientById(Guid clientId)
    {
        return await _bankingSystemDbContext.Clients.SingleOrDefaultAsync(client => client.ClientId.Equals(clientId));
    }

    private static Account? CreateAccount(Client client, Currency? currency, decimal amount = 0)
    {
        if (currency != null)
            return TestDataGenerator.GenerateRandomBankClientAccount(currency, client, amount);
        return null;
    }

    private async Task ValidateClient(Client client, bool itUpdate = false)
    {
        if (!itUpdate && await _bankingSystemDbContext.Clients.ContainsAsync(client))
            throw new CustomException("This client has already been added to the banking system!", nameof(client));

        if (string.IsNullOrWhiteSpace(client.FirstName))
            throw new CustomException("Client first name not specified!", nameof(client.FirstName));

        if (string.IsNullOrWhiteSpace(client.LastName))
            throw new CustomException("The client last name not specified!", nameof(client.LastName));

        if (string.IsNullOrWhiteSpace(client.PhoneNumber))
            throw new CustomException("Client number not specified!", nameof(client.PhoneNumber));

        if (string.IsNullOrWhiteSpace(client.Email))
            throw new CustomException("Client e-mail not specified!", nameof(client.Email));

        if (string.IsNullOrWhiteSpace(client.Address))
            throw new CustomException("Client address not specified!", nameof(client.Address));

        if (client.DateOfBirth > DateTime.Now || client.DateOfBirth.Equals(DateTime.MinValue) ||
            client.DateOfBirth.Equals(DateTime.MaxValue))
            throw new CustomException("The client date of birth is incorrect!", nameof(client.DateOfBirth));

        var age = TestDataGenerator.CalculateAge(client.DateOfBirth);

        if (age < 18)
            throw new CustomException("The client is under 18 years old!", nameof(client.Age));

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