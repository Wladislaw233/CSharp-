using BankingSystemServices.Database;
using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;

namespace ServiceTests;

public class EmployeeServiceTests
{
    private readonly EmployeeService _employeeService;
    private readonly TestDataGenerator _testDataGenerator = new();

    private Employee? _employee;

    public EmployeeServiceTests(BankingSystemDbContext bankingSystemDbContext)
    {
        _employeeService = new EmployeeService(bankingSystemDbContext);
    }

    public async Task EmployeeServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nEmployees");
        Console.ResetColor();

        _employee = await AddEmployeesTest();

        if (_employee != null)
        {
            await UpdateEmployeeTest();
            await DeleteEmployeeTest();
            await GetEmployeesWithFilterTest();
        }
        else
        {
            Console.WriteLine("Test employee not found!");
        }
    }

    private async Task<Employee?> AddEmployeesTest()
    {
        var addedBankEmployees = _testDataGenerator.GenerateListWithBankEmployees(5);

        Console.WriteLine($"Added {addedBankEmployees.Count} employees:");

        PrintEmployeeRepresentation(addedBankEmployees);

        try
        {
            foreach (var employee in addedBankEmployees)
                await _employeeService.AddEmployeeAsync(employee);

            return addedBankEmployees.FirstOrDefault();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    private async Task UpdateEmployeeTest()
    {
        if (_employee == null)
            return;

        Console.WriteLine("Let's change the employee's:");
        Console.WriteLine(
            $"Before the change {_employee.FirstName} {_employee.LastName}. id {_employee.EmployeeId}");

        var newEmployee = _testDataGenerator.GenerateRandomBankEmployee();

        try
        {
            await _employeeService.UpdateEmployeeAsync(_employee.EmployeeId, newEmployee);
            Console.WriteLine(
                $"After the change {_employee.FirstName} {_employee.LastName}. id {_employee.EmployeeId}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task DeleteEmployeeTest()
    {
        if (_employee == null)
            return;

        Console.WriteLine($"Removing an employee with id - {_employee.EmployeeId}");
        try
        {
            await _employeeService.DeleteEmployeeAsync(_employee.EmployeeId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task GetEmployeesWithFilterTest()
    {
        Console.WriteLine("Let's display the first 5 owners of the bank:");

        try
        {
            var filteredEmployees = await _employeeService.EmployeesWithFilterAndPaginationAsync(1, 5, isOwner: true);

            Console.WriteLine("Bank owners:");

            PrintEmployeeRepresentation(filteredEmployees);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static void PrintEmployeeRepresentation(IEnumerable<Employee> employees)
    {
        var mess = string.Join("\n",
            employees.Select(employee =>
                $"Firstname {employee.FirstName}, lastname {employee.LastName}, Date of Birth {employee.DateOfBirth:D}," +
                $" salary: {employee.Salary} $, is owner: {employee.IsOwner}"));

        Console.WriteLine(mess);
    }
}