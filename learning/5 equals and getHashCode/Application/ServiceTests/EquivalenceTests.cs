using BankingSystemServices;
using BankingSystemServices.Services;

namespace ServiceTests;

public class EquivalenceTests
{
    private static readonly Currency Currency = new()
    {
        Code = "USD",
        Name = "United state dollar",
        ExchangeRate = 1
    };
    public static void GetHashCodeNecessityPositiveTest()
    {
       
        Console.WriteLine("Dictionary");
        // dictionary.
        var clientsAccounts = TestDataGenerator.GenerateDictionaryWithBankClientsAccounts(Currency);
        
        var copiedClient = Client.CopyClient(clientsAccounts.FirstOrDefault().Key);

        if (copiedClient != null)
        {
            var accountsFound = clientsAccounts.TryGetValue(copiedClient, out var foundAccounts);
            if (accountsFound && foundAccounts != null)
            {
                Console.WriteLine("В результате переопределения метода GetHashCode класса Client появилась возможность " +
                                  "получать аккаунты разных объектов с одинаковыми свойствами:");

                PrintClientAccountsPerformance(copiedClient, foundAccounts);
            }
            
            if (clientsAccounts.ContainsKey(copiedClient)) 
                clientsAccounts[copiedClient].Add(TestDataGenerator.GenerateRandomBankClientAccount(Currency, copiedClient));
            
            accountsFound = clientsAccounts.TryGetValue(copiedClient, out foundAccounts);

            if (accountsFound && foundAccounts != null)
            {
                Console.WriteLine("Ситуация когда у клиента несколько аккаунтов:");
                
                PrintClientAccountsPerformance(copiedClient, foundAccounts);
            }
        }
        
        //list.
        Console.WriteLine("list");

        var bankEmployees = TestDataGenerator.GenerateListWithBankEmployees();
        var copiedEmployee = Employee.CopyEmployee(bankEmployees.LastOrDefault());
        if (copiedEmployee != null)
            Console.WriteLine(
                $"В списке есть скопированный сотрудник ({copiedEmployee.FirstName} {copiedEmployee.LastName}) - " +
                (bankEmployees.Contains(copiedEmployee) ? "да" : "нет"));
    }

    static void PrintClientAccountsPerformance(Client client, List<Account> clientAccounts)
    {
        Console.WriteLine($"Клиент: {client.FirstName} {client.LastName}, аккаунт(-ы):");
        
        var clientAccountPerformance = string.Join('\n',
            clientAccounts.Select(account =>
                $"Валюта: {Currency.Code}, обменный курс: {Currency.ExchangeRate}, баланс: {account.Amount}"));
        
        Console.WriteLine(clientAccountPerformance);
    }
}