using BankingSystemServices.Models;
using BankingSystemServices.Services;
using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;

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
            if (_bankingSystemDbContext.Currencies.FirstOrDefault() == null)
            {
                _bankingSystemDbContext.Currencies.AddRange(TestDataGenerator.GenerateListOfCurrencies());
                SaveChanges();
            }

            _defaultCurrency = _bankingSystemDbContext.Currencies.ToList().Find(currency => currency.Code == "USD");
            if (_defaultCurrency == null)
                throw new CustomException("Не удалось получить валюту по умолчанию!", nameof(_defaultCurrency));
        }
        else
            throw new CustomException("Не удалось установить соединение с базой данных!");
    }

    public void AddClient(Client client)
    {
        ValidateClient(client);
        var defaultAccount = CreateAccount(client, _defaultCurrency);
        if (defaultAccount == null)
            throw new CustomException("Не удалось создать аккаунт по умолчанию!", nameof(defaultAccount));
        _bankingSystemDbContext.Clients.Add(client);
        _bankingSystemDbContext.Accounts.Add(defaultAccount);
        SaveChanges();
    }

    public void UpdateClient(Guid clientId, string? firstName = null, string? lastName = null, int? age = null,
        DateTime? dateOfBirth = null, string? phoneNumber = null, string? address = null, string? email = null,
        decimal? bonus = null)
    {
        var client =
            _bankingSystemDbContext.Clients.SingleOrDefault(client => client.ClientId.Equals(clientId));

        if (client == null)
            throw new CustomException($"Клиента с идентификатором {clientId} не существует!",
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

        ValidateClient(client, true);
        _bankingSystemDbContext.Clients.Update(client);
        SaveChanges();
    }

    public void DeleteClient(Guid clientId)
    {
        var bankClient = _bankingSystemDbContext.Clients.SingleOrDefault(client => client.ClientId.Equals(clientId));
        if (bankClient == null)
            throw new CustomException($"Клиента с идентификатором {clientId} не существует!", nameof(clientId));
        var clientAccounts = _bankingSystemDbContext.Accounts.Where(account => account.ClientId == clientId).ToList();
        _bankingSystemDbContext.Accounts.RemoveRange(clientAccounts);
        _bankingSystemDbContext.Clients.Remove(bankClient);
        SaveChanges();
    }

    public void AddClientAccount(Guid clientId, string currencyCode, decimal amount)
    {
        var client = _bankingSystemDbContext.Clients.SingleOrDefault(client => client.ClientId.Equals(clientId));
        if (client == null)
            throw new CustomException($"Клиента с идентификатором {clientId} не существует!", nameof(clientId));
        var currency = _bankingSystemDbContext.Currencies.SingleOrDefault(currency => currency.Code == currencyCode);
        if (currency == null)
            throw new CustomException($"Валюты с кодом {currencyCode} не существует!", nameof(currencyCode));
        var account = CreateAccount(client, currency, amount);
        if (account == null)
            throw new CustomException("Не удалось создать аккаунт!", nameof(account));
        _bankingSystemDbContext.Accounts.Add(account);
        SaveChanges();
    }

    public void UpdateClientAccount(Guid accountId, string currencyCode = "", decimal? amount = null)
    {
        var account = _bankingSystemDbContext.Accounts.SingleOrDefault(account => account.AccountId.Equals(accountId));
        if (account == null)
            throw new CustomException($"Лицевого счета с идентификатором {accountId} не существует!");

        if (!string.IsNullOrWhiteSpace(currencyCode))
        {
            var currency =
                _bankingSystemDbContext.Currencies.SingleOrDefault(currency => currency.Code == currencyCode);
            if (currency == null)
                throw new CustomException($"В банке нет переданной валюты ({currencyCode})!", nameof(currencyCode));

            account.AccountNumber = account.AccountNumber.Remove(account.AccountNumber.Length - 3) + currencyCode;
            account.CurrencyId = currency.CurrencyId;
        }

        if (amount != null)
            account.Amount = (decimal)amount;
        SaveChanges();
    }

    public void DeleteClientAccount(Guid accountId)
    {
        var account = _bankingSystemDbContext.Accounts.SingleOrDefault(account => account.AccountId.Equals(accountId));
        if (account == null)
            throw new CustomException($"Лицевого счета с идентификатором {accountId} не существует!");
        _bankingSystemDbContext.Accounts.Remove(account);
        SaveChanges();
    }

    private void SaveChanges()
    {
        try
        {
            _bankingSystemDbContext.SaveChanges();
        }
        catch (Exception exception)
        {
            throw new CustomException(exception.Message, nameof(_bankingSystemDbContext));
        }
    }

    public string GetPresentationClientAccounts(Guid clientId)
    {
        var clientAccounts = (from account in _bankingSystemDbContext.Accounts
            join currency in _bankingSystemDbContext.Currencies on account.CurrencyId equals currency.CurrencyId
            where account.ClientId.Equals(clientId)
            select $"Номер счета: {account.AccountNumber}, остаток: {account.Amount} {currency.Code}").ToList();

        return string.Join("\n", clientAccounts);
    }

    public List<Account> GetClientAccounts(Guid clientId)
    {
        return _bankingSystemDbContext.Accounts.Where(account => account.ClientId.Equals(clientId)).ToList();
    }

    private Account? CreateAccount(Client client, Currency? currency, decimal amount = 0)
    {
        if (currency != null) 
            return TestDataGenerator.GenerateRandomBankClientAccount(currency, client, amount);
        return null;
    }

    private void ValidateClient(Client client, bool itUpdate = false)
    {
        if (!itUpdate && _bankingSystemDbContext.Clients.Contains(client))
            throw new CustomException("Данный клиент уже добавлен в банковскую систему!", nameof(client));
        if (string.IsNullOrWhiteSpace(client.FirstName))
            throw new CustomException("Не указано имя клиента!", nameof(client.FirstName));
        if (string.IsNullOrWhiteSpace(client.LastName))
            throw new CustomException("Не указана фамилия клиента!", nameof(client.LastName));
        if (string.IsNullOrWhiteSpace(client.PhoneNumber))
            throw new CustomException("Не указан номер клиента!", nameof(client.PhoneNumber));
        if (string.IsNullOrWhiteSpace(client.Email))
            throw new CustomException("Не указан e-mail клиента!", nameof(client.Email));
        if (string.IsNullOrWhiteSpace(client.Address))
            throw new CustomException("Не указан адрес клиента!", nameof(client.Email));

        if (client.DateOfBirth > DateTime.Now || client.DateOfBirth == DateTime.MinValue ||
            client.DateOfBirth == DateTime.MaxValue)
            throw new CustomException("Дата рождения клиента указана неверно!", nameof(client.DateOfBirth));

        var age = TestDataGenerator.CalculateAge(client.DateOfBirth);

        if (age < 18)
            throw new CustomException("Клиенту меньше 18 лет!", nameof(client.Age));

        if (age != client.Age || client.Age <= 0)
        {
            client.Age = TestDataGenerator.CalculateAge(client.DateOfBirth);
            Console.WriteLine("Возраст клиента указан неверно и был скорректирован по дате его рождения!");
        }
    }

    public List<Client> ClientsWithFilterAndPagination(int page, int pageSize, string? firstName = null,
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

        return query.ToList();
    }
}