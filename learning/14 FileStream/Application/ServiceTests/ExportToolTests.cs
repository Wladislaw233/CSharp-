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
        Console.WriteLine($"Название файла {FileName}, лежит в папке {PathToDirectory}");
        Console.ResetColor();
        
        WriteClientsDataToScvFileTest();
        ReadClientsDataFromScvFileTest();
            
    }

    private static void WriteClientsDataToScvFileTest()
    {
        var recordableClients = TestDataGenerator.GenerateListWithBankClients(5);
        
        Console.WriteLine("Запишем следующих клиентов:");
        
        var mess = string.Join("\n",
            recordableClients.Select(client =>
                $"{client.FirstName} {client.LastName}, дата рождения - {client.DateOfBirth.ToString("D")}"));
        
        Console.WriteLine(mess);

        try
        {
            ExportService.WriteClientsDataToScvFile(recordableClients, PathToDirectory, FileName);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Во время записи файла возникла ошибка: {e}");
        }
    }

    private static void ReadClientsDataFromScvFileTest()
    {
        Console.WriteLine("\nПрочитаем клиентов из файла и добавим их в базу:");
        try
        {
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
        catch (CustomException e)
        {
            CustomException.ExceptionHandling("Во время добавления клиента возникла ошибка: ", e);
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine($"Json файл не был найден! - {e}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Во время чтения файла возникла ошибка: {e}");
        }
    }
}