using Services;
using Services.Storage;

namespace ServiceTests;

public class EnumerableTests
{
    public static void GetClientsByFiltersTest()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Клиенты");
        Console.ResetColor();
        var clientStorage = new ClientStorage();
        clientStorage.AddBankClients(100);
        var filteredClients =
            ClientService.GetClientsByFilters(clientStorage,
                "Stephan",
                minDateOfBirth: new DateTime(1970, 1, 1),
                maxDateOfBirth: new DateTime(2000, 1, 1));
        Console.WriteLine("Поиск клиентов с именем 'Stephan' и датой рождения большей 1970 год и меньшей 2000 года:");
        foreach (var client in filteredClients)
            Console.WriteLine($"Клиент: {client.FirstName} {client.LastName}, " +
                              $"дата рождения: {client.DateOfBirth.ToString("D")}, " +
                              $"телефон: {client.PhoneNumber}, " +
                              $"почта: {client.Email}");

        var sortedByDateOfBirthClients = clientStorage.OrderByDescending(client => client.DateOfBirth).ToList();
        var youngestClient = sortedByDateOfBirthClients.First();
        Console.WriteLine($"\nСамый молододой клиент: {youngestClient.FirstName} {youngestClient.LastName}, " +
                          $"дата рождения {youngestClient.DateOfBirth.ToString("D")}");

        var oldestClient = sortedByDateOfBirthClients.Last();
        Console.WriteLine($"\nСамый старый клиент: {oldestClient.FirstName} {oldestClient.LastName}, " +
                          $"дата рождения {oldestClient.DateOfBirth.ToString("D")}");

        var averageAge = clientStorage.Average(client => client.Age);
        Console.WriteLine($"\nСредний возраст клиентов - {averageAge}");
    }

    public static void GetEmployeesByFiltersTest()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Сотрудники");
        Console.ResetColor();
        var employeeStorage = new EmployeeStorage();
        employeeStorage.AddBankEmployees(100);
        var filteredEmployees =
            EmployeeService.GetEmployeesByFilters(employeeStorage,
                "Ralph",
                minDateOfBirth: new DateTime(1970, 1, 1),
                maxDateOfBirth: new DateTime(2000, 1, 1));
        Console.WriteLine("Поиск сотрудников с именем 'Ralph' и датой рождения большей 1970 год и меньшей 2000 года:");
        foreach (var employee in filteredEmployees)
            Console.WriteLine($"Сотрудник: {employee.FirstName} {employee.LastName}, " +
                              $"дата рождения: {employee.DateOfBirth.ToString("D")}, " +
                              $"зарплата: {employee.Salary}, " +
                              $"контракт: {employee.Contract}");

        var sortedByDateOfBirthEmployees = employeeStorage.OrderByDescending(employee => employee.DateOfBirth).ToList();
        var youngestEmployee = sortedByDateOfBirthEmployees.First();
        Console.WriteLine($"\nСамый молододой сотрудник: {youngestEmployee.FirstName} {youngestEmployee.LastName}, " +
                          $"дата рождения {youngestEmployee.DateOfBirth.ToString("D")}");

        var oldestEmployee = sortedByDateOfBirthEmployees.Last();
        Console.WriteLine($"\nСамый старый сотрудник: {oldestEmployee.FirstName} {oldestEmployee.LastName}, " +
                          $"дата рождения {oldestEmployee.DateOfBirth.ToString("D")}");

        var averageSalary = employeeStorage.Average(employee => employee.Salary);
        Console.WriteLine($"\nСредняя зарплата сотрудников - {averageSalary}");
    }
}