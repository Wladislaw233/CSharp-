using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;
using BankingSystemServices.Exceptions;
using Services.Storage;

namespace ServiceTests;

public class EmployeeStorageTests
{
    private static List<Employee> _bankEmployees = new();
    private static readonly EmployeeStorage _employeeStorage = new();
    private static readonly EmployeeService _employeeService = new(_employeeStorage);

    public static void EmployeeStorageTest()
    {
        _bankEmployees = TestDataGenerator.GenerateListWithBankEmployees(3);
        _bankEmployees.Add(TestDataGenerator.GenerateRandomInvalidEmployee(true));
        _bankEmployees.Add(TestDataGenerator.GenerateRandomInvalidEmployee());

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("СОТРУДНИКИ");

        AddingEmployeeTest();
    }

    private static void AddingEmployeeTest()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Добавление сотрудников:");
        Console.ResetColor();
        try
        {
            foreach (var employee in _bankEmployees)
            {
                Console.WriteLine(
                    $"\nПопытка добавления сотрудника: Имя: {employee.FirstName}, фамилия: {employee.LastName}, контракт: {employee.Contract}");
                _employeeStorage.Add(employee);
                Console.WriteLine("Успешно!");
            }
        }
        catch (CustomException exception)
        {
            CustomException.ExceptionHandling("При добавлении сотрудника произошла ошибка: ", exception);
        }

        Console.WriteLine("\nСписок сотрудников после добавления:");
        _employeeService.WithdrawEmployees();
    }
}