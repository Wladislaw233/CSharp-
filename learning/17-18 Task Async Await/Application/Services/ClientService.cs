using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class ClientService
{
    private readonly BankingSystemDbContext _bankingSystemDbContext;
    private Currency? _defaultCurrency;

    public ClientService(BankingSystemDbContext bankingSystemDbContext)
    {
        _bankingSystemDbContext = bankingSystemDbContext;
    }

    public async Task AddClientAsync(Client client)
    {
        await ValidateClientAsync(client);

        _defaultCurrency ??= await GetDefaultCurrencyAsync();

        var defaultAccount = await CreateAccount(client, _defaultCurrency);

        await _bankingSystemDbContext.Clients.AddAsync(client);
        await _bankingSystemDbContext.Accounts.AddAsync(defaultAccount);

        await _bankingSystemDbContext.SaveChangesAsync();
    }

    public async Task UpdateClientAsync(Guid clientId, Client newClient)
    {
        var client = await GetClientByIdAsync(clientId);

        await ValidateClientAsync(newClient);

        client.FirstName = newClient.FirstName;
        client.LastName = newClient.LastName;
        client.Age = newClient.Age;
        client.DateOfBirth = newClient.DateOfBirth.ToUniversalTime();
        client.PhoneNumber = newClient.PhoneNumber;
        client.Address = newClient.Address;
        client.Email = newClient.Email;
        client.Bonus = newClient.Bonus;

        await _bankingSystemDbContext.SaveChangesAsync();
    }

    public async Task DeleteClientAsync(Guid clientId)
    {
        var bankClient = await GetClientByIdAsync(clientId);

        var clientAccounts = await GetClientAccountsAsync(clientId);

        _bankingSystemDbContext.Accounts.RemoveRange(clientAccounts);
        _bankingSystemDbContext.Clients.Remove(bankClient);

        await _bankingSystemDbContext.SaveChangesAsync();
    }

    public async Task AddClientAccountAsync(Guid clientId, string currencyCode, decimal amount)
    {
        var client = await GetClientByIdAsync(clientId);

        var currency = await GetCurrencyByCurrencyCodeAsync(currencyCode);

        var account = await CreateAccount(client, currency, amount);
        
        await _bankingSystemDbContext.Accounts.AddAsync(account);

        await _bankingSystemDbContext.SaveChangesAsync();
    }

    public async Task UpdateClientAccountAsync(Guid accountId, string currencyCode = "", decimal? amount = null)
    {
        var account = await GetAccountByIdAsync(accountId);

        if (!string.IsNullOrWhiteSpace(currencyCode))
        {
            var currency = await GetCurrencyByCurrencyCodeAsync(currencyCode);

            account.CurrencyId = currency.CurrencyId;
            account.Currency = currency;
        }

        if (amount != null)
            account.Amount = (decimal)amount;

        await _bankingSystemDbContext.SaveChangesAsync();
    }

    public async Task DeleteClientAccountAsync(Guid accountId)
    {
        var account = await GetAccountByIdAsync(accountId);

        _bankingSystemDbContext.Accounts.Remove(account);

        await _bankingSystemDbContext.SaveChangesAsync();
    }

    private async Task<Currency> GetDefaultCurrencyAsync()
    {
        var currency =
            await _bankingSystemDbContext.Currencies.SingleOrDefaultAsync(currency => currency.Code == "USD");

        if (currency == null)
            throw new ValueNotFoundException("Default currency USD not found.");

        return currency;
    }

    public async Task<string> GetPresentationClientAccountsAsync(Guid clientId)
    {
        var clientAccounts = await (from account in _bankingSystemDbContext.Accounts
            join currency in _bankingSystemDbContext.Currencies on account.CurrencyId equals currency.CurrencyId
            where account.ClientId.Equals(clientId)
            select $"Account number: {account.AccountNumber}, balance: {account.Amount} {currency.Code}").ToListAsync();

        return string.Join("\n", clientAccounts);
    }

    public async Task<IEnumerable<Account>> GetClientAccountsAsync(Guid clientId)
    {
        return await _bankingSystemDbContext.Accounts.Where(account => account.ClientId.Equals(clientId)).ToListAsync();
    }

    private static async Task<Account> CreateAccount(Client client, Currency currency, decimal amount = 0)
    {
        return await Task.Run(() => TestDataGenerator.GenerateBankClientAccount(currency, client, amount));
    }

    private async Task<Client> GetClientByIdAsync(Guid clientId)
    {
        var client =
            await _bankingSystemDbContext.Clients.SingleOrDefaultAsync(client => client.ClientId.Equals(clientId));

        if (client == null)
            throw new ValueNotFoundException($"The client with identifier {clientId} does not exist!");

        return client;
    }

    private async Task<Account> GetAccountByIdAsync(Guid accountId)
    {
        var account =
            await _bankingSystemDbContext.Accounts.SingleOrDefaultAsync(account => account.AccountId.Equals(accountId));

        if (account == null)
            throw new ValueNotFoundException($"There is no personal account with ID {accountId}.");

        return account;
    }

    private async Task<Currency> GetCurrencyByCurrencyCodeAsync(string currencyCode)
    {
        var currency =
            await _bankingSystemDbContext.Currencies.SingleOrDefaultAsync(currency => currency.Code == currencyCode);

        if (currency == null)
            throw new ValueNotFoundException($"The bank does not have the transferred currency ({currencyCode}).");

        return currency;
    }

    private async Task ValidateClientAsync(Client client, bool itUpdate = false)
    {
        if (!itUpdate && await _bankingSystemDbContext.Clients.AnyAsync(c => c.ClientId.Equals(client.ClientId)))
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

    public async Task<List<Client>> ClientsWithFilterAndPaginationAsync(int page, int pageSize,
        string? firstName = null,
        string? lastName = null, int? age = null, Guid? clientId = null,
        DateTime? dateOfBirth = null, string? phoneNumber = null, string? address = null, string? email = null)
    {
        IQueryable<Client> query = _bankingSystemDbContext.Clients;
        if (firstName != null)
            query = query.Where(client => client.FirstName == firstName);
        if (lastName != null)
            query = query.Where(client => client.LastName == lastName);
        if (age != null)
            query = query.Where(client => client.Age.Equals((int)age));
        if (dateOfBirth != null)
            query = query.Where(client => client.DateOfBirth.Equals(((DateTime)dateOfBirth).ToUniversalTime()));
        if (phoneNumber != null)
            query = query.Where(client => client.PhoneNumber == phoneNumber);
        if (address != null)
            query = query.Where(client => client.Address == address);
        if (email != null)
            query = query.Where(client => client.Email == email);
        if (clientId != null)
            query = query.Where(client => client.ClientId.Equals(clientId));

        query = query.OrderBy(client => client.FirstName);

        query = query.Skip((page - 1) * pageSize).Take(pageSize);

        return await query.ToListAsync();
    }
}