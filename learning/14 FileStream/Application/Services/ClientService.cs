using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Services;

namespace Services;

public class ClientService
{
    private readonly BankingSystemDbContext _bankingSystemDbContext;
    private readonly Currency? _defaultCurrency;

    public ClientService(BankingSystemDbContext bankingSystemDbContext)
    {
        _bankingSystemDbContext = bankingSystemDbContext;
        if (_bankingSystemDbContext.Database.CanConnect())
        {
            if (_bankingSystemDbContext.Currencies.FirstOrDefault() == null)
            {
                _bankingSystemDbContext.Currencies.AddRange(TestDataGenerator.GenerateListOfCurrencies());
                SaveChanges();
            }

            _defaultCurrency = _bankingSystemDbContext.Currencies.ToList().Find(currency => currency.Code == "USD");
            if (_defaultCurrency == null)
                throw new ValueNotFoundException("Failed to get default currency!");
        }
        else
        {
            throw new DatabaseNotConnectedException("Failed to establish a connection to the database!");
        }
    }

    public void AddClient(Client client)
    {
        ValidateClient(client);
        var defaultAccount = CreateAccount(client, _defaultCurrency);

        if (defaultAccount == null)
            throw new InvalidOperationException("Failed to create default account!");

        _bankingSystemDbContext.Clients.Add(client);
        _bankingSystemDbContext.Accounts.Add(defaultAccount);

        SaveChanges();
    }

    private void SaveChanges()
    {
        _bankingSystemDbContext.SaveChanges();
    }

    private static Account? CreateAccount(Client client, Currency? currency, decimal amount = 0)
    {
        return currency != null ? TestDataGenerator.GenerateRandomBankClientAccount(currency, client, amount) : null;
    }

    private void ValidateClient(Client client)
    {
        if (_bankingSystemDbContext.Clients.Contains(client))
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
}