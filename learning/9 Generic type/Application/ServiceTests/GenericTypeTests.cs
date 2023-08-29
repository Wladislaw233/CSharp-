using BankingSystemServices.Services;

namespace ServiceTests;

public class GenericTypeTests
{
    public static void BankServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Запуск тестов generic type.");
        Console.WriteLine("Черный список и бонусы:");
        Console.ResetColor();
        var bankService = new BankService();

        var bankClients = TestDataGenerator.GenerateListWitchBankClients(100);
        var bankEmployees = TestDataGenerator.GenerateListWithBankEmployees(100);

        Console.WriteLine(
            "\nДобавим клиентов в черный  список возраст которых меньше 25 и сотрудников зарплата которых меньше 150.67:");

        var blacklistedClients = bankClients.Where(client => client.Age < 25).ToList();
        foreach (var client in blacklistedClients)
            bankService.AddToBlackList(client);

        var blacklistedEmployee = bankEmployees.Where(employee => employee.Salary < new decimal(150.67)).ToList();
        foreach (var employee in blacklistedEmployee)
            bankService.AddToBlackList(employee);

        bankService.WithdrawPersonInBlackList();
        var employeeInBlackList = blacklistedEmployee.FirstOrDefault();
        if (employeeInBlackList != null)
            Console.WriteLine(
                $"\nЕсть ли сотрудник {employeeInBlackList.FirstName} {employeeInBlackList.LastName} в черном списке? " +
                $"- {bankService.IsPersonInBlackList(employeeInBlackList)}");

        Console.WriteLine("\nНачисление бонуса сотруднику в размере 569.12:");
        var employeeWithBonus = bankEmployees.FirstOrDefault();
        if (employeeWithBonus != null)
        {
            BankService.AddBonus(employeeWithBonus, new decimal(569.12));
            Console.WriteLine(
                $"Сотрудник: {employeeWithBonus.FirstName} {employeeWithBonus.LastName},бонус: {employeeWithBonus.Bonus}");
        }
    }
}