using BankingSystemServices.Services;
using Services;
using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.ExportTool;

namespace ServiceTests;

public class ExportToolTests
{
    private static readonly string PathToDirectory = Path.Combine("D:", "Learning FileStream");
    private const string FileName = "clients.csv";
    public static void ExportCsvServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Название файла clients.csv, лежит в папке D:/Learning FileStream");
        Console.ResetColor();
        
        try
        {
            WriteClientsDataToScvFileTest();
            ReadClientsDataFromScvFileTest();
        }
        catch (CustomException e)
        {
            CustomException.ExceptionHandling("Во время работы с файлами произошла ошибка: ", e);
        }
        
    }

    private static void WriteClientsDataToScvFileTest()
    {
        var recordableClients = TestDataGenerator.GenerateListWithBankClients(5);
        Console.WriteLine("Запишем следующих клиентов:");
        var mess = string.Join("\n",
            recordableClients.Select(client =>
                $"{client.FirstName} {client.LastName}, дата рождения - {client.DateOfBirth.ToString("D")}"));
        
        Console.WriteLine(mess);

        ExportService.WriteClientsDataToScvFile(recordableClients, PathToDirectory, FileName);
    }

    private static void ReadClientsDataFromScvFileTest()
    {
        Console.WriteLine("\nПрочитаем клиентов из файла и добавим их в базу:");

        var readableClients = ExportService.ReadClientsDataFromScvFile(PathToDirectory, FileName);

        var mess = string.Join("\n",
            readableClients.Select(client =>
                $"{client.FirstName} {client.LastName}, дата рождения - {client.DateOfBirth.ToString("D")}"));
        
        Console.WriteLine(mess);

        using var bankingSystemDbContext = new BankingSystemDbContext();
        {
            var clientService = new ClientService(bankingSystemDbContext);

            foreach (var client in readableClients)
                clientService.AddClient(client);
        }
    }
}