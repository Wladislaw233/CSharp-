using BankingSystemServices.Models;
using BankingSystemServices.Services;
using BankingSystemServices.ExportTool;

namespace ServiceTests;

public class ThreadAndTaskTests
{
    public static void ThreadTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Параллельный импорт и экспорт клиентов");
        Console.ResetColor();
        ParallelExportAndImportClientsTest();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Параттельное начисление на аккаунт клиента");
        Console.ResetColor();
        ParallelAccrualToAccountTest();
    }

    private static void ParallelExportAndImportClientsTest()
    {
        // клиенты для экспорта.
        var bankClientsForExport = TestDataGenerator.GenerateListWithBankClients(5);

        // клиенты для импорта.
        var bankClientsForImport = TestDataGenerator.GenerateListWithBankClients(5);

        var pathToDirectory = Path.Combine("D:", "Learning thread");
        var importFileName = "ClientsForImport.csv";
        
        try
        {
            ExportService.WriteClientsDataToScvFile(bankClientsForImport, pathToDirectory, importFileName);

            object locker = new();

            Parallel.Invoke(() =>
                {
                    lock (locker)
                    {
                        var importBankClients =
                            ExportService.ReadClientsDataFromScvFile(pathToDirectory, importFileName);
                        bankClientsForExport.AddRange(importBankClients);
                        Console.WriteLine("Импортированные клиенты");
                        PrintClients(importBankClients);
                    }
                },
                () =>
                {
                    lock (locker)
                    {
                        Console.WriteLine("Экспортированные клиенты");
                        PrintClients(bankClientsForExport);
                        var exportFileName = "ExportClients.csv";
                        ExportService.WriteClientsDataToScvFile(bankClientsForExport, pathToDirectory, exportFileName);
                    }
                });
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine($"Не найден csv файл для чтения! - {e}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Во время параллельного экспорта и импорта клиентов произошла ошибка: {e}");
        }

        Thread.Sleep(1000);
    }

    private static void ParallelAccrualToAccountTest()
    {
        var bankClient = TestDataGenerator.GenerateRandomBankClient();
        var currency = TestDataGenerator.GenerateRandomCurrency();
        var account = TestDataGenerator.GenerateRandomBankClientAccount(currency, bankClient, 0);

        Console.WriteLine($"Счет клиента {bankClient.FirstName} {bankClient.LastName} до изменения:");
        PrintClientAccount(account);

        Parallel.Invoke(() =>
            {
                for (var index = 0; index < 10; index++)
                    account.Amount += 100;
                Console.WriteLine("Аккаунт после обработки первым потоком:");
                PrintClientAccount(account);
            },
            () =>
            {
                for (var index = 0; index < 10; index++)
                    account.Amount += 100;
                Console.WriteLine("Аккаунт после обработки вторым потоком:");
                PrintClientAccount(account);
            });
        
        Thread.Sleep(1000);
    }

    private static void PrintClients(List<Client> clients)
    {
        var mess = string.Join("\n",
            clients.Select(client =>
                $"ID {client.ClientId},{client.FirstName} {client.LastName} {client.DateOfBirth.ToString("D")}"));
        
        Console.WriteLine(mess);
    }

    private static void PrintClientAccount(Account account)
    {
        Console.WriteLine(
            $"Номер {account.AccountNumber}, валюта {account.Currency.Name}, баланс {account.Amount} {account.Currency.Code}");
    }
}