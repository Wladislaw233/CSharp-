﻿using BankingSystemServices;
using BankingSystemServices.Services;
using Services;
using Services.Exceptions;
using Services.Storage;

namespace ServiceTests;

public class ClientStorageTests
{
    private static List<Client> _bankClients = new();
    private static readonly ClientStorage ClientStorage = new();

    public static void ClientStorageTest()
    {
        _bankClients = TestDataGenerator.GenerateListWitchBankClients(3);
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
        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Добавление клиентов:");
            Console.ResetColor();

            foreach (var client in _bankClients)
            {
                Console.WriteLine(
                    $"\nПопытка добавления клиента: Имя: {client.FirstName}, фамилия: {client.LastName}, возраст: {client.Age}");
                ClientStorage.Add(client);
                ClientService.WithdrawClientAccounts(ClientStorage, client);
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
        ClientStorage.WithdrawBankCurrencies();
        try
        {
            var client = _bankClients.FirstOrDefault();
            if (client != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nДобавление счета клиентам:");
                Console.ResetColor();
                Console.WriteLine("Добавим счет EUR клиенту:");
                Console.WriteLine("До изменения:");
                ClientService.WithdrawClientAccounts(ClientStorage, client);
                ClientStorage.AddAccount(client, "EUR", new decimal(124.11));

                Console.WriteLine("\nПосле изменения:");
                ClientService.WithdrawClientAccounts(ClientStorage, client);
                Console.WriteLine($"Добавим счет RUP клиенту {client.FirstName} {client.LastName} (такой валюты нет):");
                ClientStorage.AddAccount(client, "RUP", new decimal(123.1));
            }
        }
        catch (CustomException exception)
        {
            CustomException.ExceptionHandling("Произошла ошибка при добавлении счета клиенту: ", exception);
        }
    }

    private static void UpdatingClientAccountTest()
    {
        try
        {
            var client = _bankClients.FirstOrDefault();
            if (client != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nРедактирование счета клиента:");
                Console.ResetColor();

                var clientAccounts = ClientService.GetClientAccounts(ClientStorage, client);

                var clientAccount = clientAccounts.LastOrDefault();
                if (clientAccount != null)
                {
                    Console.WriteLine("Изменим счет EUR на счет RUB клиенту:");
                    Console.WriteLine("До изменения:");
                    ClientService.WithdrawClientAccounts(ClientStorage, client);
                    ClientStorage.UpdateAccount(client, clientAccount.AccountNumber, "RUB", new decimal(45677.23));
                    Console.WriteLine("\nПосле изменения:");
                    ClientService.WithdrawClientAccounts(ClientStorage, client);

                    Console.WriteLine("Изменим клиенту счет, которого у него его нет, на счет RUB:");
                    Console.WriteLine("До изменения:");
                    ClientService.WithdrawClientAccounts(ClientStorage, client);
                    ClientStorage.UpdateAccount(client, "INVALIDNUMBER", "RUB");
                }
            }
        }
        catch (CustomException exception)
        {
            CustomException.ExceptionHandling("Произошла ошибка при редактировании счета клиента: ", exception);
        }
    }
}