using BankingSystemServices.ExportTool;
using BankingSystemServices.Models;
using BankingSystemServices.Services;

namespace ServiceTests;

public class ThreadAndTaskTests
{
    private readonly TestDataGenerator _testDataGenerator = new();
    
    public void ThreadTest()
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

    private void ParallelExportAndImportClientsTest()
    {
        // clients for export.
        var bankClientsForExport = _testDataGenerator.GenerateListWithBankClients(5);

        // clients for import.
        var bankClientsForImport = _testDataGenerator.GenerateListWithBankClients(5);

        var pathToDirectory = Path.Combine("D:", "Learning thread");
        
        const string importFileName = "ClientsForImport.csv";
        const string exportFileName = "ExportClients.csv";

        var exportService = new ExportService();
        
        try
        {
            exportService.WriteClientsDataToScvFile(bankClientsForImport, pathToDirectory, importFileName);

            object locker = new();

            Parallel.Invoke(() =>
                {
                    lock (locker)
                    {
                        var importBankClients =
                            exportService.ReadClientsDataFromScvFile(pathToDirectory, importFileName);
                        
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
                        
                        exportService.WriteClientsDataToScvFile(bankClientsForExport, pathToDirectory, exportFileName);
                    }
                });
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e, "Error during parallel import and export of clients.");
            Console.WriteLine(mess);
        }

        Thread.Sleep(1000);
    }

    private void ParallelAccrualToAccountTest()
    {
        var bankClient = _testDataGenerator.GenerateRandomBankClient();
        var currency = _testDataGenerator.GenerateRandomCurrency();
        var account = TestDataGenerator.GenerateBankClientAccount(currency, bankClient, 0);

        Console.WriteLine($"Client account {bankClient.FirstName} {bankClient.LastName} before updating:");
        
        PrintClientAccount(account);
        
        try
        {
            Parallel.Invoke(
                () => AccountAccrual(account, 1),
                () => AccountAccrual(account, 2)
            );
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e,"Error during parallel accrual to accounts.");
            Console.WriteLine(mess);
        }

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