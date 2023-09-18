using BankingSystemServices.Database;
using BankingSystemServices.ExportTool;
using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;

namespace ServiceTests;

public class ExportToolTests
{
    private readonly string _pathToDirectory = Path.Combine("D:", "Learning FileStream");
    private const string FileName = "clients.csv";
    
    private readonly ExportService _exportService = new();
    
    public void ExportCsvServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"File name - {FileName}, file path - {_pathToDirectory}");
        Console.ResetColor();

        WriteClientsDataToScvFileTest();
        ReadClientsDataFromScvFileTest();
    }

    private void WriteClientsDataToScvFileTest()
    {
        var testDataGenerator = new TestDataGenerator();
        var recordableClients = testDataGenerator.GenerateListWithBankClients(5);

        Console.WriteLine("Let's sign up the next clients:");

        PrintClientRepresentation(recordableClients);

        try
        {
            _exportService.WriteClientsDataToScvFile(recordableClients, _pathToDirectory, FileName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void ReadClientsDataFromScvFileTest()
    {
        Console.WriteLine("\nLet's read clients from the file and add them to the database:");
        try
        {
            var readableClients = _exportService.ReadClientsDataFromScvFile(_pathToDirectory, FileName);

            PrintClientRepresentation(readableClients);

            using var bankingSystemDbContext = new BankingSystemDbContext();
            {
                var clientService = new ClientService(bankingSystemDbContext);

                foreach (var client in readableClients)
                    clientService.AddClient(client);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
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