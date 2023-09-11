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

        var bankClients = TestDataGenerator.GenerateListWithBankClients(100);
        var bankEmployees = TestDataGenerator.GenerateListWithBankEmployees(100);

        var clientAge = 25;
        var employeeSalary = new decimal(150.67);
        
        Console.WriteLine(
            $"\nДобавим клиентов в черный  список возраст которых меньше {clientAge} и сотрудников зарплата которых меньше {employeeSalary}:");

        var blacklistedClients = bankClients.Where(client => client.Age < clientAge).ToList();
        
        foreach (var client in blacklistedClients)
            bankService.AddToBlackList(client);

       
        var blacklistedEmployee = bankEmployees.Where(employee => employee.Salary < employeeSalary).ToList();
        
        foreach (var employee in blacklistedEmployee)
            bankService.AddToBlackList(employee);

        bankService.WithdrawPersonInBlackList();
        var employeeInBlackList = blacklistedEmployee.FirstOrDefault();

        if (employeeInBlackList != null)
        {
            var isEmployeeInBlackList = bankService.IsPersonInBlackList(employeeInBlackList);
            Console.WriteLine(
                $"\nЕсть ли сотрудник {employeeInBlackList.FirstName} {employeeInBlackList.LastName} в черном списке? " +
                $"- {isEmployeeInBlackList}");
        }
        else
            Console.WriteLine("Сотрудник из черного листа не найден!");

        var employeeBonus = new decimal(569.12);
        
        Console.WriteLine($"\nНачисление бонуса сотруднику в размере {employeeBonus}:");
        
        var employeeWithBonus = bankEmployees.FirstOrDefault();
        
        if (employeeWithBonus != null)
        {
            BankService.AddBonus(employeeWithBonus, employeeBonus);
            Console.WriteLine(
                $"Сотрудник: {employeeWithBonus.FirstName} {employeeWithBonus.LastName}, бонус: {employeeWithBonus.Bonus}");
        }
        else
            Console.WriteLine("Сотрудник с бонусом не найден!");
    }
}