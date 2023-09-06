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
        try
        {
            ExportService.WritePersonsDataToJsonFile(bankClients, PathToDirectory, fileName);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Во время записи файла возникла ошибка: {e}");
        }

        try
        {
            bankClients = ExportService.ReadPersonsDataFromJsonFile<Client>(PathToDirectory, fileName);

            Console.WriteLine("Считанные клиенты из файла ClientsData.json:");

            mess = string.Join("\n",
                bankClients.Select(client =>
                    $"Id клиента - {client.ClientId}, {client.FirstName} {client.LastName}"));

            Console.WriteLine(mess);
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

    private static void ExportEmployeesInJsonTest()
    {
        var bankEmployees = TestDataGenerator.GenerateListWithBankEmployees(3);

        Console.WriteLine("Запишем сотрудников в файл EmployeesData.json:");

        var mess = string.Join("\n",
            bankEmployees.Select(employee =>
                $"Id сотрудника - {employee.EmployeeId}, {employee.FirstName} {employee.LastName}"));

        Console.WriteLine(mess);

        var fileName = "EmployeesData.json";
        
        try
        {
            ExportService.WritePersonsDataToJsonFile(bankEmployees, PathToDirectory, fileName);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Во время записи файла возникла ошибка: {e}");
        }
        
        try
        {
            bankEmployees = ExportService.ReadPersonsDataFromJsonFile<Employee>(PathToDirectory, fileName);

            Console.WriteLine("Считанные сотрудники из файла EmployeesData.json:");

            mess = string.Join("\n",
                bankEmployees.Select(employee =>
                    $"Id сотрудника - {employee.EmployeeId}, {employee.FirstName} {employee.LastName}"));

            Console.WriteLine(mess);
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