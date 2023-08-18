using Models;
using Services;
using Services.Exceptions;
using Services.Storage;

namespace ServiceTests;

public class EmployeeServiceTests
{
    private List<Employee> _bankEmployees = new();
    private readonly EmployeeStorage _employeeStorage = new();
    
    public void AddBankEmployeeTest()
    {
        _bankEmployees = TestDataGenerator.GenerateListWithEmployees(2, true);
        _bankEmployees[1].Contract = "";
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("СОТРУДНИКИ");
        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Добавление сотрудников:");
            Console.ResetColor();
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

        Console.WriteLine("\nСписок сотрудников после добавления:\n" + string.Join('\n',
            _employeeStorage.Data.Select(employee =>
                $"Имя: {employee.FirstName}, фамилия: {employee.LastName}, контракт: {employee.Contract}")));
    }
}