﻿using Bogus;

namespace BankingSystemServices.Services;

public class TestDataGenerator
{
    private static readonly Faker<Client> FakerClients = new Faker<Client>()
        .RuleFor(client => client.FirstName, faker => faker.Person.FirstName)
        .RuleFor(client => client.LastName, faker => faker.Person.LastName)
        .RuleFor(client => client.DateOfBirth,
            faker => faker.Date.Between(new DateTime(1960, 1, 1), new DateTime(2002, 12, 30)))
        .RuleFor(client => client.Age, (_, client) => CalculateAge(client.DateOfBirth))
        .RuleFor(client => client.PhoneNumber, faker => faker.Person.Phone)
        .RuleFor(client => client.Email, faker => faker.Person.Email)
        .RuleFor(client => client.Address, faker => faker.Address.FullAddress())
        .RuleFor(client => client.Bonus, faker => faker.Finance.Amount());
    
    private static readonly Faker<Employee> FakerEmployee = new Faker<Employee>()
        .RuleFor(employee => employee.FirstName, faker => faker.Person.FirstName)
        .RuleFor(employee => employee.LastName, faker => faker.Person.LastName)
        .RuleFor(employee => employee.DateOfBirth,
            faker => faker.Date.Between(new DateTime(1960, 1, 1), new DateTime(2002, 12, 30)))
        .RuleFor(employee => employee.Age, (_, employee) => CalculateAge(employee.DateOfBirth))
        .RuleFor(employee => employee.PhoneNumber, faker => faker.Person.Phone)
        .RuleFor(employee => employee.Email, faker => faker.Person.Email)
        .RuleFor(employee => employee.Address, faker => faker.Address.FullAddress())
        .RuleFor(employee => employee.Bonus, faker => faker.Finance.Amount())
        .RuleFor(employee => employee.Salary, faker => faker.Finance.Amount())
        .RuleFor(employee => employee.IsOwner, faker => faker.Random.Bool())
        .RuleFor(employee => employee.Bonus, faker => faker.Finance.Amount())
        .RuleFor(employee => employee.Contract, (_, employee) => GenerateEmployeeContract(employee));

    private static readonly Faker FakerAccounts = new();

    public static string GenerateEmployeeContract(Employee employee)
    {
        return $"{employee.FirstName} {employee.LastName}, дата рождения: {employee.DateOfBirth.ToString("D")}";
    }
    public static int CalculateAge(DateTime dateOfBirth)
    {
        var subtractedMonth = dateOfBirth > DateTime.Now.AddYears(-(DateTime.Now.Year - dateOfBirth.Year)) ? 1 : 0;
        return DateTime.Now.Year - dateOfBirth.Year - subtractedMonth;
    }

    public static Client GenerateRandomInvalidClient(bool changeAge = false)
    {
        var invalidClient = FakerClients.Generate();
        if (changeAge)
            invalidClient.Age = 0;
        else
            invalidClient.FirstName = "";

        return invalidClient;
    }
    
    public static Employee GenerateRandomInvalidEmployee(bool changeAge = false)
    {
        var invalidEmployee = FakerEmployee.Generate();
        if (changeAge)
            invalidEmployee.Age = 0;
        else
            invalidEmployee.FirstName = "";

        return invalidEmployee;
    }
    
    public static Client GenerateRandomBankClient()
    {
        return FakerClients.Generate();
    }

    public static Employee GenerateRandomBankEmployee()
    {
        return FakerEmployee.Generate();
    }

    public static Account GenerateRandomBankClientAccount(Currency currency, Client client, decimal? amount = null)
    {
        return new Account
        {
            CurrencyId = currency.CurrencyId,
            Currency = currency,
            ClientId = client.ClientId,
            Client = client,
            Amount = amount != null ? (decimal)amount : FakerAccounts.Finance.Amount(),
            AccountNumber = FakerAccounts.Finance.Account(25)
        };
    }

    public static Dictionary<Client, List<Account>> GenerateDictionaryWithBankClientsAccounts(Currency currency,
        int numberOfClients = 10)
    {
        var clientsAccounts = new Dictionary<Client, List<Account>>();

        var clients = FakerClients.Generate(numberOfClients);
        foreach (var client in clients)
                clientsAccounts.TryAdd(client,
                    new List<Account> { GenerateRandomBankClientAccount(currency, client) });
        
        return clientsAccounts;
    }

    public static List<Client> GenerateListWitchBankClients(int numberOfClients = 10)
    {
        return FakerClients.Generate(numberOfClients);
    }

    public static Dictionary<string, Client> GenerateDictionaryWithBankClients(List<Client> listBankClients)
    {
        var dictionaryBankClients = new Dictionary<string, Client>();
        foreach (var bankClient in listBankClients)
            dictionaryBankClients[bankClient.PhoneNumber] = bankClient;
        dictionaryBankClients.Add("00000000", FakerClients.Generate());
        return dictionaryBankClients;
    }

    public static List<Employee> GenerateListWithBankEmployees(int numberOfEmployees = 10)
    {
        return FakerEmployee.Generate(numberOfEmployees);
    }

    public static List<Currency> GenerateListOfCurrencies()
    {
        return new List<Currency>()
        {
            new Currency(){Code = "USD", Name = "US Dollar", ExchangeRate = 1 },
            new Currency(){Code = "EUR", Name = "Euro", ExchangeRate = new decimal(0.97) },
            new Currency(){Code = "RUB", Name = "Russian ruble", ExchangeRate = new decimal(96.64) }
        };
    }
}