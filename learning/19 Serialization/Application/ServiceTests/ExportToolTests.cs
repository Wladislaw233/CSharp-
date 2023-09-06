using BankingSystemServices.ExportTool;
using BankingSystemServices.Models;
using BankingSystemServices.Services;

namespace ServiceTests;

public class ExportToolTests
{
    private static readonly string PathToDirectory = Path.Combine("D:", "Learning Serialization");

    public static void ExportJsonServiceTest()
    {
        ExportClientsInJsonTest();
        ExportEmployeesInJsonTest();
    }

    private static void ExportClientsInJsonTest()
    {
        var bankClients = TestDataGenerator.GenerateListWithBankClients(3);

        Console.WriteLine("Запишем клиентов в файл ClientsData.json:");

        var mess = string.Join("\n",
            bankClients.Select(client =>
                $"Id клиента - {client.ClientId}, {client.FirstName} {client.LastName}"));

        Console.WriteLine(mess);

        var fileName = "ClientsData.json";

        ExportService.WritePersonsDataToJsonFile(bankClients, PathToDirectory, fileName);

        bankClients = ExportService.ReadPersonsDataFromJsonFile<Client>(PathToDirectory, fileName);

        Console.WriteLine("Считанные клиенты из файла ClientsData.json:");

        mess = string.Join("\n",
            bankClients.Select(client =>
                $"Id клиента - {client.ClientId}, {client.FirstName} {client.LastName}"));

        Console.WriteLine(mess);
    }

    private static void ExportEmployeesInJsonTest()
    {
        var bankEmployees = TestDataGenerator.GenerateListWithBankEmployees(3);

        Console.WriteLine("Запишем сотрудников в файл EmployeesData.json:");

        var mess = string.Join("\n",
            bankEmployees.Select(employee =>
                $"Id сотрудника - {employee.EmployeeId}, {employee.FirstName} {employee.LastName}"));

        Console.WriteLine(mess);

        var fileName = "EmployeesData.json";

        ExportService.WritePersonsDataToJsonFile(bankEmployees, PathToDirectory, fileName);

        fileName = "EmployeesData.json";

        bankEmployees = ExportService.ReadPersonsDataFromJsonFile<Employee>(PathToDirectory, fileName);

        Console.WriteLine("Считанные сотрудники из файла EmployeesData.json:");
        mess = string.Join("\n",
            bankEmployees.Select(employee =>
                $"Id сотрудника - {employee.EmployeeId}, {employee.FirstName} {employee.LastName}"));

        Console.WriteLine(mess);
    }
}