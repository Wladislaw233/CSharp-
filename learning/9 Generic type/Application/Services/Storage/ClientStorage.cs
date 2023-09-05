using System.Collections;
using System.Transactions;
using BankingSystemServices.Models;
using BankingSystemServices.Services;
using BankingSystemServices.Exceptions;

namespace Services.Storage;

public class ClientStorage : IClientStorage, IEnumerable<Client>
{
    private readonly List<Currency> _listOfCurrencies = TestDataGenerator.GenerateListOfCurrencies();
    private readonly Currency _defaultCurrency;
    public Dictionary<Client, List<Account>> Data { get; } = new();

    public ClientStorage()
    {
        var currency = _listOfCurrencies.Find(currency => currency.Code == "USD");
        if (currency != null)
            _defaultCurrency = currency;
        else
            throw new CustomException("Не удалось установить валюту по умолчанию!", nameof(_defaultCurrency));
    }

    public void Add(Client client)
    {
        if (Data.ContainsKey(client))
            throw new CustomException("Данный клиент уже добавлен в банковскую систему!", nameof(client));

        ClientService.ValidateClient(client);
        Data[client] = new List<Account>
            { TestDataGenerator.GenerateRandomBankClientAccount(_defaultCurrency, client) };
    }

    public void Update(Client client, string? firstName = null, string? lastName = null, int? age = null,
        DateTime? dateOfBirth = null, string? phoneNumber = null, string? address = null, string? email = null,
        decimal? bonus = null)
    {
        
        if (!Data.ContainsKey(client))
            throw new CustomException(
                $"Клиента {client.FirstName} {client.LastName} не существует в банковской системе!",
                nameof(client));
        
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
        
        ClientService.ValidateClient(client);
    }

    public void Delete(Client client)
    {
        if (!Data.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));
        Data.Remove(client);
    }

    public void AddAccount(Client client, string currencyCode = "USD", decimal amount = 0)
    {
        if (!Data.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));

        var currency = _listOfCurrencies.Find(foundCurrency => foundCurrency.Code == currencyCode);
        if (currency == null)
            throw new CustomException($"В банке нет переданной валюты ({currencyCode})!", nameof(currencyCode));

        Data[client].Add(TestDataGenerator.GenerateRandomBankClientAccount(currency, client, amount));
    }

    public void UpdateAccount(Client client, string accountNumber, string? currencyCode = null,
        decimal? amount = null)
    {
        if (!Data.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));

        var account = Data[client].Find(foundAccount => foundAccount.AccountNumber == accountNumber);

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
    }

    public void DeleteAccount(Client client, string accountNumber)
    {
        if (!Data.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));

        var account = Data[client].Find(foundAccount => foundAccount.AccountNumber == accountNumber);

        if (account == null)
            throw new CustomException($"У клиента нет счета с номером {accountNumber}!", nameof(accountNumber));

        Data[client].Remove(account);
    }

    public void WithdrawBankCurrencies()
    {
        Console.WriteLine("\nВалюты банка:\n" + string.Join('\n',
            _listOfCurrencies.Select(currency => $"{currency.Code} {currency.Name} {currency.ExchangeRate}")));
    }

    public IEnumerator<Client> GetEnumerator()
    {
        return Data.Keys.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}