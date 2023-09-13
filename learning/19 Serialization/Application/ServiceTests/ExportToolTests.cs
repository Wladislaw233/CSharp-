using BankingSystemServices.ExportTool;
using BankingSystemServices.Models;
using BankingSystemServices.Services;

namespace ServiceTests;

public static class ExportToolTests
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

        const string fileName = "ClientsData.json";

        Console.WriteLine($"Let's write clients to the file {fileName}:");

        PrintClientsRepresentation(bankClients);

        try
        {
            ExportService.WritePersonsDataToJsonFile(bankClients, PathToDirectory, fileName);
        }
        catch (Exception e)
        {
            var eMess = ExceptionHandlingService.GeneralExceptionHandler(e,
                "An error occurred while writing the file.");
            Console.WriteLine(eMess);
        }

        try
        {
            bankClients = ExportService.ReadPersonsDataFromJsonFile<Client>(PathToDirectory, fileName);

            Console.WriteLine($"Read clients from file {fileName}:");

            PrintClientsRepresentation(bankClients);
        }
        catch (FileNotFoundException e)
        {
            var eMess = ExceptionHandlingService.GeneralExceptionHandler(e, "Json file was not found!");
            Console.WriteLine(eMess);
        }
        catch (Exception e)
        {
            var eMess = ExceptionHandlingService.GeneralExceptionHandler(e);
            Console.WriteLine(eMess);
        }
    }

    private static void ExportEmployeesInJsonTest()
    {
        var bankEmployees = TestDataGenerator.GenerateListWithBankEmployees(3);

        const string fileName = "EmployeesData.json";

        Console.WriteLine($"Let's record employees in a file {fileName}:");

        PrintEmployeesRepresentation(bankEmployees);

        try
        {
            ExportService.WritePersonsDataToJsonFile(bankEmployees, PathToDirectory, fileName);
        }
        catch (Exception e)
        {
            var eMess = ExceptionHandlingService.GeneralExceptionHandler(e);
            Console.WriteLine(eMess);
        }

        try
        {
            bankEmployees = ExportService.ReadPersonsDataFromJsonFile<Employee>(PathToDirectory, fileName);

            Console.WriteLine($"Read employees from file {fileName}:");

            PrintEmployeesRepresentation(bankEmployees);
        }
        catch (FileNotFoundException e)
        {
            var eMess = ExceptionHandlingService.GeneralExceptionHandler(e, "Json file was not found.");
            Console.WriteLine(eMess);
        }
        catch (Exception e)
        {
            var eMess = ExceptionHandlingService.GeneralExceptionHandler(e);
            Console.WriteLine(eMess);
        }
    }

    private static void PrintClientsRepresentation(IEnumerable<Client> clients)
    {
        var mess = string.Join("\n",
            clients.Select(client =>
                $"Client ID -{client.ClientId}, {client.FirstName} {client.LastName}"));

        Console.WriteLine(mess);
    }

    private static void PrintEmployeesRepresentation(IEnumerable<Employee> employees)
    {
        var mess = string.Join("\n",
            employees.Select(employee =>
                $"Employee ID - {employee.EmployeeId}, {employee.FirstName} {employee.LastName}"));

        Console.WriteLine(mess);
    }
}