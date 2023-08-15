using Models;
using Services;

namespace ServiceTests;

public class EquivalenceTests
{
    public static void GetHashCodeNecessityPositivTest()
    {
        // dictionary.
        var clientsAccounts = TestDataGenerator.GenerateDictionaryWithClientsAccounts();
        var copiedClient = Client.CopyClient(clientsAccounts.FirstOrDefault().Key);

        var accountsFound = clientsAccounts.TryGetValue(copiedClient, out var foundAccounts);

        Console.WriteLine(accountsFound
            ? "В результате переопределения метода GetHashCode класса Client появилась возможность " +
              $"получать аккаунты разных объектов с одинаковыми свойствами:\n Клиент: {copiedClient.FirstName} {copiedClient.LastName}, аккаунт(-ы):\n" +
              string.Join(',',
                  foundAccounts.Select(account =>
                      $"Курс: {account.Currency.Name} {account.Currency.Course} р., баланс: {account.Amount}"))
            : "Аккаунты клиента не найдены!");

        TestDataGenerator.AddClientAccount(ref clientsAccounts, copiedClient);

        accountsFound = clientsAccounts.TryGetValue(copiedClient, out foundAccounts);
        Console.WriteLine(accountsFound
            ? $"Ситуация когда у клиента несколько аккаунтов:\nКлиент: {copiedClient.FirstName} {copiedClient.LastName}, аккаунты:\n" +
              string.Join(',',
                  foundAccounts.Select(account =>
                      $"Курс: {account.Currency.Name} {account.Currency.Course} р., баланс: {account.Amount}"))
            : "Аккаунт клиента не найден!");

        //list.

        var bankEmployees = TestDataGenerator.GenerateListWithEmployees();
        var copiedEmployee = bankEmployees.FirstOrDefault();
        
    }
}