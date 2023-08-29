using BankingSystemServices;
using BankingSystemServices.Services;
using ExportTool;
using Services;
using Services.Database;

namespace ServiceTests;

public class ExportToolTests
{
    public static void ExportCsvServiceTest()
    {
        // Название файла clients.csv, лежит в папке D:\Learning FileStream
        var recordableClients = TestDataGenerator.GenerateListWitchBankClients();
        
        var pathToDirectory = Path.Combine("D:", "Learning FileStream");
        var fileName = "clients.csv";
        var exportService = new ExportService(pathToDirectory, fileName);

        Console.WriteLine("Запишем следующих клиентов:" +
                          "\n" + string.Join("\n",
                              recordableClients.Select(client =>
                                  $"{client.FirstName} {client.LastName}, дата рождения - {client.DateOfBirth.ToString("D")}")));
        
        exportService.WriteClientsDataToScvFile(recordableClients);

        var readableClients = exportService.ReadClientsDataFromScvFile();
        
        Console.WriteLine("\nПрочитаем клиентов из файла и добавим их в базу:" +
                          "\n" + string.Join("\n",
                              readableClients.Select(client =>
                                  $"{client.FirstName} {client.LastName}, дата рождения - {client.DateOfBirth.ToString("D")}.")));
        
        using var bankingSystemDbContext = new BankingSystemDbContext();
        var clientService = new ClientService(bankingSystemDbContext);
        
        foreach (var client in readableClients)
        {
            client.DateOfBirth = client.DateOfBirth.ToUniversalTime();
            clientService.AddClient(client);
        }

        bankingSystemDbContext.Dispose();
    }
}