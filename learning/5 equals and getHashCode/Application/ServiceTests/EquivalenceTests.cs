using Models;
using Services;

namespace ServiceTests;

public class EquivalenceTests
{
    public static void GetHashCodeNecessityPositivTest()
    {
        var clientsAccounts = TestDataGenerator.GenerateDictionaryWithClientsAccounts();
        var copiedClient = Client.CopyClient(clientsAccounts.FirstOrDefault().Key);

        var accountsFound = clientsAccounts.TryGetValue(copiedClient, out var foundAccount);

        Console.WriteLine(accountsFound
            ? "В результате переопределения метода GetHashCode класса Client появилась возможность " +
              $"получать аккаунты разных объектов с одинаковыми свойствами:\n Клиент: {copiedClient.FirstName} {copiedClient.LastName}, аккаунт(-ы):\n" +
              Account.ConvertToStringAccountsList(foundAccount)
            : "Аккаунты клиента не найдены!");

        TestDataGenerator.AddClientAccount(ref clientsAccounts, copiedClient);

        accountsFound = clientsAccounts.TryGetValue(copiedClient, out foundAccount);
        Console.WriteLine(accountsFound
            ? $"Ситуация когда у клиента несколько аккаунтов:\nКлиент: {copiedClient.FirstName} {copiedClient.LastName}, аккаунты:\n" +
              Account.ConvertToStringAccountsList(foundAccount)
            : "Аккаунт клиента не найден!");
    }
}