using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Services;

namespace Services;

public class ClientService
{
    private readonly BankingSystemDbContext _bankingSystemDbContext;
    private Currency? _defaultCurrency;

    public ClientService(BankingSystemDbContext bankingSystemDbContext)
    {
        _bankingSystemDbContext = bankingSystemDbContext;
    }

    public void AddClient(Client client)
    {
        ValidateClient(client);

        _defaultCurrency ??= GetDefaultCurrency();

        var defaultAccount = CreateAccount(client, _defaultCurrency);
        
        _bankingSystemDbContext.Clients.Add(client);
        _bankingSystemDbContext.Accounts.Add(defaultAccount);

        _bankingSystemDbContext.SaveChanges();
    }

    public void UpdateClient(Guid clientId, Client newClient)
    {
        var client = GetClientById(clientId);

        ValidateClient(newClient, true);

        client.FirstName = newClient.FirstName;
        client.LastName = newClient.LastName;
        client.Age = newClient.Age;
        client.DateOfBirth = newClient.DateOfBirth.ToUniversalTime();
        client.PhoneNumber = newClient.PhoneNumber;
        client.Address = newClient.Address;
        client.Email = newClient.Email;
        client.Bonus = newClient.Bonus;

        _bankingSystemDbContext.SaveChanges();
    }

    public void DeleteClient(Guid clientId)
    {
        var bankClient = GetClientById(clientId);

        var clientAccounts = GetClientAccounts(clientId);

        _bankingSystemDbContext.Accounts.RemoveRange(clientAccounts);
        _bankingSystemDbContext.Clients.Remove(bankClient);

        _bankingSystemDbContext.SaveChanges();
    }

    public void AddClientAccount(Guid clientId, string currencyCode, decimal amount)
    {
        var client = GetClientById(clientId);

        var currency = GetCurrencyByCurrencyCode(currencyCode);

        var account = CreateAccount(client, currency, amount);
        
        _bankingSystemDbContext.Accounts.Add(account);

        _bankingSystemDbContext.SaveChanges();
    }

    public void UpdateClientAccount(Guid accountId, string currencyCode = "", decimal? amount = null)
    {
        var account = GetAccountById(accountId);

        if (!string.IsNullOrWhiteSpace(currencyCode))
        {
            var currency = GetCurrencyByCurrencyCode(currencyCode);

            account.CurrencyId = currency.CurrencyId;
            account.Currency = currency;
        }

        if (amount != null)
            account.Amount = (decimal)amount;

        _bankingSystemDbContext.SaveChanges();
    }

    public void DeleteClientAccount(Guid accountId)
    {
        var account = GetAccountById(accountId);

        _bankingSystemDbContext.Accounts.Remove(account);

        _bankingSystemDbContext.SaveChanges();
    }

    public string GetPresentationClientAccounts(Guid clientId)
    {
        var clientAccounts = (from account in _bankingSystemDbContext.Accounts
            join currency in _bankingSystemDbContext.Currencies on account.CurrencyId equals currency.CurrencyId
            where account.ClientId.Equals(clientId)
            select $"Account number: {account.AccountNumber}, balance: {account.Amount} {currency.Code}").ToList();

        return string.Join("\n", clientAccounts);
    }

    public IEnumerable<Account> GetClientAccounts(Guid clientId)
    {
        return _bankingSystemDbContext.Accounts.Where(account => account.ClientId.Equals(clientId)).ToList();
    }

    private static Account CreateAccount(Client client, Currency currency, decimal amount = 0)
    {
        return TestDataGenerator.GenerateBankClientAccount(currency, client, amount);
    }

    private Client GetClientById(Guid clientId)
    {
        var client = _bankingSystemDbContext.Clients.SingleOrDefault(client => client.ClientId.Equals(clientId));

        if (client == null)
            throw new ValueNotFoundException($"The client with identifier {clientId} does not exist!");

        return client;
    }

    private Account GetAccountById(Guid accountId)
    {
        var account = _bankingSystemDbContext.Accounts.SingleOrDefault(account => account.AccountId.Equals(accountId));

        if (account == null)
            throw new ValueNotFoundException($"There is no personal account with ID {accountId}.");

        return account;
    }

    private Currency GetCurrencyByCurrencyCode(string currencyCode)
    {
        var currency =
            _bankingSystemDbContext.Currencies.SingleOrDefault(currency => currency.Code == currencyCode);

        if (currency == null)
            throw new ValueNotFoundException($"The bank does not have the transferred currency ({currencyCode}).");

        return currency;
    }

    private Currency GetDefaultCurrency()
    {
        var currency = _bankingSystemDbContext.Currencies.SingleOrDefault(currency => currency.Code == "USD");

        if (currency == null)
            throw new ValueNotFoundException("Default currency not found.");

        return currency;
    }

    private void ValidateClient(Client client, bool itUpdate = false)
    {
        if (!itUpdate && _bankingSystemDbContext.Clients.Any(c => c.ClientId.Equals(client.ClientId)))
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

    public IEnumerable<Client> ClientsWithFilterAndPagination(int page, int pageSize, string? firstName = null,
        string? lastName = null, int? age = null,
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

        query = query.OrderBy(client => client.FirstName);

        query = query.Skip((page - 1) * pageSize).Take(pageSize);

        return query.ToList();
    }
}