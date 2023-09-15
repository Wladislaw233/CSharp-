using BankingSystemServices.ExportTool;
using BankingSystemServices.Models;
using BankingSystemServices.Services;

namespace ServiceTests;

public class ExportToolTests
{
    private readonly string _pathToDirectory = Path.Combine("D:", "Learning Serialization");

    private readonly TestDataGenerator _testDataGenerator = new();

    public void ExportJsonServiceTest()
    {
        ExportClientsInJsonTest();
        ExportEmployeesInJsonTest();
    }

    private void ExportClientsInJsonTest()
    {
        var bankClients = _testDataGenerator.GenerateListWithBankClients(3);

        const string fileName = "ClientsData.json";

        Console.WriteLine($"Let's write clients to the file {fileName}:");

        PrintClientsRepresentation(bankClients);

        try
        {
            ExportService.WritePersonsDataToJsonFile(bankClients, _pathToDirectory, fileName);
        }
        catch (Exception e)
        {
            var eMess = ExceptionHandlingService.GeneralExceptionHandler(e,
                "An error occurred while writing the file.");
            Console.WriteLine(eMess);
        }

        try
        {
            bankClients = ExportService.ReadPersonsDataFromJsonFile<Client>(_pathToDirectory, fileName);

            Console.WriteLine($"Read clients from file {fileName}:");

            PrintClientsRepresentation(bankClients);
        }
        catch (Exception e)
        {
            var eMess = ExceptionHandlingService.GeneralExceptionHandler(e,
                "An error occurred while reading the file.");
            Console.WriteLine(eMess);
        }
    }

    private void ExportEmployeesInJsonTest()
    {
        var bankEmployees = _testDataGenerator.GenerateListWithBankEmployees(3);

        const string fileName = "EmployeesData.json";

        Console.WriteLine($"Let's record employees in a file {fileName}:");

        PrintEmployeesRepresentation(bankEmployees);

        try
        {
            ExportService.WritePersonsDataToJsonFile(bankEmployees, _pathToDirectory, fileName);
        }
        catch (Exception e)
        {
            var eMess = ExceptionHandlingService.GeneralExceptionHandler(e,
                "An error occurred while writing the file.");
            Console.WriteLine(eMess);
        }

        try
        {
            bankEmployees = ExportService.ReadPersonsDataFromJsonFile<Employee>(_pathToDirectory, fileName);

            Console.WriteLine($"Read employees from file {fileName}:");

            PrintEmployeesRepresentation(bankEmployees);
        }
        catch (Exception e)
        {
            var eMess = ExceptionHandlingService.GeneralExceptionHandler(e,
                "An error occurred while reading the file.");
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