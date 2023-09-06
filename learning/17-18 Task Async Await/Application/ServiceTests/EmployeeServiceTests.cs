using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;
using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;

namespace ServiceTests;

public class EmployeeServiceTests
{
    public static void EmployeeServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nСотрудники");
        Console.ResetColor();

        using var bankingSystemDbContext = new BankingSystemDbContext();

        var employeeService = new EmployeeService(bankingSystemDbContext);

        var bankEmployee = AddingEmployeesTest(employeeService).Result;

        if (bankEmployee != null)
        {
            UpdatingEmployeeTest(employeeService, bankEmployee).Wait();
            DeletingEmployeeTest(employeeService, bankEmployee).Wait();
            GettingEmployeesWithFilterTest(employeeService).Wait();
        }
        else
        {
            Console.WriteLine("Сотрудник для тестов не найден!");
        }
    }

    private static async Task<Employee?> AddingEmployeesTest(EmployeeService employeeService)
    {
        var addedBankEmployees = TestDataGenerator.GenerateListWithBankEmployees(100);
        
        Console.WriteLine("Добавляемые 5 сотрудников:");
        
        var mess = string.Join("\n",
            addedBankEmployees.Select(employee =>
                $"Имя: {employee.FirstName}, фамилия: {employee.LastName}, зарплата: {employee.Salary} $, владелец: {employee.IsOwner} "));
        
        Console.WriteLine(mess);

        try
        {
            foreach (var employee in addedBankEmployees)
                await employeeService.AddEmployee(employee);
            
            return addedBankEmployees.FirstOrDefault();
        }
        catch (CustomException e)
        {
            CustomException.ExceptionHandling("Во время добавления сотрудников вознилка ошибка: ", e);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Возникла ошибка: {e}");
        }

        return null;
    }

    private static async Task UpdatingEmployeeTest(EmployeeService employeeService, Employee bankEmployee)
    {
        Console.WriteLine("Изменим сотруднику имя и фамилию:");
        Console.WriteLine(
            $"До изменения {bankEmployee.FirstName} {bankEmployee.LastName}. id {bankEmployee.EmployeeId}");

        try
        {
            await employeeService.UpdateEmployee(bankEmployee.EmployeeId, "Иван", "Иванов");
            Console.WriteLine(
                $"После изменения {bankEmployee.FirstName} {bankEmployee.LastName}. id {bankEmployee.EmployeeId}");
        }
        catch (CustomException e)
        {
            CustomException.ExceptionHandling("Во время обновления сотрудника вознилка ошибка: ", e);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Возникла ошибка: {e}");
        }
        
        
    }

    private static async Task DeletingEmployeeTest(EmployeeService employeeService, Employee bankEmployee)
    {
        Console.WriteLine($"Удаление сотрудника с id - {bankEmployee.EmployeeId}");
        try
        {
            await employeeService.DeleteEmployee(bankEmployee.EmployeeId);
        }
        catch (CustomException e)
        {
            CustomException.ExceptionHandling("Во время удаления сотрудника вознилка ошибка: ", e);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Возникла ошибка: {e}");
        }
    }

    private static async Task GettingEmployeesWithFilterTest(EmployeeService employeeService)
    {
        Console.WriteLine("Выведем первые 5 владельцев банка:");

        try
        {
            var filteredEmployees = await employeeService.EmployeesWithFilterAndPagination(1, 5, isOwner: true);

            Console.WriteLine("Владельцы:");

            var mess = string.Join("\n",
                filteredEmployees.Select(employee =>
                    $"Имя {employee.FirstName}, фамилия {employee.LastName}, дата рождения {employee.DateOfBirth.ToString("D")}," +
                    $" зарплата: {employee.Salary} $, владелец: {employee.IsOwner}"));

            Console.WriteLine(mess);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Возникла во время выбора сотрудников по фильтрам: {e}");
        }
    }
}