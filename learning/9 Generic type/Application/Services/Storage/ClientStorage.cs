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
            throw new ValueNotFoundException("Failed to set default currency!");
    }

    public void Add(Client client)
    {
        var defaultAccount = TestDataGenerator.GenerateRandomBankClientAccount(_defaultCurrency, client, 0);
        ClientWithAccountsList.Add(client, new List<Account>(){defaultAccount});
    }

    public void Update(Client client, Client newClient)
    {
        client.FirstName = newClient.FirstName;
        client.LastName = newClient.LastName;
        client.DateOfBirth = newClient.DateOfBirth;
        client.Age = newClient.Age;
        client.Bonus = newClient.Bonus;
        client.Address = newClient.Address;
        client.PhoneNumber = newClient.PhoneNumber;
        client.Email = newClient.Email;
    }

    public void Delete(Client client)
    {
        ClientWithAccountsList.Remove(client);
    }

    public void AddAccount(Client client, Account account)
    {
        ClientWithAccountsList[client].Add(account);
    }

    public void UpdateAccount(Account account, Account newAccount)
    {
        account.Currency = newAccount.Currency;
        account.CurrencyId = newAccount.CurrencyId;
        account.Amount = newAccount.Amount;
    }

    public void DeleteAccount(Account account)
    {
        ClientWithAccountsList.Values.SelectMany(list => list).ToList().Remove(account);
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