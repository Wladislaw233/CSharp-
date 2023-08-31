using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;
using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;

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
                DeletingClientAccountTest(clientService, bankClient);
                UpdatingClientTest(clientService, bankClient);
                DeletingClientTest(clientService, bankClient);
                GettingClientsWithFilterTest(clientService, bankClient);
            }
        }
        catch (CustomException exception)
        {
            CustomException.ExceptionHandling("Программа остановлена по причине:", exception);
        }
    }

    private static void AddingClientAccountTest(ClientService clientService, Client bankClient)
    {
        Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:");

        var presentationBankClientAccounts =
            clientService.GetPresentationClientAccounts(bankClient.ClientId).ToList();

        Console.WriteLine(presentationBankClientAccounts);
        Console.WriteLine("Добавим счет EUR с балансом 1455,23:");

        clientService.AddClientAccount(bankClient.ClientId, "EUR", new decimal(1455.23));

        presentationBankClientAccounts =
            clientService.GetPresentationClientAccounts(bankClient.ClientId).ToList();

        Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:");
        Console.WriteLine(presentationBankClientAccounts);
    }

    private static void DeletingClientAccountTest(ClientService clientService, Client bankClient)
    {
        Console.WriteLine("Удалим счет EUR с балансом 1455,23:");

        var bankClientAccounts = clientService.GetClientAccounts(bankClient.ClientId);

        var account = bankClientAccounts.Last();

        clientService.DeleteClientAccount(account.AccountId);

        var presentationBankClientAccounts =
            clientService.GetPresentationClientAccounts(bankClient.ClientId).ToList();

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

    private static void GettingClientsWithFilterTest(ClientService clientService, Client bankClient)
    {
        Console.WriteLine("Выведем клиентов с именем Al:");
        var filteredClients = clientService.ClientsWithFilterAndPagination(1, 100, "Al");

        Console.WriteLine("Клиенты:");

        Console.WriteLine(string.Join("\n",
            filteredClients.Select(client =>
                $"Имя {client.FirstName}, фамилия {client.LastName}, дата рождения {client.DateOfBirth.ToString("D")}")));
    }
}