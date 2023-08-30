using BankingSystemServices;
using BankingSystemServices.Services;
using Services;
using Services.Exceptions;
using Services.Storage;

namespace ServiceTests;

public class EmployeeStorageTests
{
    private static List<Employee> _bankEmployees = new();
    private static readonly EmployeeStorage EmployeeStorage = new();

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
        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Добавление сотрудников:");
            Console.ResetColor();
            foreach (var employee in _bankEmployees)
            {
                Console.WriteLine(
                    $"\nПопытка добавления сотрудника: Имя: {employee.FirstName}, фамилия: {employee.LastName}, контракт: {employee.Contract}");
                EmployeeStorage.Add(employee);
                Console.WriteLine("Успешно!");
            }
        }
        catch (CustomException exception)
        {
            CustomException.ExceptionHandling("При добавлении сотрудника произошла ошибка: ", exception);
        }

        Console.WriteLine("\nСписок сотрудников после добавления:");
        EmployeeService.WithdrawEmployees(EmployeeStorage);
    }
}