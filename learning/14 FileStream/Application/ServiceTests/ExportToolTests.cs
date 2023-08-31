using BankingSystemServices.Services;
using ExportTool;
using Services;
using Services.Database;
using Services.Exceptions;

namespace ServiceTests;

public class ExportToolTests
{
    public static void ExportCsvServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Название файла clients.csv, лежит в папке D:/Learning FileStream");
        Console.ResetColor();
        
        var pathToDirectory = Path.Combine("D:", "Learning FileStream");
        var fileName = "clients.csv";
        var exportService = new ExportService(pathToDirectory, fileName);

        try
        {
            WriteClientsDataToScvFileTest(exportService);
            ReadClientsDataFromScvFileTest(exportService);
        }
        catch (CustomException e)
        {
            CustomException.ExceptionHandling("Во время работы с файлами произошла ошибка: ", e);
        }
        
    }

    private static void WriteClientsDataToScvFileTest(ExportService exportService)
    {
        var recordableClients = TestDataGenerator.GenerateListWithBankClients(5);
        Console.WriteLine("Запишем следующих клиентов:");
        Console.WriteLine(string.Join("\n",
            recordableClients.Select(client =>
                $"{client.FirstName} {client.LastName}, дата рождения - {client.DateOfBirth.ToString("D")}")));

        exportService.WriteClientsDataToScvFile(recordableClients);
    }

    private static void ReadClientsDataFromScvFileTest(ExportService exportService)
    {
        Console.WriteLine("\nПрочитаем клиентов из файла и добавим их в базу:");

        var readableClients = exportService.ReadClientsDataFromScvFile();

        Console.WriteLine(string.Join("\n",
            readableClients.Select(client =>
                $"{client.FirstName} {client.LastName}, дата рождения - {client.DateOfBirth.ToString("D")}.")));

        using var bankingSystemDbContext = new BankingSystemDbContext();
        var clientService = new ClientService(bankingSystemDbContext);
        foreach (var client in readableClients)
            clientService.AddClient(client);
        bankingSystemDbContext.Dispose();
    }
}