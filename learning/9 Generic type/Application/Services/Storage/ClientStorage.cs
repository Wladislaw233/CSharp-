using System.Collections;
using System.Transactions;
using BankingSystemServices.Models;
using BankingSystemServices.Services;
using BankingSystemServices.Exceptions;
using Npgsql;

namespace Services.Storage;

public class ClientStorage : IClientStorage, IEnumerable<Client>
{
    public readonly List<Currency> ListOfCurrencies = TestDataGenerator.GenerateListOfCurrencies();
    private readonly Currency _defaultCurrency;
    public Dictionary<Client, List<Account>> ClientWithAccountsList { get; } = new();

    public ClientStorage()
    {
        var currency = ListOfCurrencies.Find(currency => currency.Code == "USD");
        if (currency != null)
            _defaultCurrency = currency;
        else
            throw new ValueNotFoundException("Не удалось установить валюту по умолчанию!");
    }

    public void Add(Client client)
    {
        var defaultAccount = TestDataGenerator.GenerateRandomBankClientAccount(,);
        ClientWithAccountsList.Add(client, );
    }

    public void Update(Client client, Client newClient)
    {
        
       /* if (!Data.ContainsKey(client))
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
        
        ClientService.ValidateClient(client);*/
    }

    public void Delete(Client client)
    {
        /*if (!Data.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));
        Data.Remove(client);*/
    }

    public void AddAccount(Client client, Account account)
    {
        
        Data[client].Add(TestDataGenerator.GenerateRandomBankClientAccount(currency, client, amount));
    }

    public void UpdateAccount(Account account, Account newAccount)
    {
        /*if (!Data.ContainsKey(client))
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
            account.Amount += (decimal)amount;*/
    }

    public void DeleteAccount(Account account)
    {
        /*if (!Data.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));

        var account = Data[client].Find(foundAccount => foundAccount.AccountNumber == accountNumber);

        if (account == null)
            throw new CustomException($"У клиента нет счета с номером {accountNumber}!", nameof(accountNumber));

        Data[client].Remove(account);*/
    }
    
    public IEnumerator<Client> GetEnumerator()
    {
        return ClientWithAccountsList.Keys.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}