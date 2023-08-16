using Models;
using Services;
using Services.Exceptions;

namespace ServiceTests;

public class EmployeeServiceTests
{
    private List<Employee> _bankEmployees = new();
    private readonly EmployeeService _employeeService = new();
    
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
                _employeeService.AddBankEmployee(employee);
                Console.WriteLine("Успешно!");
            }
        }
        catch (CustomException exception)
        {
            ExceptionHandling("При добавлении сотрудника произошла ошибка: ", exception);
        }

        Console.WriteLine("\nСписок сотрудников после добавления:\n" + string.Join('\n',
            _employeeService.BankEmployees.Select(employee =>
                $"Имя: {employee.FirstName}, фамилия: {employee.LastName}, контракт: {employee.Contract}")));
    }
    private static void ExceptionHandling(string description, CustomException exception)
    {
        Console.WriteLine(description + exception.Message + (string.IsNullOrWhiteSpace(exception.ParameterOfException)
            ? ""
            : $"\nПараметр: {exception.ParameterOfException}"));
    }
}