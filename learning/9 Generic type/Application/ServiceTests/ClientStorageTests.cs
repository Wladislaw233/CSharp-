using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;
using Services.Storage;

namespace ServiceTests;

public class ClientStorageTests
{
    private static List<Client> _bankClients = new();
    private static readonly ClientStorage _clientStorage = new();
    private static readonly ClientService _clientService = new(_clientStorage);

    public static void ClientStorageTest()
    {
        _bankClients = TestDataGenerator.GenerateListWithBankClients(3);
        _bankClients.Add(TestDataGenerator.GenerateRandomInvalidClient(true));
        _bankClients.Add(TestDataGenerator.GenerateRandomInvalidClient());

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Запуск тестов хранилища клиентов и сотрудников.");
        Console.WriteLine("КЛИЕНТЫ");

        AddingClientsTest();
        AddingClientAccountTest();
        UpdatingClientAccountTest();
    }

    private static void AddingClientsTest()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Добавление клиентов:");
        Console.ResetColor();
        try
        {
            foreach (var client in _bankClients)
            {
                Console.WriteLine(
                    $"\nПопытка добавления клиента: Имя: {client.FirstName}, фамилия: {client.LastName}, возраст: {client.Age}");

                _clientStorage.Add(client);
                _clientService.WithdrawClientAccounts(client);
                
                Console.WriteLine("Успешно!");
            }
        }
        catch (CustomException exception)
        {
            CustomException.ExceptionHandling("Произошла ошибка при добавлении клиента: ", exception);
        }
    }

    private static void AddingClientAccountTest()
    {
        _clientStorage.WithdrawBankCurrencies();

        var client = _bankClients.FirstOrDefault();
        if (client != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nДобавление счета клиентам:");
            Console.ResetColor();
            var currencyCode = "EUR";
            Console.WriteLine($"Добавим счет {currencyCode} клиенту:");
            Console.WriteLine("До изменения:");

            try
            {
                _clientService.WithdrawClientAccounts(client);

                _clientStorage.AddAccount(client, currencyCode, new decimal(124.11));

                Console.WriteLine("\nПосле изменения:");
                _clientService.WithdrawClientAccounts(client);

                var nonExistCurrencyCode = "RUP";
                
                Console.WriteLine(
                    $"Добавим счет {nonExistCurrencyCode} клиенту {client.FirstName} {client.LastName} (такой валюты нет):");

                _clientStorage.AddAccount(client, nonExistCurrencyCode, new decimal(123.1));
            }
            catch (CustomException exception)
            {
                CustomException.ExceptionHandling("Произошла ошибка при добавлении счета клиенту: ", exception);
            }
        }
        else
        {
            Console.WriteLine("Клиент не найден!");
        }
    }

    private static void UpdatingClientAccountTest()
    {
        var client = _bankClients.FirstOrDefault();
        if (client != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nРедактирование счета клиента:");
            Console.ResetColor();

            try
            {
                var clientAccounts = _clientService.GetClientAccounts(client);
                var clientAccount = clientAccounts.LastOrDefault();
                
                if (clientAccount != null)
                {
                    
                    if (clientAccount.Currency is null)
                        return;
                    
                    var oldCurrencyCode = clientAccount.Currency.Code;
                    var newCurrencyCode = "RUB";
                    
                    Console.WriteLine($"Изменим счет {oldCurrencyCode} на счет {newCurrencyCode} клиенту:");
                    Console.WriteLine("До изменения:");

                    _clientService.WithdrawClientAccounts(client);

                    _clientStorage.UpdateAccount(client, clientAccount.AccountNumber, newCurrencyCode, new decimal(45677.23));

                    Console.WriteLine("\nПосле изменения:");

                    _clientService.WithdrawClientAccounts(client);

                    var invalidAccountNumber = "INVALID_NUMBER";
                    
                    Console.WriteLine($"Изменим клиенту счет ({invalidAccountNumber}), которого у него его нет, на счет {newCurrencyCode}:");
                    Console.WriteLine("До изменения:");

                    _clientService.WithdrawClientAccounts(client);

                    _clientStorage.UpdateAccount(client, invalidAccountNumber, newCurrencyCode);
                }
                else
                {
                    Console.WriteLine("Аккаунт клиента не найден!");
                }
            }
            catch (CustomException exception)
            {
                CustomException.ExceptionHandling("Произошла ошибка при редактировании счета клиента: ",
                    exception);
            }
        }
        else
        {
            Console.WriteLine("Клиент не найден!");
        }
    }
}