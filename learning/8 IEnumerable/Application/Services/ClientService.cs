using BankingSystemServices.Models;
using BankingSystemServices.Services;
using BankingSystemServices.Exceptions;
using Services.Storage;

namespace Services;

public class ClientService
{
    private readonly Dictionary<Client, List<Account>> _clientsAccounts = new();

    private readonly List<Currency> _listOfCurrencies = TestDataGenerator.GenerateListOfCurrencies();

    public List<Account> AddClient(Client client)
    {
        ValidateClient(client);
        CreateDefaultAccount(client);
        return _clientsAccounts[client];
    }

    public void AddClientAccount(Client client, string currencyCode = "USD", decimal amount = 0)
    {
        if (!_clientsAccounts.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));

        var currency = _listOfCurrencies.Find(foundCurrency => foundCurrency.Code == currencyCode);
        if (currency == null)
            throw new CustomException($"В банке нет переданной валюты ({currencyCode})!", nameof(currencyCode));

        _clientsAccounts[client].Add(TestDataGenerator.GenerateRandomBankClientAccount(currency, client, amount));
    }

    public List<Account> GetClientAccounts(Client client)
    {
        if (!_clientsAccounts.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));
        return _clientsAccounts[client];
    }

    public List<Account> UpdateClientAccount(Client client, string accountNumber, string? currencyCode = null,
        decimal? amount = null)
    {
        if (!_clientsAccounts.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));

        var account = _clientsAccounts[client].Find(foundAccount => foundAccount.AccountNumber == accountNumber);

        if (account == null)
            throw new CustomException($"У клиента нет счета с номером {accountNumber}!", nameof(accountNumber));

        if (!string.IsNullOrWhiteSpace(currencyCode))
        {
            var currency = _listOfCurrencies.Find(foundCurrency => foundCurrency.Code == currencyCode);
            if (currency == null)
                throw new CustomException($"В банке нет переданной валюты ({currencyCode})!", nameof(currencyCode));
            account.CurrencyId = currency.CurrencyId;
            account.Currency = currency;
        }

        if (amount != null)
            account.Amount += (decimal)amount;

        return _clientsAccounts[client];
    }

    private void CreateDefaultAccount(Client client)
    {
        var currency = _listOfCurrencies.FirstOrDefault();
        if (currency == null)
            throw new CustomException("В банковской системе нет валют!", nameof(currency));

        _clientsAccounts.TryAdd(client,
            new List<Account>
                { TestDataGenerator.GenerateRandomBankClientAccount(currency, client) });
    }

    private void ValidateClient(Client client)
    {
        if (_clientsAccounts.ContainsKey(client))
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
            client.Age = age;
            Console.WriteLine("Возраст клиента указан неверно и был скорректирован по дате его рождения!");
        }
    }

    public void WithdrawBankCurrencies()
    {
        Console.WriteLine("\nВалюты банка:");
        Console.WriteLine(string.Join('\n',
            _listOfCurrencies.Select(currency => $"{currency.Code} {currency.Name} {currency.ExchangeRate}")));
    }

    public void WithdrawClientAccounts(Client client)
    {
        if (!_clientsAccounts.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));

        Console.WriteLine($"Клиент: {client.FirstName} {client.LastName}, лицевые счета:");

        Console.WriteLine(string.Join('\n',
            _clientsAccounts[client].Select(clientAccount =>
                $"Номер счета: {clientAccount.AccountNumber}, валюта: {clientAccount.Currency.Name}, " +
                $"баланс: {clientAccount.Amount} {clientAccount.Currency.Code}")));
    }

    public static IEnumerable<Client> GetClientsByFilters(ClientStorage clientStorage, string firstNameFilter = "",
        string lastNameFilter = "", string phoneNumberFilter = "", DateTime? minDateOfBirth = null,
        DateTime? maxDateOfBirth = null)
    {
        IEnumerable<Client> filteredClients = clientStorage;
        if (!string.IsNullOrWhiteSpace(firstNameFilter))
            filteredClients = filteredClients.Where(client => client.FirstName == firstNameFilter);
        if (!string.IsNullOrWhiteSpace(lastNameFilter))
            filteredClients = filteredClients.Where(client => client.LastName == lastNameFilter);
        if (!string.IsNullOrWhiteSpace(phoneNumberFilter))
            filteredClients = filteredClients.Where(client => client.PhoneNumber == phoneNumberFilter);
        if (minDateOfBirth.HasValue)
            filteredClients = filteredClients.Where(client => client.DateOfBirth >= minDateOfBirth);
        if (maxDateOfBirth.HasValue)
            filteredClients = filteredClients.Where(client => client.DateOfBirth <= maxDateOfBirth);
        return filteredClients;
    }
}