using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services.Storage;

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
        var client =
            _clientStorage.ClientWithAccountsList.Keys.SingleOrDefault(client => client.ClientId.Equals(clientId));

        if (client is null)
            throw new ArgumentException($"The client does with ID {clientId} not exist in the banking system.",
                nameof(clientId));

        ValidateClient(newClient, true);

        _clientStorage.Update(client, newClient);
    }

    public void DeleteClient(Guid clientId)
    {
        var client = GetClientById(clientId);

        _clientStorage.Delete(client);
    }

    public void AddClientAccount(Guid clientId, string currencyCode = "USD", decimal amount = 0)
    {
        var client = GetClientById(clientId);

        var currency = GetCurrencyByCurrencyCode(currencyCode);

        var account = TestDataGenerator.GenerateRandomBankClientAccount(currency, client, amount);

        _clientStorage.AddAccount(client, account);
    }

    public void UpdateAccount(Guid accountId, string? currencyCode = null, decimal? amount = null)
    {
        if (currencyCode is null && amount is null)
            throw new InvalidOperationException("update options are not specified.");

        var account = GetAccountById(accountId);

        var newAccount = new Account();

        if (!string.IsNullOrWhiteSpace(currencyCode))
        {
            var currency = GetCurrencyByCurrencyCode(currencyCode);

            newAccount.CurrencyId = currency.CurrencyId;
            newAccount.Currency = currency;
        }

        if (amount is null)
            newAccount.Amount = account.Amount;
        else
            newAccount.Amount = (decimal)amount;

        _clientStorage.UpdateAccount(account, newAccount);
    }

    public void DeleteAccount(Guid accountId)
    {
        var account = GetAccountById(accountId);

        _clientStorage.DeleteAccount(account);
    }

    public List<Account> GetClientAccounts(Client client)
    {
        if (!_clientStorage.ClientWithAccountsList.ContainsKey(client))
            throw new ArgumentException("The client does not exist in the banking system!", nameof(client));

        return _clientStorage.ClientWithAccountsList[client];
    }

    public void WithdrawClientAccounts(Client client)
    {
        if (!_clientStorage.ClientWithAccountsList.ContainsKey(client))
            throw new ArgumentException("The client does not exist in the banking system!", nameof(client));

        Console.WriteLine($"Client: {client.FirstName} {client.LastName}, accounts:");

        var mess = string.Join('\n',
            _clientStorage.ClientWithAccountsList[client].Select(clientAccount =>
                $"Account number: {clientAccount.AccountNumber}, currency: {clientAccount.Currency?.Name}, " +
                $"amount: {clientAccount.Amount} {clientAccount.Currency?.Code}"));

        Console.WriteLine(mess);
    }

    public void WithdrawBankCurrencies()
    {
        Console.WriteLine("\nBank currencies:\n" + string.Join('\n',
            _clientStorage.ListOfCurrencies.Select(currency =>
                $"{currency.Code} {currency.Name} {currency.ExchangeRate}")));
    }

    private Client GetClientById(Guid clientId)
    {
        var client =
            _clientStorage.ClientWithAccountsList.Keys.SingleOrDefault(client => client.ClientId.Equals(clientId));

        if (client is null)
            throw new ArgumentException($"The client does with ID {clientId} not exist in the banking system.",
                nameof(clientId));

        return client;
    }

    private Account GetAccountById(Guid accountId)
    {
        var account = _clientStorage.ClientWithAccountsList.Values.SelectMany(list => list)
            .FirstOrDefault(account => account.AccountId.Equals(accountId));

        if (account == null)
            throw new ArgumentException(
                $"The personal account with ID {accountId} does not exist in the banking system!");

        return account;
    }

    private Currency GetCurrencyByCurrencyCode(string currencyCode)
    {
        var currency = _clientStorage.ListOfCurrencies.SingleOrDefault(currency => currency.Code == currencyCode);

        if (currency is null)
            throw new ArgumentException(
                $"The transferred currency {currencyCode} does not exist in the banking system!",
                nameof(currencyCode));

        return currency;
    }

    private void ValidateClient(Client client, bool isUpdating = false)
    {
        if (!isUpdating && _clientStorage.ClientWithAccountsList.ContainsKey(client))
            throw new ArgumentException("This client has already been added to the banking system!", nameof(client));

        if (string.IsNullOrWhiteSpace(client.FirstName))
            throw new PropertyValidationException("Client first name not specified!", nameof(client.FirstName),
                nameof(Client));

        if (string.IsNullOrWhiteSpace(client.LastName))
            throw new PropertyValidationException("The client last name not specified!", nameof(client.LastName),
                nameof(Client));

        if (string.IsNullOrWhiteSpace(client.PhoneNumber))
            throw new PropertyValidationException("Client number not specified!", nameof(client.PhoneNumber),
                nameof(Client));

        if (string.IsNullOrWhiteSpace(client.Email))
            throw new PropertyValidationException("Client e-mail not specified!", nameof(client.Email), nameof(Client));

        if (string.IsNullOrWhiteSpace(client.Address))
            throw new PropertyValidationException("Client address not specified!", nameof(client.Address),
                nameof(Client));

        if (client.DateOfBirth > DateTime.Now || client.DateOfBirth.Equals(DateTime.MinValue) ||
            client.DateOfBirth.Equals(DateTime.MaxValue))
            throw new PropertyValidationException("The client date of birth is incorrect!", nameof(client.DateOfBirth),
                nameof(Client));

        var age = TestDataGenerator.CalculateAge(client.DateOfBirth);

        if (age < 18)
            throw new PropertyValidationException("The client is under 18 years old!", nameof(client.Age),
                nameof(Client));

        if (age != client.Age || client.Age <= 0) client.Age = TestDataGenerator.CalculateAge(client.DateOfBirth);
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