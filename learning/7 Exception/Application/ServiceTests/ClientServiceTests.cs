using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;

namespace ServiceTests;

public class ClientServiceTests
{
    private List<Client> _bankClients = new();
    private readonly ClientService _clientService = new();
    private readonly TestDataGenerator _testDataGenerator = new();
    public void ClientServiceTest()
    {
        AddClientTest();
        AddClientAccountTest();
        UpdateClientAccountTest();
    }

    private void AddClientTest()
    {
        _bankClients = _testDataGenerator.GenerateListWithBankClients(3);
        _bankClients.Add(_testDataGenerator.GenerateRandomInvalidClient(true));
        _bankClients.Add(_testDataGenerator.GenerateRandomInvalidClient());

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("КЛИЕНТЫ");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Добавление клиентов:");
        Console.ResetColor();

        try
        {
            foreach (var client in _bankClients)
            {
                Console.WriteLine(
                    $"\nПопытка добавления клиента: Имя: {client.FirstName}, фамилия: {client.LastName}, возраст: {client.Age}");
                _clientService.AddClient(client);
                _clientService.WithdrawClientAccounts(client);
                Console.WriteLine("Успешно!");
            }
        }
        catch (CustomException exception)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(exception, "Произошла ошибка при добавлении клиента.");
            Console.WriteLine(mess);
        }
    }

    private void AddClientAccountTest()
    {
        _clientService.WithdrawBankCurrencies();
        var client = _bankClients.FirstOrDefault();
        if (client != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nДобавление счета клиентам:");
            Console.ResetColor();
            Console.WriteLine("Добавим счет EUR клиенту:");
            Console.WriteLine("До изменения:");
            try
            {
                _clientService.WithdrawClientAccounts(client);
                _clientService.AddClientAccount(client, "EUR", new decimal(124.11));
                Console.WriteLine("\nПосле изменения:");
                _clientService.WithdrawClientAccounts(client);
                Console.WriteLine($"Добавим счет RUP клиенту {client.FirstName} {client.LastName} (такой валюты нет):");
                _clientService.AddClientAccount(client, "RUP", new decimal(123.1));
            }
            catch (CustomException exception)
            {
                var mess = ExceptionHandlingService.GeneralExceptionHandler(exception, "Произошла ошибка при добавлении счета клиенту.");
                Console.WriteLine(mess);
            }
        }
    }

    private void UpdateClientAccountTest()
    {
        var client = _bankClients.FirstOrDefault();
        if (client != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nРедактирование счета клиента:");
            Console.ResetColor();

            var clientAccounts = _clientService.GetClientAccounts(client);

            var clientAccount = clientAccounts.LastOrDefault();
            if (clientAccount != null)
            {
                Console.WriteLine("Изменим счет EUR на счет RUB клиенту:");
                Console.WriteLine("До изменения:");
                try
                {
                    _clientService.WithdrawClientAccounts(client);
                    _clientService.UpdateClientAccount(client, clientAccount.AccountNumber, "RUB",
                        new decimal(45677.23));
                    Console.WriteLine("\nПосле изменения:");
                    _clientService.WithdrawClientAccounts(client);
                    Console.WriteLine("Изменим клиенту счет, которого у него его нет, на счет RUB:");
                    Console.WriteLine("До изменения:");
                    clientAccounts = _clientService.GetClientAccounts(client);
                    _clientService.WithdrawClientAccounts(client);
                    _clientService.UpdateClientAccount(client, "INVALIDNUMBER", "RUB");
                }
                catch (CustomException exception)
                {
                    var mess = ExceptionHandlingService.GeneralExceptionHandler(exception, "Произошла ошибка при добавлении счета клиенту.");
                    Console.WriteLine(mess);
                }
            }
        }
    }
}