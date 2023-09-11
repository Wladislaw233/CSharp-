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

        var fileName = "ClientsData.json";
        
        Console.WriteLine($"Запишем клиентов в файл {fileName}:");

        var mess = string.Join("\n",
            bankClients.Select(client =>
                $"Id клиента - {client.ClientId}, {client.FirstName} {client.LastName}"));

        Console.WriteLine(mess);
        
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

            Console.WriteLine($"Считанные клиенты из файла {fileName}:");

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

        var fileName = "EmployeesData.json";
        
        Console.WriteLine($"Запишем сотрудников в файл {fileName}:");

        var mess = string.Join("\n",
            bankEmployees.Select(employee =>
                $"Id сотрудника - {employee.EmployeeId}, {employee.FirstName} {employee.LastName}"));

        Console.WriteLine(mess);
        
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

            Console.WriteLine($"Считанные сотрудники из файла {fileName}:");

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