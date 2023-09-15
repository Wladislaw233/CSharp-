﻿using BankingSystemServices.Database;
using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;

namespace ServiceTests;

public class ClientServiceTests
{
    private readonly ClientService _clientService;
    private readonly TestDataGenerator _testDataGenerator = new();
    private Client? _client;

    public ClientServiceTests(BankingSystemDbContext bankingSystemDbContext)
    {
        _clientService = new ClientService(bankingSystemDbContext);
    }

    public void ClientServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Clients");
        Console.ResetColor();

        _client = AddingClientTest();

        if (_client != null)
        {
            AddingClientAccountTest();
            UpdatingClientAccountTest();
            DeletingClientAccountTest();
            UpdatingClientTest();
            DeletingClientTest();
            GettingClientsWithFilterTest();
        }
        else
        {
            Console.WriteLine("Test client not found!");
        }
    }

    private Client? AddingClientTest()
    {
        var bankClients = _testDataGenerator.GenerateListWithBankClients(5);

        try
        {
            foreach (var client in bankClients)
                _clientService.AddClient(client);

            return bankClients.FirstOrDefault();
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e, "error when adding client.");
            Console.WriteLine(mess);
            return null;
        }
    }

    private void AddingClientAccountTest()
    {
        if (_client == null)
            return;

        Console.WriteLine($"Client's personal accounts {_client.FirstName} {_client.LastName}:");

        var representationBankClientAccounts =
            _clientService.GetPresentationClientAccounts(_client.ClientId);

        Console.WriteLine(representationBankClientAccounts);

        const string currencyCode = "EUR";
        var amount = new decimal(1455.23);

        Console.WriteLine($"Add account {currencyCode} with balance {amount}:");

        try
        {
            _clientService.AddClientAccount(_client.ClientId, currencyCode, amount);

            representationBankClientAccounts =
                _clientService.GetPresentationClientAccounts(_client.ClientId);

            Console.WriteLine($"Client's personal accounts {_client.FirstName} {_client.LastName}:");
            Console.WriteLine(representationBankClientAccounts);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e, "error when adding client account.");
            Console.WriteLine(mess);
        }
    }

    private void UpdatingClientAccountTest()
    {
        if (_client == null)
            return;

        try
        {
            var bankClientAccounts = _clientService.GetClientAccounts(_client.ClientId);

            var account = bankClientAccounts.Last();

            var amount = new decimal(30000);

            Console.WriteLine(
                $"Let's update the account balance {account.AccountNumber} for client {_client.FirstName} {_client.LastName} to {amount}:");

            _clientService.UpdateClientAccount(account.AccountId, amount: amount);

            var presentationBankClientAccounts =
                _clientService.GetPresentationClientAccounts(_client.ClientId);

            Console.WriteLine($"Client's personal accounts {_client.FirstName} {_client.LastName}:");
            Console.WriteLine(presentationBankClientAccounts);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e, "error when updating client account.");
            Console.WriteLine(mess);
        }
    }

    private void DeletingClientAccountTest()
    {
        if (_client == null)
            return;

        var bankClientAccounts = _clientService.GetClientAccounts(_client.ClientId);

        var account = bankClientAccounts.Last();

        try
        {
            Console.WriteLine($"Let's delete the account {account.AccountNumber}:");

            _clientService.DeleteClientAccount(account.AccountId);

            Console.WriteLine($"Client's personal accounts {_client.FirstName} {_client.LastName}:");

            var presentationBankClientAccounts =
                _clientService.GetPresentationClientAccounts(_client.ClientId);

            Console.WriteLine(presentationBankClientAccounts);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e, "error when deleting client account.");
            Console.WriteLine(mess);
        }
    }

    private void UpdatingClientTest()
    {
        if (_client == null)
            return;

        Console.WriteLine("Let's change the client's first and last name:");
        Console.WriteLine(
            $"Before the change {_client.FirstName} {_client.LastName}. id {_client.ClientId}");

        var newClient = _testDataGenerator.GenerateRandomBankClient();

        try
        {
            _clientService.UpdateClient(_client.ClientId, newClient);
            Console.WriteLine(
                $"After the change {_client.FirstName} {_client.LastName}. id {_client.ClientId}");
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e, "error when updating client.");
            Console.WriteLine(mess);
        }
    }

    private void DeletingClientTest()
    {
        if (_client == null)
            return;

        Console.WriteLine($"Delete client by id - {_client.ClientId}");
        try
        {
            _clientService.DeleteClient(_client.ClientId);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e, "error when deleting client.");
            Console.WriteLine(mess);
        }
    }

    private void GettingClientsWithFilterTest()
    {
        const string clientFirstName = "Al";

        Console.WriteLine($"We will display clients with the name {clientFirstName}:");
        try
        {
            var filteredClients = _clientService.ClientsWithFilterAndPagination(1, 100, clientFirstName);

            Console.WriteLine("Clients:");

            var mess = string.Join("\n",
                filteredClients.Select(client =>
                    $"First name {client.FirstName}, last name {client.LastName}, date of birth {client.DateOfBirth:D}"));

            Console.WriteLine(mess);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e, "error when getting clients with filter.");
            Console.WriteLine(mess);
        }
    }
}