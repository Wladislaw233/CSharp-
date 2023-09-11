using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;

namespace ServiceTests;

public class ClientServiceTests
{
    public static void ClientServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Клиенты");
        Console.ResetColor();
        using var bankingSystemDbContext = new BankingSystemDbContext();
        try
        {
            var clientService = new ClientService(bankingSystemDbContext);
            var bankClients = TestDataGenerator.GenerateListWithBankClients(5);

            foreach (var client in bankClients)
                clientService.AddClient(client);

            var bankClient = bankClients.FirstOrDefault();
            if (bankClient != null)
            {
                AddingClientAccountTest(clientService, bankClient);
                UpdatingClientAccountTest(clientService, bankClient);
                DeletingClientAccountTest(clientService, bankClient);
                UpdatingClientTest(clientService, bankClient);
                DeletingClientTest(clientService, bankClient);
                GettingClientsWithFilterTest(clientService);
            }
            else
            {
                Console.WriteLine("Клиент для тестов не найден!");
            }
        }
        catch (CustomException exception)
        {
            CustomException.ExceptionHandling("Программа остановлена по причине:", exception);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Во время работы программы возникла следующая ошибка: {e}");
        }

        bankingSystemDbContext.Dispose();
    }

    private static void AddingClientAccountTest(ClientService clientService, Client bankClient)
    {
        Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:");

        var presentationBankClientAccounts =
            clientService.GetPresentationClientAccounts(bankClient.ClientId);

        Console.WriteLine(presentationBankClientAccounts);

        var currencyCode = "EUR";
        var accountAmount = new decimal(1455.23);

        Console.WriteLine($"Добавим счет {currencyCode} с балансом {accountAmount}:");

        clientService.AddClientAccount(bankClient.ClientId, currencyCode, accountAmount);

        presentationBankClientAccounts =
            clientService.GetPresentationClientAccounts(bankClient.ClientId);

        Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:");
        Console.WriteLine(presentationBankClientAccounts);
    }

    private static void UpdatingClientAccountTest(ClientService clientService, Client bankClient)
    {
        var bankClientAccounts = clientService.GetClientAccounts(bankClient.ClientId);

        var account = bankClientAccounts.Last();

        var accountAmount = new decimal(30000);

        Console.WriteLine(
            $"Обновим баланс счета {account.Currency.Code} клиенту {bankClient.FirstName} {bankClient.LastName} на {accountAmount}:");

        clientService.UpdateClientAccount(account.AccountId, amount: accountAmount);

        var presentationBankClientAccounts =
            clientService.GetPresentationClientAccounts(bankClient.ClientId);

        Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:");
        Console.WriteLine(presentationBankClientAccounts);
    }

    private static void DeletingClientAccountTest(ClientService clientService, Client bankClient)
    {
        var bankClientAccounts = clientService.GetClientAccounts(bankClient.ClientId);

        var account = bankClientAccounts.Last();

        Console.WriteLine($"Удалим счет {account.Currency.Code}:");

        clientService.DeleteClientAccount(account.AccountId);

        var presentationBankClientAccounts =
            clientService.GetPresentationClientAccounts(bankClient.ClientId);

        Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:");
        Console.WriteLine(presentationBankClientAccounts);
    }

    private static void UpdatingClientTest(ClientService clientService, Client bankClient)
    {
        Console.WriteLine("Изменим клиенту имя и фамилию:");
        Console.WriteLine(
            $"До изменения {bankClient.FirstName} {bankClient.LastName}. id {bankClient.ClientId}");

        clientService.UpdateClient(bankClient.ClientId, "Влад", "Юрченко");
        Console.WriteLine(
            $"После изменения {bankClient.FirstName} {bankClient.LastName}. id {bankClient.ClientId}");
    }

    private static void DeletingClientTest(ClientService clientService, Client bankClient)
    {
        Console.WriteLine($"Удаление клиента с id - {bankClient.ClientId}");
        clientService.DeleteClient(bankClient.ClientId);
    }

    private static void GettingClientsWithFilterTest(ClientService clientService)
    {
        var clientFirstName = "Al";

        Console.WriteLine($"Выведем клиентов с именем {clientFirstName}:");
        var filteredClients = clientService.ClientsWithFilterAndPagination(1, 100, clientFirstName);

        Console.WriteLine("Клиенты:");

        var mess = string.Join("\n",
            filteredClients.Select(client =>
                $"Имя {client.FirstName}, фамилия {client.LastName}, дата рождения {client.DateOfBirth.ToString("D")}"));

        Console.WriteLine(mess);
    }
}