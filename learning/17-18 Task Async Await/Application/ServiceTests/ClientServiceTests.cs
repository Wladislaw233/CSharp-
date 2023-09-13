using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;

namespace ServiceTests;

public static class ClientServiceTests
{
    public static void ClientServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Clients");
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
        {
            Console.WriteLine("Test client not found!");
        }
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
        catch (InvalidOperationException e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e,
                "An error occurred while performing the operation.");
            Console.WriteLine(mess);
        }
        catch (ArgumentException e)
        {
            var mess = ExceptionHandlingService.ArgumentExceptionHandler(e,
                "An error occurred while adding the client to the database.");
            Console.WriteLine(mess);
        }
        catch (PropertyValidationException e)
        {
            var mess = ExceptionHandlingService.PropertyValidationExceptionHandler(e);
            Console.WriteLine(mess);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e);
            Console.WriteLine(mess);
        }

        return null;
    }

    private static async Task AddingClientAccountTest(ClientService clientService, Client bankClient)
    {
        Console.WriteLine($"Client's personal accounts {bankClient.FirstName} {bankClient.LastName}:");

        var representationBankClientAccounts =
            await clientService.GetPresentationClientAccounts(bankClient.ClientId);

        Console.WriteLine(representationBankClientAccounts);

        const string currencyCode = "EUR";
        var amount = new decimal(1455.23);

        Console.WriteLine($"Add account {currencyCode} with balance {amount}:");

        try
        {
            await clientService.AddClientAccount(bankClient.ClientId, currencyCode, amount);

            representationBankClientAccounts =
                await clientService.GetPresentationClientAccounts(bankClient.ClientId);

            Console.WriteLine($"Client's personal accounts {bankClient.FirstName} {bankClient.LastName}:");
            Console.WriteLine(representationBankClientAccounts);
        }
        catch (ArgumentException e)
        {
            var mess = ExceptionHandlingService.ArgumentExceptionHandler(e,
                "An error occurred while adding the client account to the database.");
            Console.WriteLine(mess);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e);
            Console.WriteLine(mess);
        }
    }

    private static async Task UpdatingClientAccountTest(ClientService clientService, Client bankClient)
    {
        try
        {
            var bankClientAccounts = await clientService.GetClientAccounts(bankClient.ClientId);

            var account = bankClientAccounts.Last();

            var amount = new decimal(30000);

            Console.WriteLine(
                $"Let's update the account balance {account.AccountNumber} for client {bankClient.FirstName} {bankClient.LastName} to {amount}:");

            await clientService.UpdateClientAccount(account.AccountId, amount: amount);

            var presentationBankClientAccounts =
                await clientService.GetPresentationClientAccounts(bankClient.ClientId);

            Console.WriteLine($"Client's personal accounts {bankClient.FirstName} {bankClient.LastName}:");
            Console.WriteLine(presentationBankClientAccounts);
        }
        catch (ArgumentException e)
        {
            var mess = ExceptionHandlingService.ArgumentExceptionHandler(e,
                "An error occurred while updating the client account in database.");
            Console.WriteLine(mess);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e);
            Console.WriteLine(mess);
        }
    }

    private static async Task DeletingClientAccountTest(ClientService clientService, Client bankClient)
    {
        var bankClientAccounts = await clientService.GetClientAccounts(bankClient.ClientId);

        var account = bankClientAccounts.Last();

        try
        {
            Console.WriteLine($"Let's delete the account {account.AccountNumber}:");

            await clientService.DeleteClientAccount(account.AccountId);

            Console.WriteLine($"Client's personal accounts {bankClient.FirstName} {bankClient.LastName}:");

            var presentationBankClientAccounts =
                await clientService.GetPresentationClientAccounts(bankClient.ClientId);

            Console.WriteLine(presentationBankClientAccounts);
        }
        catch (ArgumentException e)
        {
            var mess = ExceptionHandlingService.ArgumentExceptionHandler(e,
                "An error occurred while deleting the client account in database.");
            Console.WriteLine(mess);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e);
            Console.WriteLine(mess);
        }
    }

    private static async Task UpdatingClientTest(ClientService clientService, Client bankClient)
    {
        Console.WriteLine("Let's change the client's first and last name:");
        Console.WriteLine(
            $"Before the change {bankClient.FirstName} {bankClient.LastName}. id {bankClient.ClientId}");

        try
        {
            await clientService.UpdateClient(bankClient.ClientId, "Vlad", "Yurchenko");
            Console.WriteLine(
                $"After the change {bankClient.FirstName} {bankClient.LastName}. id {bankClient.ClientId}");
        }
        catch (ArgumentException e)
        {
            var mess = ExceptionHandlingService.ArgumentExceptionHandler(e,
                "An error occurred while updating the client in database.");
            Console.WriteLine(mess);
        }
        catch (PropertyValidationException e)
        {
            var mess = ExceptionHandlingService.PropertyValidationExceptionHandler(e);
            Console.WriteLine(mess);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e);
            Console.WriteLine(mess);
        }
    }

    private static async Task DeletingClientTest(ClientService clientService, Client bankClient)
    {
        Console.WriteLine($"Delete client by id - {bankClient.ClientId}");
        try
        {
            await clientService.DeleteClient(bankClient.ClientId);
        }
        catch (ArgumentException e)
        {
            var mess = ExceptionHandlingService.ArgumentExceptionHandler(e,
                "An error occurred while deleting the client in database.");
            Console.WriteLine(mess);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e);
            Console.WriteLine(mess);
        }
    }

    private static async Task GettingClientsWithFilterTest(ClientService clientService)
    {
        const string clientFirstName = "Al";

        Console.WriteLine($"We will display clients with the name {clientFirstName}:");
        try
        {
            var filteredClients = await clientService.ClientsWithFilterAndPagination(1, 100, clientFirstName);

            Console.WriteLine("Clients:");

            var mess = string.Join("\n",
                filteredClients.Select(client =>
                    $"First name {client.FirstName}, last name {client.LastName}, date of birth {client.DateOfBirth:D}"));

            Console.WriteLine(mess);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e);
            Console.WriteLine(mess);
        }
    }
}