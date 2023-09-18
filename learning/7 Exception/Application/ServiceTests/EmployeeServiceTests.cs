using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;
using BankingSystemServices.Exceptions;

namespace ServiceTests;

public class EmployeeServiceTests
{
    public static void EmployeeServiceTest()
    {
        AddBankEmployeeTest();
    }
    
    private static void AddBankEmployeeTest()
    {
        var testDataGenerator = new TestDataGenerator();
        var bankEmployees = testDataGenerator.GenerateListWithBankEmployees(3);
        var employeeService = new EmployeeService();
        bankEmployees.Add(testDataGenerator.GenerateRandomInvalidEmployee(true));
        bankEmployees.Add(testDataGenerator.GenerateRandomInvalidEmployee());

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("СОТРУДНИКИ");
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Добавление сотрудников:");
        Console.ResetColor();
        try
        {
            foreach (var employee in bankEmployees)
            {
                Console.WriteLine(
                    $"\nПопытка добавления сотрудника: Имя: {employee.FirstName}, фамилия: {employee.LastName}, контракт: {employee.Contract}");
                employeeService.AddBankEmployee(employee);
                Console.WriteLine("Успешно!");
            }
        }
        catch (CustomException exception)
        {
            Console.WriteLine(exception);
        }

        Console.WriteLine("\nСписок сотрудников после добавления:");
        employeeService.WithdrawEmployees();
    }
}