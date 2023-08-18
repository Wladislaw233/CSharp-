using Models;
using Services.Exceptions;
using Services.Storage;

namespace Services;

public class ClientService
{
    public static string GenerateAccountNumber(Currency currency, int accountCounter)
    {
        return "ACC" + accountCounter.ToString("D10") + currency.Code;
    }

    public static void BeforeDeletingClient(Dictionary<Client, List<Account>> clientsAccounts, Client client)
    {
        if (!clientsAccounts.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));
    }

    public static Account BeforeDeletingAccount(Dictionary<Client, List<Account>> clientsAccounts, Client client,
        string accountNumber)
    {
        if (!clientsAccounts.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));
        var clientAccount = clientsAccounts[client].Find(account => account.AccountNumber == accountNumber);
        if (clientAccount == null)
            throw new CustomException($"У клиента нет счета с номером {accountNumber}!", nameof(accountNumber));
        return clientAccount;
    }

    public static Currency BeforeAddingClientAccount(Dictionary<Client, List<Account>> clientsAccounts, Client client,
        List<Currency> listOfCurrencies, string currencyCode = "USD")
    {
        if (!clientsAccounts.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));
        var currency = listOfCurrencies.Find(foundCurrency => foundCurrency.Code == currencyCode);
        if (currency.Code == null)
            throw new CustomException($"В банке нет переданной валюты ({currencyCode})!", nameof(currencyCode));
        return currency;
    }

    public static List<Account> GetClientAccounts(Dictionary<Client, List<Account>> clientsAccounts, Client client)
    {
        if (!clientsAccounts.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));
        return clientsAccounts[client];
    }

    public static void UpdateClientAccount(Dictionary<Client, List<Account>> clientsAccounts, Client client,
        string accountNumber, List<Currency> listOfCurrencies, string currencyCode = "", double amount = 0)
    {
        if (!clientsAccounts.ContainsKey(client))
            throw new CustomException("Клиента не существует в банковской системе!", nameof(client));
        var account = clientsAccounts[client].Find(foundAccount => foundAccount.AccountNumber == accountNumber);
        if (account == null)
            throw new CustomException($"У клиента нет счета с номером {accountNumber}!", nameof(accountNumber));
        if (!string.IsNullOrWhiteSpace(currencyCode))
        {
            var currency = listOfCurrencies.Find(foundCurrency => foundCurrency.Code == currencyCode);
            if (currency.Code == null)
                throw new CustomException($"В банке нет переданной валюты ({currencyCode})!", nameof(currencyCode));
            account.AccountNumber = account.AccountNumber.Replace(account.Currency.Code, currencyCode);
            account.Currency = currency;
        }

        account.Amount = amount;
    }

    public static List<Account> CreateDefaultAccount(int accountCounter, Currency currency)
    {
        return new List<Account>
            { new(GenerateAccountNumber(currency, accountCounter), currency) };
    }

    public static void WithdrawClientAccounts(Client client, IEnumerable<Account> clientAccounts)
    {
        Console.WriteLine($"Клиент: {client.FirstName} {client.LastName}, лицевые счета:" +
                          $"\n" + string.Join('\n',
                              clientAccounts.Select(clientAccount =>
                                  $"Номер счета: {clientAccount.AccountNumber} валюта: {clientAccount.Currency.Name} " +
                                  $"баланс: {clientAccount.Amount} {clientAccount.Currency.Code}")) +
                          "\n");
    }
    public static void ValidateClient(Dictionary<Client, List<Account>> clientsAccounts, Client client)
    {
        if (clientsAccounts.ContainsKey(client))
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

    public static IEnumerable<Client> GetClientsByFilters(ClientStorage clientStorage, string firstNameFilter = "",
        string lastNameFilter = "", string phoneNumberFilter = "", DateTime? minDateOfBirth = null,
        DateTime? maxDateOfBirth = null)
    {
        IEnumerable<Client> filteredClients = clientStorage.Data.Keys;
        if (!string.IsNullOrWhiteSpace(firstNameFilter))
            filteredClients = filteredClients.Where(client => client.FirstName.Contains(firstNameFilter));
        if (!string.IsNullOrWhiteSpace(lastNameFilter))
            filteredClients = filteredClients.Where(client => client.LastName.Contains(lastNameFilter));
        if (!string.IsNullOrWhiteSpace(phoneNumberFilter))
            filteredClients = filteredClients.Where(client => client.PhoneNumber.Contains(phoneNumberFilter));
        if (minDateOfBirth.HasValue)
            filteredClients = filteredClients.Where(client => client.DateOfBirth >= minDateOfBirth);
        if (maxDateOfBirth.HasValue)
            filteredClients = filteredClients.Where(client => client.DateOfBirth <= maxDateOfBirth);
        return filteredClients;
    }
}