using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;
using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;

namespace ServiceTests;

public static class EmployeeServiceTests
{
    public static void EmployeeServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nEmployees");
        Console.ResetColor();

        using var bankingSystemDbContext = new BankingSystemDbContext();
 
        var employeeService = new EmployeeService(bankingSystemDbContext);

        var bankEmployee = AddingEmployeesTest(employeeService);

        if (bankEmployee != null)
        {
            UpdatingEmployeeTest(employeeService, bankEmployee);
            DeletingEmployeeTest(employeeService, bankEmployee);
            GettingEmployeesWithFilterTest(employeeService);
        }
        else
        {
            Console.WriteLine("Test employee not found!");
        }
    }

    private static Employee? AddingEmployeesTest(EmployeeService employeeService)
    {
        var addedBankEmployees = TestDataGenerator.GenerateListWithBankEmployees(100);
        
        Console.WriteLine("Added 5 employees:");
        
        PrintEmployeeRepresentation(addedBankEmployees);
        
        try
        {
            foreach (var employee in addedBankEmployees) 
                employeeService.AddEmployee(employee);
            
            return addedBankEmployees.FirstOrDefault();
        }
        catch (ArgumentException e)
        {
            var mess = ExceptionHandlingService.ArgumentExceptionHandler(e,
                "An error occurred while adding the employee to the database.");
            Console.WriteLine(mess);
        }
        catch (PropertyValidationException e)
        {
            var mess = ExceptionHandlingService.PropertyValidationExceptionHandler(e);
            Console.WriteLine(mess);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e);
            Console.WriteLine(mess);
        }

        return null;
    }

    private static void UpdatingEmployeeTest(EmployeeService employeeService, Employee bankEmployee)
    {
        Console.WriteLine("Let's change the employee's first and last name:");
        Console.WriteLine(
            $"Before the change {bankEmployee.FirstName} {bankEmployee.LastName}. id {bankEmployee.EmployeeId}");

        try
        {
            employeeService.UpdateEmployee(bankEmployee.EmployeeId, "Ivan", "Ivanov");
            Console.WriteLine(
                $"After the change {bankEmployee.FirstName} {bankEmployee.LastName}. id {bankEmployee.EmployeeId}");
        }
        catch (ArgumentException e)
        {
            var mess = ExceptionHandlingService.ArgumentExceptionHandler(e,
                "An error occurred while updating the employee in database.");
            Console.WriteLine(mess);
        }
        catch (PropertyValidationException e)
        {
            var mess = ExceptionHandlingService.PropertyValidationExceptionHandler(e);
            Console.WriteLine(mess);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e);
            Console.WriteLine(mess);
        }
        
        
    }

    private static void DeletingEmployeeTest(EmployeeService employeeService, Employee bankEmployee)
    {
        Console.WriteLine($"Removing an employee with id - {bankEmployee.EmployeeId}");
        try
        {
            employeeService.DeleteEmployee(bankEmployee.EmployeeId);
        }
        catch (ArgumentException e)
        {
            var mess = ExceptionHandlingService.ArgumentExceptionHandler(e,
                "An error occurred while deleting the client in database.");
            Console.WriteLine(mess);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e);
            Console.WriteLine(mess);
        }
    }

    private static void GettingEmployeesWithFilterTest(EmployeeService employeeService)
    {
        Console.WriteLine("Let's display the first 5 owners of the bank:");

        try
        {
            var filteredEmployees = employeeService.EmployeesWithFilterAndPagination(1, 5, isOwner: true);

            Console.WriteLine("Bank owners:");

            PrintEmployeeRepresentation(filteredEmployees);
        }
        catch (Exception e)
        {
            var mess = ExceptionHandlingService.GeneralExceptionHandler(e);
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