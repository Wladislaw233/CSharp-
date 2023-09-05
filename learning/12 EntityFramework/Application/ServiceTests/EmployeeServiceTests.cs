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

        try
        {
            var employeeService = new EmployeeService(bankingSystemDbContext);

            var addedBankEmployees = TestDataGenerator.GenerateListWithBankEmployees(100);
        
            foreach (var employee in addedBankEmployees)
                employeeService.AddEmployee(employee);

            var bankEmployees = employeeService.EmployeesWithFilterAndPagination(1, 5);

            Console.WriteLine("Добавленные 5 сотрудников:");
            Console.WriteLine(string.Join("\n",
                bankEmployees.Select(employee =>
                    $"Имя: {employee.FirstName}, фамилия: {employee.LastName}, зарплата: {employee.Salary} $, владелец: {employee.IsOwner} ")));

            var bankEmployee = bankEmployees.FirstOrDefault();
            if (bankEmployee != null)
            {
                UpdatingEmployeeTest(employeeService, bankEmployee);
                DeletingEmployeeTest(employeeService, bankEmployee);
                GettingEmployeesWithFilterTest(employeeService);
            }
            else
                Console.WriteLine("Сотрудник для тестов не найден!");
        }
        catch (CustomException exception)
        {
            CustomException.ExceptionHandling("Программа остановлена по причине:", exception);
        }
        bankingSystemDbContext.Dispose();
    }

    private static void UpdatingEmployeeTest(EmployeeService employeeService, Employee bankEmployee)
    {
        Console.WriteLine("Изменим сотруднику имя и фамилию:");
        Console.WriteLine(
            $"До изменения {bankEmployee.FirstName} {bankEmployee.LastName}. id {bankEmployee.EmployeeId}");

        employeeService.UpdateEmployee(bankEmployee.EmployeeId, "Иван", "Иванов");

        Console.WriteLine(
            $"После изменения {bankEmployee.FirstName} {bankEmployee.LastName}. id {bankEmployee.EmployeeId}");
    }

    private static void DeletingEmployeeTest(EmployeeService employeeService, Employee bankEmployee)
    {
        Console.WriteLine($"Удаление сотрудника с id - {bankEmployee.EmployeeId}");
        employeeService.DeleteEmployee(bankEmployee.EmployeeId);
    }

    private static void GettingEmployeesWithFilterTest(EmployeeService employeeService)
    {
        Console.WriteLine("Выведем первые 5 владельцев банка:");
        var filteredEmployees = employeeService.EmployeesWithFilterAndPagination(1, 5, isOwner: true);
        Console.WriteLine("Владельцы:");
        Console.WriteLine(string.Join("\n",
            filteredEmployees.Select(employee =>
                $"Имя {employee.FirstName}, фамилия {employee.LastName}, дата рождения {employee.DateOfBirth.ToString("D")}, зарплата: {employee.Salary} $, владелец: {employee.IsOwner}")));
    }
}