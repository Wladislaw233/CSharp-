using Models;
using Services;

namespace ServiceTests;

public class EquivalenceTests
{
    public static void GetHashCodeNecessityPositiveTest()
    {
        // dictionary.
        var clientsAccounts = TestDataGenerator.GenerateDictionaryWithClientsAccounts();
        var copiedClient = Client.CopyClient(clientsAccounts.FirstOrDefault().Key);

        var accountsFound = clientsAccounts.TryGetValue(copiedClient, out var foundAccounts);
        
        Console.WriteLine(accountsFound
            ? "В результате переопределения метода GetHashCode класса Client появилась возможность " +
              $"получать аккаунты разных объектов с одинаковыми свойствами:\n Клиент: {copiedClient.FirstName} {copiedClient.LastName}, аккаунт(-ы):\n" +
              string.Join('\n',
                  foundAccounts.Select(account =>
                      $"Валюта: {account.Currency.Code}, обменный курс: {account.Currency.ExchangeRate}, баланс: {account.Amount}"))
            : "Аккаунты клиента не найдены!");

        TestDataGenerator.AddClientAccount(ref clientsAccounts, copiedClient);

        accountsFound = clientsAccounts.TryGetValue(copiedClient, out foundAccounts);
        Console.WriteLine(accountsFound
            ? $"Ситуация когда у клиента несколько аккаунтов:\nКлиент: {copiedClient.FirstName} {copiedClient.LastName}, аккаунты:\n" +
              string.Join('\n',
                  foundAccounts.Select(account =>
                      $"Валюта: {account.Currency.Code}, обменный курс: {account.Currency.ExchangeRate}, баланс: {account.Amount}"))
            : "Аккаунт клиента не найден!");

        //list.
        Console.WriteLine();

        var bankEmployees = TestDataGenerator.GenerateListWithEmployees();
        var copiedEmployee = Employee.CopyEmployee(bankEmployees.LastOrDefault());
        Console.WriteLine(
            $"В списке есть скопированный сотрудник ({copiedEmployee.FirstName} {copiedEmployee.LastName}) - " +
            (bankEmployees.Contains(copiedEmployee) ? "да" : "нет"));
    }
}