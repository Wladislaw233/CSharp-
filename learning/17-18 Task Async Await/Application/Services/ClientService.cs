using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
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

    public async Task AddClient(Client client)
    {
        await ValidateClient(client);

        var defaultAccount = CreateAccount(client, _defaultCurrency);

        if (defaultAccount == null)
            throw new InvalidOperationException("Failed to create default account!");

        await _bankingSystemDbContext.Clients.AddAsync(client);
        await _bankingSystemDbContext.Accounts.AddAsync(defaultAccount);

        await SaveChanges();
    }

    public async Task UpdateClient(Guid clientId, string? firstName = null, string? lastName = null, int? age = null,
        DateTime? dateOfBirth = null, string? phoneNumber = null, string? address = null, string? email = null,
        decimal? bonus = null)
    {
        var client =
            await _bankingSystemDbContext.Clients.SingleOrDefaultAsync(client => client.ClientId.Equals(clientId));

        if (client == null)
            throw new ArgumentException($"The client with identifier {clientId} does not exist!",
                nameof(clientId));

        if (firstName != null)
            client.FirstName = firstName;

        if (lastName != null)
            client.LastName = lastName;

        if (age != null)
            client.Age = (int)age;

        if (dateOfBirth != null)
            client.DateOfBirth = ((DateTime)dateOfBirth).ToUniversalTime();

        if (phoneNumber != null)
            client.PhoneNumber = phoneNumber;

        if (address != null)
            client.Address = address;

        if (email != null)
            client.Email = email;

        if (bonus != null)
            client.Bonus = (decimal)bonus;

        await ValidateClient(client, true);
        await SaveChanges();
    }

    public async Task DeleteClient(Guid clientId)
    {
        var bankClient =
            await _bankingSystemDbContext.Clients.SingleOrDefaultAsync(client => client.ClientId.Equals(clientId));

        if (bankClient == null)
            throw new ArgumentException($"The client with identifier {clientId} does not exist!", nameof(clientId));

        var clientAccounts = await _bankingSystemDbContext.Accounts.Where(account => account.ClientId == clientId)
            .ToListAsync();

        _bankingSystemDbContext.Accounts.RemoveRange(clientAccounts);
        _bankingSystemDbContext.Clients.Remove(bankClient);

        await SaveChanges();
    }

    public async Task AddClientAccount(Guid clientId, string currencyCode, decimal amount)
    {
        var client =
            await _bankingSystemDbContext.Clients.SingleOrDefaultAsync(client => client.ClientId.Equals(clientId));

        if (client == null)
            throw new ArgumentException($"The client with identifier {clientId} does not exist!", nameof(clientId));

        var currency =
            await _bankingSystemDbContext.Currencies.SingleOrDefaultAsync(currency => currency.Code == currencyCode);

        if (currency == null)
            throw new ArgumentException($"There is no currency with {currencyCode}!", nameof(currencyCode));

        var account = CreateAccount(client, currency, amount);

        if (account == null)
            throw new InvalidOperationException("Failed to create account!");

        await _bankingSystemDbContext.Accounts.AddAsync(account);

        await SaveChanges();
    }

    public async Task UpdateClientAccount(Guid accountId, string currencyCode = "", decimal? amount = null)
    {
        var account =
            await _bankingSystemDbContext.Accounts.SingleOrDefaultAsync(account => account.AccountId.Equals(accountId));

        if (account == null)
            throw new ArgumentException($"There is no personal account with ID {accountId}.", nameof(accountId));

        if (!string.IsNullOrWhiteSpace(currencyCode))
        {
            var currency =
                await _bankingSystemDbContext.Currencies.SingleOrDefaultAsync(currency =>
                    currency.Code == currencyCode);

            if (currency == null)
                throw new ArgumentException($"The bank does not have the transferred currency ({currencyCode}).",
                    nameof(currencyCode));

            account.AccountNumber = account.AccountNumber.Remove(account.AccountNumber.Length - 3) + currencyCode;
            account.CurrencyId = currency.CurrencyId;
        }

        if (amount != null)
            account.Amount = (decimal)amount;

        await SaveChanges();
    }

    public async Task DeleteClientAccount(Guid accountId)
    {
        var account =
            await _bankingSystemDbContext.Accounts.SingleOrDefaultAsync(account => account.AccountId.Equals(accountId));

        if (account == null)
            throw new ArgumentException($"There is no personal account with ID {accountId}.", nameof(accountId));

        _bankingSystemDbContext.Accounts.Remove(account);

        await SaveChanges();
    }

    private async Task SaveChanges()
    {
        await _bankingSystemDbContext.SaveChangesAsync();
    }

    public async Task<string> GetPresentationClientAccounts(Guid clientId)
    {
        var clientAccounts = await (from account in _bankingSystemDbContext.Accounts
            join currency in _bankingSystemDbContext.Currencies on account.CurrencyId equals currency.CurrencyId
            where account.ClientId.Equals(clientId)
            select $"Account number: {account.AccountNumber}, balance: {account.Amount} {currency.Code}").ToListAsync();

        return string.Join("\n", clientAccounts);
    }

    public async Task<List<Account>> GetClientAccounts(Guid clientId)
    {
        return await _bankingSystemDbContext.Accounts.Where(account => account.ClientId.Equals(clientId)).ToListAsync();
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

    public async Task<List<Client>> ClientsWithFilterAndPagination(int page, int pageSize, string? firstName = null,
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
        var resultQuery = await query.ToListAsync();
        return resultQuery;
    }
}