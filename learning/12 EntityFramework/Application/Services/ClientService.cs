using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Services;

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
            if (_bankingSystemDbContext.Currencies.FirstOrDefault() == null)
            {
                _bankingSystemDbContext.Currencies.AddRange(TestDataGenerator.GenerateListOfCurrencies());
                SaveChanges();
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

    public void AddClient(Client client)
    {
        ValidateClient(client, false);
        var defaultAccount = CreateAccount(client, _defaultCurrency);

        if (defaultAccount == null)
            throw new InvalidOperationException("Failed to create default account!");

        _bankingSystemDbContext.Clients.Add(client);
        _bankingSystemDbContext.Accounts.Add(defaultAccount);

        SaveChanges();
    }

    public void UpdateClient(Guid clientId, string? firstName = null, string? lastName = null, int? age = null,
        DateTime? dateOfBirth = null, string? phoneNumber = null, string? address = null, string? email = null,
        decimal? bonus = null)
    {
        var client = GetClientById(clientId);

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

        ValidateClient(client, true);
        SaveChanges();
    }
    
    public void DeleteClient(Guid clientId)
    {
        var bankClient = GetClientById(clientId);

        var clientAccounts = GetClientAccounts(clientId);

        _bankingSystemDbContext.Accounts.RemoveRange(clientAccounts);
        _bankingSystemDbContext.Clients.Remove(bankClient);

        SaveChanges();
    }

    public void AddClientAccount(Guid clientId, string currencyCode, decimal amount)
    {
        var client = GetClientById(clientId);

        var currency = GetCurrencyByCurrencyCode(currencyCode);

        var account = CreateAccount(client, currency, amount);

        if (account == null)
            throw new InvalidOperationException("Failed to create account!");

        _bankingSystemDbContext.Accounts.Add(account);

        SaveChanges();
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
        SaveChanges();
    }

    public void DeleteClientAccount(Guid accountId)
    {
        var account = GetAccountById(accountId);

        _bankingSystemDbContext.Accounts.Remove(account);

        SaveChanges();
    }

    private void SaveChanges()
    {
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

    public List<Account> GetClientAccounts(Guid clientId)
    {
        return _bankingSystemDbContext.Accounts.Where(account => account.ClientId.Equals(clientId)).ToList();
    }

    private Account? CreateAccount(Client client, Currency? currency, decimal amount = 0)
    {
        return currency != null ? TestDataGenerator.GenerateRandomBankClientAccount(currency, client, amount) : null;
    }

    private Client GetClientById(Guid clientId)
    {
        var client = _bankingSystemDbContext.Clients.SingleOrDefault(client => client.ClientId.Equals(clientId));

        if (client == null)
            throw new ArgumentException($"The client with identifier {clientId} does not exist!", nameof(clientId));

        return client;
    }

    private Account GetAccountById(Guid accountId)
    {
        var account = _bankingSystemDbContext.Accounts.SingleOrDefault(account => account.AccountId.Equals(accountId));

        if (account == null)
            throw new ArgumentException($"There is no personal account with ID {accountId}.", nameof(accountId));

        return account;
    }

    private Currency GetCurrencyByCurrencyCode(string currencyCode)
    {
        var currency =
            _bankingSystemDbContext.Currencies.SingleOrDefault(currency => currency.Code == currencyCode);
        
        if (currency == null)
            throw new ArgumentException($"The bank does not have the transferred currency ({currencyCode}).",
                nameof(currencyCode));

        return currency;
    }
    
    private void ValidateClient(Client client, bool itUpdate)
    {
        if (!itUpdate && _bankingSystemDbContext.Clients.Contains(client))
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

    public List<Client> ClientsWithFilterAndPagination(int page, int pageSize, string? firstName = null,
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