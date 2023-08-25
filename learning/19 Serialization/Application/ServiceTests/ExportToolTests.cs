using ExportTool;
using Models;
using Services;
using Services.Database;

namespace ServiceTests;

public class ExportToolTests
{
    public static void ExportCsvServiceTest()
    {
        // Название файла clients.csv, лежит в папке D:\Learning FileStream
        using var bankingSystemDbContext = new BankingSystemDbContext();
        var clientService = new ClientService(bankingSystemDbContext);
        var recordableClients = Task.Run( async () => await clientService.ClientsWithFilterAndPagination(1, 5)).Result;
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
            clientService.AddClient(client).Wait();
    }

    public static void ExportJsonServiceTest()
    {
        var bankClients = TestDataGenerator.GenerateListWitchBankClients(3);
        Console.WriteLine("Запишем клиентов в файл ClientsData.json:" +
                          "\n" + string.Join("\n",
                              bankClients.Select(client =>
                                  $"Id клиента - {client.ClientId}, {client.FirstName} {client.LastName}")));

        var pathToDirectory = Path.Combine("D:", "Learning Serialization");
        var fileName = "ClientsData.json";
        var exportService = new ExportService(pathToDirectory, fileName);
        exportService.WritePersonsDataToJsonFile(bankClients, fileName);

        var bankEmployees = TestDataGenerator.GenerateListWithEmployees(3);
        Console.WriteLine("Запишем сотрудников в файл EmployeesData.json:" +
                          "\n" + string.Join("\n",
                              bankEmployees.Select(employee =>
                                  $"Id сотрудника - {employee.EmployeeId}, {employee.FirstName} {employee.LastName}")));
        fileName = "EmployeesData.json";
        exportService.WritePersonsDataToJsonFile(bankEmployees, fileName);

        fileName = "ClientsData.json";
        bankClients = exportService.ReadPersonsDataFromJsonFile<Client>(fileName);
        Console.WriteLine("Считанные клиенты из файла ClientsData.json:" +
                          "\n" + string.Join("\n",
                              bankClients.Select(client =>
                                  $"Id клиента - {client.ClientId}, {client.FirstName} {client.LastName}")));
        fileName = "EmployeesData.json";
        bankEmployees = exportService.ReadPersonsDataFromJsonFile<Employee>(fileName);
        Console.WriteLine("Считанные сотрудники из файла EmployeesData.json:" +
                          "\n" + string.Join("\n",
                              bankEmployees.Select(employee =>
                                  $"Id сотрудника - {employee.EmployeeId}, {employee.FirstName} {employee.LastName}")));
    }
}