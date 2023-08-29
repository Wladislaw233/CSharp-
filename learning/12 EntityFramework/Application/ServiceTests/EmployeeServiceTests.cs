using BankingSystemServices.Services;
using Services;
using Services.Database;
using Services.Exceptions;

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
            Console.WriteLine("Добавленные 5 сотрудников:" +
                              "\n" + string.Join("\n",
                                  bankEmployees.Select(employee =>
                                      $"Имя: {employee.FirstName}, фамилия: {employee.LastName}, зарплата: {employee.Salary} $, владелец: {employee.IsOwner} ")));

            var bankEmployee = bankEmployees.FirstOrDefault();
            if (bankEmployee != null)
            {
                Console.WriteLine("Изменим сотруднику имя и фамилию:" +
                                  $"\nДо изменения {bankEmployee.FirstName} {bankEmployee.LastName}. id {bankEmployee.EmployeeId}");
                employeeService.UpdateEmployee(bankEmployee.EmployeeId, "Иван", "Иванов");

                Console.WriteLine(
                    $"После изменения {bankEmployee.FirstName} {bankEmployee.LastName}. id {bankEmployee.EmployeeId}");
                Console.WriteLine($"Удаление сотрудника с id - {bankEmployee.EmployeeId}");
                employeeService.DeleteEmployee(bankEmployee.EmployeeId);

                Console.WriteLine("Выведем владельцев банка:");
                var filteredEmployees = employeeService.EmployeesWithFilterAndPagination(1, 100, isOwner: true);
                Console.WriteLine("Сотрудники:\n" + string.Join("\n",
                    filteredEmployees.Select(employee =>
                        $"Имя {employee.FirstName}, фамилия {employee.LastName}, дата рождения {employee.DateOfBirth.ToString("D")}, зарплата: {employee.Salary} $, владелец: {employee.IsOwner}")));
            }
        }
        catch (CustomException exception)
        {
            CustomException.ExceptionHandling("Программа остановлена по причине:", exception);
        }
    }
}