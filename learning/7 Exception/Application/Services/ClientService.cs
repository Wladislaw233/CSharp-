using BankingSystemServices.Services;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;

namespace Services;

public class ClientService
{
    private readonly Dictionary<Client, List<Account>> _clientsAccounts = new();

    private readonly List<Currency> _listOfCurrencies = TestDataGenerator.GenerateListOfCurrencies();

    public void AddClient(Client client)
    {
        ValidateClient(client);
        CreateDefaultAccount(client);
    }

    public void AddClientAccount(Client client, string currencyCode = "USD", decimal amount = 0)
    {
        if (!_clientsAccounts.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));

        var currency = _listOfCurrencies.Find(foundCurrency => foundCurrency.Code == currencyCode);
        
        if (currency == null)
            throw new CustomException($"В банке нет переданной валюты ({currencyCode})!", nameof(currencyCode));

        var clientAccount = TestDataGenerator.GenerateBankClientAccount(currency, client, amount);
        
        _clientsAccounts[client].Add(clientAccount);
    }

    public List<Account> GetClientAccounts(Client client)
    {
        if (!_clientsAccounts.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));
        return _clientsAccounts[client];
    }

    public void UpdateClientAccount(Client client, string accountNumber, string? currencyCode = null,
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
            account.Amount = (decimal)amount;
    }

    private void CreateDefaultAccount(Client client)
    {
        var currency = _listOfCurrencies.FirstOrDefault();
        if (currency == null)
            throw new CustomException("В банковской системе нет валют!", nameof(currency));

        var defaultAccount = TestDataGenerator.GenerateBankClientAccount(currency, client);
        var clientAccountList = new List<Account> { defaultAccount };
        _clientsAccounts.TryAdd(client, clientAccountList);
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

        var mess = string.Join('\n',
            _clientsAccounts[client].Select(clientAccount =>
                $"Номер счета: {clientAccount.AccountNumber}, валюта: {clientAccount.Currency.Name}, " +
                $"баланс: {clientAccount.Amount} {clientAccount.Currency.Code}"));
        
        Console.WriteLine(mess);
    }
}