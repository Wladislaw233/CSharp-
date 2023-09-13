using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;
using Services.Storage;

namespace ServiceTests;

public static class EnumerableTests
{
    public static void GetClientsByFiltersTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Enumerable test");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Clients");
        Console.ResetColor();

        var clientStorage = new ClientStorage();
        var clientService = new ClientService(clientStorage);

        var bankClients = TestDataGenerator.GenerateListWithBankClients(10000);

        foreach (var client in bankClients)
            clientStorage.Add(client);

        const string clientFirstName = "Mack";
        var clientMinDateOfBirth = new DateTime(1970, 1, 1);
        var clientMaxDateOfBirth = new DateTime(2000, 1, 1);

        Console.WriteLine(
            $"Search for clients with name {clientFirstName} and date of birth greater than {clientMinDateOfBirth:D} and lesser {clientMaxDateOfBirth:D}:");

        var filteredClients =
            clientService.GetClientsByFilters(
                clientFirstName,
                minDateOfBirth: clientMinDateOfBirth,
                maxDateOfBirth: clientMaxDateOfBirth
            );

        foreach (var client in filteredClients)
            PrintClientRepresentation(client);

        var sortedByDateOfBirthClients = clientStorage.OrderByDescending(client => client.DateOfBirth).ToList();

        var youngestClient = sortedByDateOfBirthClients.First();
        PrintClientRepresentation(youngestClient);

        var oldestClient = sortedByDateOfBirthClients.Last();
        PrintClientRepresentation(oldestClient);

        var averageAge = clientStorage.Average(client => client.Age);
        Console.WriteLine($"\nAverage age of clients - {averageAge}");
    }

    public static void GetEmployeesByFiltersTest()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Employees");
        Console.ResetColor();

        var employeeStorage = new EmployeeStorage();
        var employeeService = new EmployeeService(employeeStorage);

        var bankEmployees = TestDataGenerator.GenerateListWithBankEmployees(10000);

        foreach (var employee in bankEmployees)
            employeeStorage.Add(employee);

        var employeeFirstName = "Mack";
        var employeeMinDateOfBirth = new DateTime(1970, 1, 1);
        var employeeMaxDateOfBirth = new DateTime(2000, 1, 1);

        Console.WriteLine(
            $"Search for employees with the name {employeeFirstName} and a date of birth higher than {employeeMinDateOfBirth:D} and lower than {employeeMaxDateOfBirth:D}:");

        var filteredEmployees =
            employeeService.GetEmployeesByFilters(
                employeeFirstName,
                minDateOfBirth: employeeMinDateOfBirth,
                maxDateOfBirth: employeeMaxDateOfBirth
            );

        foreach (var employee in filteredEmployees)
            PrintEmployeeRepresentation(employee);

        var sortedByDateOfBirthEmployees = employeeStorage.OrderByDescending(employee => employee.DateOfBirth).ToList();

        var youngestEmployee = sortedByDateOfBirthEmployees.First();
        PrintEmployeeRepresentation(youngestEmployee);

        var oldestEmployee = sortedByDateOfBirthEmployees.Last();
        PrintEmployeeRepresentation(oldestEmployee);

        var averageSalary = employeeStorage.Average(employee => employee.Salary);
        Console.WriteLine($"\nAverage employee salary - {averageSalary}");
    }

    private static void PrintClientRepresentation(Client client)
    {
        Console.WriteLine($"Client: {client.FirstName} {client.LastName}, " +
                          $"Date of Birth: {client.DateOfBirth:D}, " +
                          $"phone number: {client.PhoneNumber}, " +
                          $"email: {client.Email}");
    }

    private static void PrintEmployeeRepresentation(Employee employee)
    {
        Console.WriteLine($"Employee: {employee.FirstName} {employee.LastName}, " +
                          $"Date of Birth: {employee.DateOfBirth:D}, " +
                          $"Salary: {employee.Salary}, " +
                          $"Contract: {employee.Contract}");
    }
}