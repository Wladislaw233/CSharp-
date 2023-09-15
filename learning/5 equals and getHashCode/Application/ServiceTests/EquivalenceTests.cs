using BankingSystemServices.Services;
using BankingSystemServices.Models;

namespace ServiceTests;

public class EquivalenceTests
{
    private static readonly TestDataGenerator _testDataGenerator = new();
    
    public static void GetHashCodeNecessityPositiveTest()
    {
        EquivalenceInDictionaryTest();
        EquivalenceInListTest();
    }

    private static void EquivalenceInDictionaryTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Словарь");
        Console.ResetColor();

        var currency = _testDataGenerator.GenerateRandomCurrency();
        var clientsAccounts = _testDataGenerator.GenerateDictionaryWithBankClientsAccounts(currency);

        var client = clientsAccounts.First().Key;
        Console.WriteLine("Копируемый клиент:");
        PrintClientRepresentation(client);
        var copiedClient = Client.CopyClient(client);
        if (copiedClient != null)
        {
            Console.WriteLine("Скопированный клиент:");
            PrintClientRepresentation(copiedClient);
            var accountsFound = clientsAccounts.TryGetValue(copiedClient, out var foundAccounts);
            if (accountsFound && foundAccounts != null)
            {
                Console.WriteLine(
                    "В результате переопределения метода GetHashCode класса Client появилась возможность " +
                    "получать аккаунты разных объектов с одинаковыми свойствами:");

                PrintClientAccountsRepresentation(copiedClient, foundAccounts, currency);
            }
            else
            {
                Console.WriteLine("Не удалось найти аккаунты скопированного клиента!");
            }

            var clientAccount = TestDataGenerator.GenerateBankClientAccount(currency, copiedClient);
            Console.WriteLine(
                $"Добавим аккаунт {clientAccount.AccountNumber}, {clientAccount.Amount} {clientAccount.Currency?.Code} скопированному клиенту:");

            if (clientsAccounts.ContainsKey(copiedClient))
                clientsAccounts[copiedClient].Add(clientAccount);

            accountsFound = clientsAccounts.TryGetValue(copiedClient, out foundAccounts);

            if (accountsFound && foundAccounts != null)
                PrintClientAccountsRepresentation(copiedClient, foundAccounts, currency);
            else
                Console.WriteLine("Не удалось найти аккаунты скопированного клиента!");
        }
        else
            Console.WriteLine("Клиент не скопирован!");
        
    }

    private static void EquivalenceInListTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Коллекция");
        Console.ResetColor();

        var bankEmployees = _testDataGenerator.GenerateListWithBankEmployees();
        var employee = bankEmployees.First();
        Console.WriteLine("Копируемый сотрудник:");
        PrintEmployeeRepresentation(employee);
        var copiedEmployee = Employee.CopyEmployee(employee);
        if (copiedEmployee != null)
        {
            Console.WriteLine("Скопированный сотрудник:");
            PrintEmployeeRepresentation(copiedEmployee);
            Console.WriteLine(
                $"В списке есть скопированный сотрудник ({copiedEmployee.FirstName} {copiedEmployee.LastName})?");
            Console.WriteLine(bankEmployees.Contains(copiedEmployee) ? "да" : "нет");
        }
        else
            Console.WriteLine("Сотрудник не скопирован!");
        
    }

    private static void PrintClientRepresentation(Client client)
    {
        Console.WriteLine(
            $"ID {client.ClientId}, {client.FirstName} {client.LastName}, {client.DateOfBirth.ToString("D")}");
    }

    private static void PrintEmployeeRepresentation(Employee employee)
    {
        Console.WriteLine(
            $"ID {employee.EmployeeId}, {employee.FirstName} {employee.LastName}, {employee.DateOfBirth.ToString("D")}");
    }

    private static void PrintClientAccountsRepresentation(Client client, List<Account> clientAccounts,
        Currency currency)
    {
        Console.WriteLine($"Клиент: {client.FirstName} {client.LastName}, счёт(-а):");

        var clientAccountPerformance = string.Join('\n',
            clientAccounts.Select(account =>
                $"Номер счета: {account.AccountNumber}, валюта: {currency.Code}, обменный курс: {currency.ExchangeRate}, баланс: {account.Amount}"));

        Console.WriteLine(clientAccountPerformance);
    }
}