using Services;

namespace ServiceTests;

public class GenericTypeTests
{
    public static void BankServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Черный список и бонусы:");
        Console.ResetColor();
        var bankService = new BankService();

        var bankClients = TestDataGenerator.GenerateListWitchBankClients(100);
        var bankEmployees = TestDataGenerator.GenerateListWithEmployees(100);

        Console.WriteLine(
            "\nДобавим клиентов в черный  список возраст которых меньше 20 и сотрудников имя которых 'Ralph':");

        var blacklistedClients = bankClients.Where(client => client.Age < 20).ToList();
        foreach (var client in blacklistedClients)
            bankService.AddToBlackList(client);

        var blacklistedEmployee = bankEmployees.Where(employee => employee.FirstName == "Ralph").ToList();
        foreach (var employee in blacklistedEmployee)
            bankService.AddToBlackList(employee);

        bankService.WithdrawPersonInBlackList();
        var employeeInBlackList = blacklistedEmployee.FirstOrDefault();
        if (employeeInBlackList != null)
            Console.WriteLine(
                $"\nЕсть ли сотрудник {employeeInBlackList.FirstName} {employeeInBlackList.LastName} в черном списке? " +
                $"- {bankService.IsPersonInBlackList(employeeInBlackList)}");

        Console.WriteLine("\nНачисление бонуса сотруднику:");
        var employeeWithBonus = bankEmployees.FirstOrDefault();
        if (employeeWithBonus != null)
        {
            BankService.AddBonus(employeeWithBonus, 569.12);
            Console.WriteLine(
                $"Сотрудник: {employeeWithBonus.FirstName} {employeeWithBonus.LastName},бонус: {employeeWithBonus.Bonus}");
        }
    }
}