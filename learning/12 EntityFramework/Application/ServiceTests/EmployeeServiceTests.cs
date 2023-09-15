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

    public void EmployeeServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nEmployees");
        Console.ResetColor();

        _employee = AddingEmployeesTest();

        if (_employee != null)
        {
            UpdatingEmployeeTest();
            DeletingEmployeeTest();
            GettingEmployeesWithFilterTest();
        }
        else
        {
            Console.WriteLine("Test employee not found!");
        }
    }

    private Employee? AddingEmployeesTest()
    {
        var addedBankEmployees = _testDataGenerator.GenerateListWithBankEmployees(100);

        Console.WriteLine("Added 5 employees:");

        PrintEmployeeRepresentation(addedBankEmployees);

        try
        {
            foreach (var employee in addedBankEmployees)
                _employeeService.AddEmployee(employee);

            return addedBankEmployees.FirstOrDefault();
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e, "error when adding employee.");
            Console.WriteLine(mess);
            return null;
        }
    }

    private void UpdatingEmployeeTest()
    {
        if (_employee == null)
            return;

        Console.WriteLine("Let's change the employee's first and last name:");
        Console.WriteLine(
            $"Before the change {_employee.FirstName} {_employee.LastName}. id {_employee.EmployeeId}");

        var newEmployee = _testDataGenerator.GenerateRandomBankEmployee();
        try
        {
            _employeeService.UpdateEmployee(_employee.EmployeeId, newEmployee);
            Console.WriteLine(
                $"After the change {_employee.FirstName} {_employee.LastName}. id {_employee.EmployeeId}");
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e, "error when updating employee.");
            Console.WriteLine(mess);
        }
    }

    private void DeletingEmployeeTest()
    {
        if (_employee == null)
            return;

        Console.WriteLine($"Removing an employee with id - {_employee.EmployeeId}");
        try
        {
            _employeeService.DeleteEmployee(_employee.EmployeeId);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e, "error when deleting employee.");
            Console.WriteLine(mess);
        }
    }

    private void GettingEmployeesWithFilterTest()
    {
        Console.WriteLine("Let's display the first 5 owners of the bank:");

        try
        {
            var filteredEmployees = _employeeService.EmployeesWithFilterAndPagination(1, 5, isOwner: true);

            Console.WriteLine("Bank owners:");

            PrintEmployeeRepresentation(filteredEmployees);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e, "error when getting employees with filter.");
            Console.WriteLine(mess);
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