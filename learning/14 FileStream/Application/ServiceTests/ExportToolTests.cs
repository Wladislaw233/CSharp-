using ExportTool;
using Services;
using Services.Database;

namespace ServiceTests;

public class ExportToolTests
{
    public static void ExportServiceTest()
    {
        // Название файла clients.csv, лежит в папке D:\Learning FileStream
        using var bankingSystemDbContext = new BankingSystemDbContext();
        var clientService = new ClientService(bankingSystemDbContext);
        var recordableClients = clientService.ClientsWithFilterAndPagination(1, 5);
        
        var pathToDirectory = Path.Combine("D:", "Learning FileStream");
        var fileName = "clients.csv";
        var exportService = new ExportService(pathToDirectory, fileName);

        Console.WriteLine("Запишем следующих клиентов:" +
                          "\n" + string.Join("\n",
                              recordableClients.Select(client =>
                                  $"{client.FirstName} {client.LastName}, дата рождения - {client.DateOfBirth.ToString("D")}")));
        
        exportService.WriteClientsDataToScvFile(recordableClients);

        var readableClients = exportService.ReadClientsDataFromScvFile();
        
        Console.WriteLine("Прочитаем клиентов из файла и добавим их в базу:" +
                          "\n" + string.Join("\n",
                              readableClients.Select(client =>
                                  $"{client.FirstName} {client.LastName}, дата рождения - {client.DateOfBirth.ToString("D")}.")));

        foreach (var client in readableClients)
            clientService.AddClient(client);
    }
}