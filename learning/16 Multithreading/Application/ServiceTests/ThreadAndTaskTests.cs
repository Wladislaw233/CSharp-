using BankingSystemServices.ExportTool;
using BankingSystemServices.Models;
using BankingSystemServices.Services;

namespace ServiceTests;

public static class ThreadAndTaskTests
{
    public static void ThreadTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Parallel import and export of clients");
        Console.ResetColor();
        ParallelExportAndImportClientsTest();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Parallel accrual to the client account");
        Console.ResetColor();
        ParallelAccrualToAccountTest();
    }

    private static void ParallelExportAndImportClientsTest()
    {
        // clients for export.
        var bankClientsForExport = TestDataGenerator.GenerateListWithBankClients(5);

        // clients for import.
        var bankClientsForImport = TestDataGenerator.GenerateListWithBankClients(5);

        var pathToDirectory = Path.Combine("D:", "Learning thread");
        const string importFileName = "ClientsForImport.csv";

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
                        Console.WriteLine("Imported clients");
                        PrintClients(importBankClients);
                    }
                },
                () =>
                {
                    lock (locker)
                    {
                        Console.WriteLine("Exported clients");
                        PrintClients(bankClientsForExport);
                        const string exportFileName = "ExportClients.csv";
                        ExportService.WriteClientsDataToScvFile(bankClientsForExport, pathToDirectory, exportFileName);
                    }
                });
        }
        catch (FileNotFoundException e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e, "CSV file not found to read.");
            Console.WriteLine(mess);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e);
            Console.WriteLine(mess);
        }

        Thread.Sleep(1000);
    }

    private static void ParallelAccrualToAccountTest()
    {
        var bankClient = TestDataGenerator.GenerateRandomBankClient();
        var currency = TestDataGenerator.GenerateRandomCurrency();
        var account = TestDataGenerator.GenerateRandomBankClientAccount(currency, bankClient, 0);

        Console.WriteLine($"Client account {bankClient.FirstName} {bankClient.LastName} before updating:");
        PrintClientAccount(account);

        var accountAccrual = new Action<Account, int>(AccountAccrual);

        Parallel.Invoke(
            () => accountAccrual(account, 1),
            () => accountAccrual(account, 2)
        );

        Thread.Sleep(1000);
    }

    private static void AccountAccrual(Account account, int threadIndex)
    {
        for (var index = 0; index < 10; index++)
            account.Amount += 100;
        Console.WriteLine($"Account after processing by stream - {threadIndex}:");
        PrintClientAccount(account);
    }

    private static void PrintClients(IEnumerable<Client> clients)
    {
        var mess = string.Join("\n",
            clients.Select(client =>
                $"ID {client.ClientId},{client.FirstName} {client.LastName} {client.DateOfBirth:D}"));

        Console.WriteLine(mess);
    }

    private static void PrintClientAccount(Account account)
    {
        Console.WriteLine(
            $"Account number {account.AccountNumber}, currency {account.Currency?.Name}, amount {account.Amount} {account.Currency?.Code}");
    }
}