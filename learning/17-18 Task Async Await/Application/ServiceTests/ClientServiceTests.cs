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
        var clientService = new ClientService(bankingSystemDbContext);
        
        var bankClient = AddingClientTest(clientService).Result;

        if (bankClient != null)
        {
            AddingClientAccountTest(clientService, bankClient).Wait();
            UpdatingClientAccountTest(clientService, bankClient).Wait();
            DeletingClientAccountTest(clientService, bankClient).Wait();
            UpdatingClientTest(clientService, bankClient).Wait();
            DeletingClientTest(clientService, bankClient).Wait();
            GettingClientsWithFilterTest(clientService).Wait();
        }
        else
            Console.WriteLine("Клиент для тестов не найден!");
    }

    private static async Task<Client?> AddingClientTest(ClientService clientService)
    {
        var bankClients = TestDataGenerator.GenerateListWithBankClients(5);

        try
        {
            foreach (var client in bankClients)
                await clientService.AddClient(client);

            return bankClients.FirstOrDefault();
        }
        catch (CustomException e)
        {
            CustomException.ExceptionHandling("Во время добавления клиента возникла ошибка: ", e);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Возникла ошибка: {e}");
        }

        return null;
    }
    
    private static async Task AddingClientAccountTest(ClientService clientService, Client bankClient)
    {
        Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:");

        var presentationBankClientAccounts =
            await clientService.GetPresentationClientAccounts(bankClient.ClientId);

        Console.WriteLine(presentationBankClientAccounts);

        var currencyCode = "EUR";
        var amount = new decimal(1455.23);
        
        Console.WriteLine($"Добавим счет {currencyCode} с балансом {amount}:");

        try
        {
            await clientService.AddClientAccount(bankClient.ClientId, currencyCode, amount);

            presentationBankClientAccounts =
                await clientService.GetPresentationClientAccounts(bankClient.ClientId);

            Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:");
            Console.WriteLine(presentationBankClientAccounts);
        }
        catch (CustomException e)
        {
            CustomException.ExceptionHandling("Во время добавления счета клиенту возникла ошибка: ", e);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Возникла ошибка: {e}");
        }
    }

    private static async Task UpdatingClientAccountTest(ClientService clientService, Client bankClient)
    {
        try
        {
            var bankClientAccounts = await clientService.GetClientAccounts(bankClient.ClientId);

            var account = bankClientAccounts.Last();

            var amount = new decimal(30000);
            
            Console.WriteLine($"Обновим баланс счета {account.AccountNumber} клиенту {bankClient.FirstName} {bankClient.LastName} на {amount}:");
            
            await clientService.UpdateClientAccount(account.AccountId, amount: amount);

            var presentationBankClientAccounts =
                await clientService.GetPresentationClientAccounts(bankClient.ClientId);

            Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:");
            Console.WriteLine(presentationBankClientAccounts);
        }
        catch (CustomException e)
        {
            CustomException.ExceptionHandling("Во время обновления счета клиента возникла ошибка: ", e);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Возникла ошибка: {e}");
        }
    }
    
    private static async Task DeletingClientAccountTest(ClientService clientService, Client bankClient)
    {
        var bankClientAccounts = await clientService.GetClientAccounts(bankClient.ClientId);

        var account = bankClientAccounts.Last();

        try
        {
            Console.WriteLine($"Удалим счет {account.AccountNumber}:");
            
            await clientService.DeleteClientAccount(account.AccountId);
            
            Console.WriteLine($"Лицевые счета клиента {bankClient.FirstName} {bankClient.LastName}:");
            
            var presentationBankClientAccounts =
                await clientService.GetPresentationClientAccounts(bankClient.ClientId);
            
            Console.WriteLine(presentationBankClientAccounts);
        }
        catch (CustomException e)
        {
            CustomException.ExceptionHandling("Во время удаления счета клиента возникла ошибка: ", e);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Возникла ошибка: {e}");
        }
    }

    private static async Task UpdatingClientTest(ClientService clientService, Client bankClient)
    {
        Console.WriteLine("Изменим клиенту имя и фамилию:");
        Console.WriteLine(
            $"До изменения {bankClient.FirstName} {bankClient.LastName}. id {bankClient.ClientId}");

        try
        {
            await clientService.UpdateClient(bankClient.ClientId, "Влад", "Юрченко");
            Console.WriteLine(
                $"После изменения {bankClient.FirstName} {bankClient.LastName}. id {bankClient.ClientId}");
        }
        catch (CustomException e)
        {
            CustomException.ExceptionHandling("Во время обновления клиента возникла ошибка: ", e);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Возникла ошибка: {e}");
        }
    }

    private static async Task DeletingClientTest(ClientService clientService, Client bankClient)
    {
        Console.WriteLine($"Удаление клиента с id - {bankClient.ClientId}");
        try
        {
            await clientService.DeleteClient(bankClient.ClientId);
        }
        catch (CustomException e)
        {
            CustomException.ExceptionHandling("Во время удаления клиента возникла ошибка: ", e);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Возникла ошибка: {e}");
        }
    }

    private static async Task GettingClientsWithFilterTest(ClientService clientService)
    {
        var clientFirstName = "Al";
        
        Console.WriteLine($"Выведем клиентов с именем {clientFirstName}:");
        try
        {
            var filteredClients = await clientService.ClientsWithFilterAndPagination(1, 100, "Al");

            Console.WriteLine("Клиенты:");

            var mess = string.Join("\n",
                filteredClients.Select(client =>
                    $"Имя {client.FirstName}, фамилия {client.LastName}, дата рождения {client.DateOfBirth.ToString("D")}"));

            Console.WriteLine(mess);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Возникла ошибка при получении клиентов по фильтрам: {e}");
        }
    }
}