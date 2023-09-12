using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Bogus;
using Services.Storage;
using Currency = Bogus.DataSets.Currency;

namespace Services;

public class ClientService
{
    private readonly ClientStorage _clientStorage;

    public ClientService(ClientStorage clientStorage)
    {
        _clientStorage = clientStorage;
    }

    public void AddClient(Client client)
    {
        ValidateClient(client);
        _clientStorage.Add(client);
    }

    public void UpdateClient(Guid clientId, Client newClient)
    {
        if (!ClientExistsInBankingSystem(clientId))
            throw new ArgumentException("Клиента не существует в банковской системе.", nameof(clientId));
        
        ValidateClient(newClient, true);
        
        _clientStorage.Update(clientId, newClient);
    }

    public void DeleteClient(Guid clientId)
    {
        if(!ClientExistsInBankingSystem(clientId))
            throw new ArgumentException("Клиента не существует в банковской системе.", nameof(clientId));
        
        _clientStorage.Delete(clientId);
    }

    public void AddClientAccount(Guid clientId, string currencyCode = "USD", decimal amount = 0)
    {
        var client =
            _clientStorage.ClientWithAccountsList.Keys.FirstOrDefault(client => client.ClientId.Equals(clientId));
        
        if (!ClientExistsInBankingSystem(client: client))
            throw new ArgumentException("Клиента не существует в банковской системе.", nameof(clientId));
        
        var currency = _clientStorage.ListOfCurrencies.FirstOrDefault(currency => currency.Code == currencyCode);
        
        if (currency == null)
            throw new ArgumentException("переданной валюты не существует в банковской системе!",
                nameof(currencyCode));
        
        var account = TestDataGenerator.GenerateRandomBankClientAccount(currency, client, amount);

        _clientStorage.AddAccount(client, account);
    }

    public void UpdateAccount(Guid accountId, Account newAccount)
    {
        
    }

    public void DeleteAccount(Guid accountId)
    {
        var account = _clientStorage.ClientWithAccountsList.Values.SelectMany(list => list)
            .FirstOrDefault(account => account.AccountId.Equals(accountId));

        if (account == null)
            throw new ArgumentException($"Лицевого счета не существует в банковской системе!");
        
        _clientStorage.DeleteAccount(account);
    }
    
    /*public List<Account> GetClientAccounts(Client client)
    {
        if (!_clientStorage.Data.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));

        return _clientStorage.Data[client];
    }

    public void WithdrawClientAccounts(Client client)
    {
        if (!_clientStorage.Data.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));

        Console.WriteLine($"Клиент: {client.FirstName} {client.LastName}, лицевые счета:");

        var mess = string.Join('\n',
            _clientStorage.Data[client].Select(clientAccount =>
                $"Номер счета: {clientAccount.AccountNumber}, валюта: {clientAccount.Currency.Name}, " +
                $"баланс: {clientAccount.Amount} {clientAccount.Currency.Code}"));
        
        Console.WriteLine(mess);
    }*/

    public void WithdrawBankCurrencies()
    {
        /*Console.WriteLine("\nВалюты банка:\n" + string.Join('\n',
            _listOfCurrencies.Select(currency => $"{currency.Code} {currency.Name} {currency.ExchangeRate}")));*/
    }
    
    private void ValidateClient(Client client, bool isUpdating = false)
    {
        if (!isUpdating && ClientExistsInBankingSystem(client: client))
            throw new InvalidOperationException("Клиент уже существует в банковской системе!");
        
        if (string.IsNullOrWhiteSpace(client.FirstName))
            throw new ArgumentException("Не указано имя клиента!", nameof(client.FirstName));
        
        if (string.IsNullOrWhiteSpace(client.LastName))
            throw new ArgumentException("Не указана фамилия клиента!", nameof(client.LastName));
        
        if (string.IsNullOrWhiteSpace(client.PhoneNumber))
            throw new ArgumentException("Не указан номер клиента!", nameof(client.PhoneNumber));
        
        if (string.IsNullOrWhiteSpace(client.Email))
            throw new ArgumentException("Не указан e-mail клиента!", nameof(client.Email));
        
        if (string.IsNullOrWhiteSpace(client.Address))
            throw new ArgumentException("Не указан адрес клиента!", nameof(client.Email));

        if (client.DateOfBirth > DateTime.Now || client.DateOfBirth == DateTime.MinValue ||
            client.DateOfBirth == DateTime.MaxValue)
            throw new ArgumentException("Дата рождения клиента указана неверно!", nameof(client.DateOfBirth));

        var age = TestDataGenerator.CalculateAge(client.DateOfBirth);

        if (age < 18)
            throw new ArgumentException("Клиенту меньше 18 лет!", nameof(client.DateOfBirth));

        if (age != client.Age || client.Age <= 0)
        {
            client.Age = age;
            Console.WriteLine("Возраст клиента указан неверно и был скорректирован по дате его рождения!");
        }
    }
    
    private bool ClientExistsInBankingSystem(Guid? clientId = null, Client? client = null)
    {
        if (client != null)
            return _clientStorage.ClientWithAccountsList.ContainsKey(client);
            
        if (clientId.HasValue)
            return _clientStorage.ClientWithAccountsList.Keys.FirstOrDefault(bankClient => bankClient.ClientId.Equals(clientId)) != null;

        return false;
    }
    /*
    public List<Client> GetClientsByFilters(string firstNameFilter = "",
        string lastNameFilter = "", string phoneNumberFilter = "", DateTime? minDateOfBirth = null,
        DateTime? maxDateOfBirth = null)
    {
        IEnumerable<Client> filteredClients = _clientStorage;
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

        return filteredClients.ToList();
    }*/
}