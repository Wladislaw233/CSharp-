using BankingSystemServices.Models;
using BankingSystemServices.Services;
using BankingSystemServices.Exceptions;
using Services.Storage;

namespace Services;

public class ClientService
{

    private readonly ClientStorage _clientStorage;

    public ClientService(ClientStorage clientStorage)
    {
        _clientStorage = clientStorage;
    }
    
    public List<Account> GetClientAccounts(Client client)
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
    }

    public static void ValidateClient(Client client)
    {
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
    }
}