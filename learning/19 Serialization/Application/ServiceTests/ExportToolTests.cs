using BankingSystemServices.ExportTool;
using BankingSystemServices.Models;
using Services;
using BankingSystemServices.Database;
using BankingSystemServices.Services;

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

        Console.WriteLine("Запишем следующих клиентов:" +
                          "\n" + string.Join("\n",
                              recordableClients.Select(client =>
                                  $"{client.FirstName} {client.LastName}, дата рождения - {client.DateOfBirth.ToString("D")}")));
        
        ExportService.WriteClientsDataToScvFile(recordableClients, pathToDirectory, fileName);

        var readableClients = ExportService.ReadClientsDataFromScvFile(pathToDirectory, fileName);
        
        Console.WriteLine("Прочитаем клиентов из файла и добавим их в базу:" +
                          "\n" + string.Join("\n",
                              readableClients.Select(client =>
                                  $"{client.FirstName} {client.LastName}, дата рождения - {client.DateOfBirth.ToString("D")}.")));

        foreach (var client in readableClients)
            clientService.AddClient(client).Wait();
    }

    public static void ExportJsonServiceTest()
    {
        var bankClients = TestDataGenerator.GenerateListWithBankClients(3);
        Console.WriteLine("Запишем клиентов в файл ClientsData.json:" +
                          "\n" + string.Join("\n",
                              bankClients.Select(client =>
                                  $"Id клиента - {client.ClientId}, {client.FirstName} {client.LastName}")));

        var pathToDirectory = Path.Combine("D:", "Learning Serialization");
        var fileName = "ClientsData.json";
        ExportService.WritePersonsDataToJsonFile(bankClients, pathToDirectory, fileName);

        var bankEmployees = TestDataGenerator.GenerateListWithBankEmployees(3);
        Console.WriteLine("Запишем сотрудников в файл EmployeesData.json:" +
                          "\n" + string.Join("\n",
                              bankEmployees.Select(employee =>
                                  $"Id сотрудника - {employee.EmployeeId}, {employee.FirstName} {employee.LastName}")));
        fileName = "EmployeesData.json";
        ExportService.WritePersonsDataToJsonFile(bankEmployees, pathToDirectory, fileName);

        fileName = "ClientsData.json";
        bankClients = ExportService.ReadPersonsDataFromJsonFile<Client>(pathToDirectory, fileName);
        Console.WriteLine("Считанные клиенты из файла ClientsData.json:" +
                          "\n" + string.Join("\n",
                              bankClients.Select(client =>
                                  $"Id клиента - {client.ClientId}, {client.FirstName} {client.LastName}")));
        fileName = "EmployeesData.json";
        bankEmployees = ExportService.ReadPersonsDataFromJsonFile<Employee>(pathToDirectory, fileName);
        Console.WriteLine("Считанные сотрудники из файла EmployeesData.json:" +
                          "\n" + string.Join("\n",
                              bankEmployees.Select(employee =>
                                  $"Id сотрудника - {employee.EmployeeId}, {employee.FirstName} {employee.LastName}")));
    }
}