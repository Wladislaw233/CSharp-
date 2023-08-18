using Models;

namespace Services.Storage;

public class ClientStorage : IClientStorage
{
    private readonly List<Currency> _listOfCurrencies = new()
    {
        new Currency("USD", "US Dollar", 1),
        new Currency("EUR", "Euro", 0.97),
        new Currency("RUB", "Russian ruble", 96.64)
    };
    
    private int _accountCounter;
    public Dictionary<Client, List<Account>> Data { get; } = new();
    
    public void Add(Client client)
    {
        ClientService.ValidateClient(Data, client);
        _accountCounter++;
        Data[client] = ClientService.CreateDefaultAccount(_accountCounter, _listOfCurrencies.FirstOrDefault());
    }

    public void Update(Client oldClient, Client newClient)
    {
        ClientService.ValidateClient(Data, newClient);
        ClientService.BeforeDeletingClient(Data, oldClient);
        Data.Remove(oldClient);
        _accountCounter++;
        Data[newClient] = ClientService.CreateDefaultAccount(_accountCounter, _listOfCurrencies.FirstOrDefault());
    }

    public void Delete(Client client)
    {
        ClientService.BeforeDeletingClient(Data, client);
        Data.Remove(client);
    }

    public void AddAccount(Client client, string currencyCode = "USD", double amount = 0)
    {
        var currency = ClientService.BeforeAddingClientAccount(Data, client, _listOfCurrencies, currencyCode);
        _accountCounter++;
        Data[client].Add(new Account(ClientService.GenerateAccountNumber(currency, _accountCounter), currency, amount));
    }

    public void UpdateAccount(Client client, string accountNumber, string currencyCode, double amount)
    {
        ClientService.UpdateClientAccount(Data, client, accountNumber, _listOfCurrencies, currencyCode, amount);
    }
    
    public void DeleteAccount(Client client, string accountNumber)
    {
        var deletedAccount = ClientService.BeforeDeletingAccount(Data, client, accountNumber);
        Data[client].Remove(deletedAccount);
    }
    
    public void WithdrawBankCurrencies()
    {
        Console.WriteLine("\nВалюты банка:\n" + string.Join('\n',
            _listOfCurrencies.Select(currency => $"{currency.Code} {currency.Name} {currency.ExchangeRate}")));
    }
}