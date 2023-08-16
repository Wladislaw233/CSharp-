using Models;
using Services.Exceptions;

namespace Services;

public class ClientService
{
    private int _accountCounter;
    private readonly Dictionary<Client, List<Account>> _clientsAccounts = new();

    private readonly List<Currency> _listOfCurrencies = new()
    {
        new("USD", "US Dollar", 1),
        new("EUR", "Euro", 0.97),
        new("RUB", "Russian ruble", 96.64)
    };

    private string GenerateAccountNumber(Currency currency)
    {
        _accountCounter++;
        return "ACC" + _accountCounter.ToString("D10") + currency.Code;
    }

    public List<Account> AddClient(Client client)
    {
        ValidateClient(client);
        CreateDefaultAccount(client);
        return _clientsAccounts[client];
    }

    public List<Account> AddClientAccount(Client client, string currencyCode = "USD", double amount = 0)
    {
        if (!_clientsAccounts.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));
        var currency = _listOfCurrencies.Find(foundCurrency => foundCurrency.Code == currencyCode);
        if (currency.Code == null)
            throw new CustomException($"В банке нет переданной валюты ({currencyCode})!", nameof(currencyCode));
        _clientsAccounts[client].Add(new Account(GenerateAccountNumber(currency), currency, amount));
        return _clientsAccounts[client];
    }

    public List<Account> GetClientAccounts(Client client)
    {
        if (!_clientsAccounts.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));
        return _clientsAccounts[client];
    }
    public List<Account> UpdateClientAccount(Client client, string accountNumber, string currencyCode,
        double amount)
    {
        if (!_clientsAccounts.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));
        var account = _clientsAccounts[client].Find(foundAccount => foundAccount.AccountNumber == accountNumber);
        if (account == null)
            throw new CustomException($"У клиента нет счета с номером {accountNumber}!", nameof(accountNumber));
        if (!string.IsNullOrWhiteSpace(currencyCode))
        {
            var currency = _listOfCurrencies.Find(foundCurrency => foundCurrency.Code == currencyCode);
            if (currency.Code == null)
                throw new CustomException($"В банке нет переданной валюты ({currencyCode})!", nameof(currencyCode));
            account.AccountNumber = account.AccountNumber.Replace(account.Currency.Code, currencyCode);
            account.Currency = currency;
        }

        account.Amount = amount;
        return _clientsAccounts[client];
    }

    private void CreateDefaultAccount(Client client)
    {
        var clientAccounts = new List<Account>();
        var currency = _listOfCurrencies.FirstOrDefault();
        clientAccounts.Add(new Account(GenerateAccountNumber(currency), currency));
        _clientsAccounts.Add(client, clientAccounts);
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

        if (CalculateAge(client.DateOfBirth) < 18)
            throw new CustomException("Клиенту меньше 18 лет!", nameof(client.Age));

        if (CalculateAge(client.DateOfBirth) != client.Age || client.Age <= 0)
        {
            client.Age = CalculateAge(client.DateOfBirth);
            Console.WriteLine("Возраст клиента указан неверно и был скорректирован по дате его рождения!");
        }
    }

    public void WithdrawBankCurrencies()
    {
        Console.WriteLine("\nВалюты банка:\n" + string.Join('\n',
            _listOfCurrencies.Select(currency => $"{currency.Code} {currency.Name} {currency.ExchangeRate}")));
    }
    private int CalculateAge(DateTime dateOfBirth)
    {
        return DateTime.Now.Year - dateOfBirth.Year - (dateOfBirth >
                                                       DateTime.Now.AddYears(-(DateTime.Now.Year - dateOfBirth.Year))
            ? 1
            : 0);
    }
}