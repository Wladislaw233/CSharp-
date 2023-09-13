using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.ExportTool;
using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;

namespace ServiceTests;

public static class ExportToolTests
{
    private static readonly string PathToDirectory = Path.Combine("D:", "Learning FileStream");
    private const string FileName = "clients.csv";

    public static void ExportCsvServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"File name - {FileName}, file path - {PathToDirectory}");
        Console.ResetColor();

        WriteClientsDataToScvFileTest();
        ReadClientsDataFromScvFileTest();
    }

    private static void WriteClientsDataToScvFileTest()
    {
        var recordableClients = TestDataGenerator.GenerateListWithBankClients(5);

        Console.WriteLine("Let's sign up the next clients:");

        PrintClientRepresentation(recordableClients);

        try
        {
            ExportService.WriteClientsDataToScvFile(recordableClients, PathToDirectory, FileName);
        }
        catch (Exception e)
        {
            var eMess = ExceptionHandlingService.GeneralExceptionHandler(e,
                "An error occurred while writing the file.");
            Console.WriteLine(eMess);
        }
    }

    private static void ReadClientsDataFromScvFileTest()
    {
        Console.WriteLine("\nLet's read clients from the file and add them to the database:");
        try
        {
            var readableClients = ExportService.ReadClientsDataFromScvFile(PathToDirectory, FileName);

            PrintClientRepresentation(readableClients);

            using var bankingSystemDbContext = new BankingSystemDbContext();
            {
                var clientService = new ClientService(bankingSystemDbContext);

                foreach (var client in readableClients)
                    clientService.AddClient(client);
            }
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
        catch (FileNotFoundException e)
        {
            var eMess = ExceptionHandlingService.GeneralExceptionHandler(e, $"File {FileName} not found.");
            Console.WriteLine(eMess);
        }
        catch (Exception e)
        {
            var eMess = ExceptionHandlingService.GeneralExceptionHandler(e,
                "An error occurred while reading the file.");
            Console.WriteLine(eMess);
        }
    }

    private static void PrintClientRepresentation(IEnumerable<Client> clients)
    {
        var mess = string.Join("\n",
            clients.Select(client =>
                $"{client.FirstName} {client.LastName}, Date of Birth - {client.DateOfBirth:D}"));

        Console.WriteLine(mess);
    }
}