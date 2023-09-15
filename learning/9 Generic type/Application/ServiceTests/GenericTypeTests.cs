using BankingSystemServices.Services;

namespace ServiceTests;

public static class GenericTypeTests
{
    public static void BankServiceTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Running generic type tests.");
        Console.WriteLine("Blacklist and bonuses:");
        Console.ResetColor();
        var bankService = new BankService();

        var testDataGenerator = new TestDataGenerator();
        var bankClients = testDataGenerator.GenerateListWithBankClients(100);
        var bankEmployees = testDataGenerator.GenerateListWithBankEmployees(100);

        var clientAge = 25;
        var employeeSalary = new decimal(150.67);
        
        Console.WriteLine(
            $"\nLet's add clients to the blacklist whose age is less than {clientAge} and employees whose salary is less than {employeeSalary}:");

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
                $"\nIs {employeeInBlackList.FirstName} {employeeInBlackList.LastName} blacklisted? " +
                $"- {isEmployeeInBlackList}");
        }
        else
            Console.WriteLine("The employee was not found on the blacklist!");

        var employeeBonus = new decimal(569.12);
        
        Console.WriteLine($"\nAccrual of a bonus to an employee in the amount of {employeeBonus}:");
        
        var employeeWithBonus = bankEmployees.FirstOrDefault();
        
        if (employeeWithBonus != null)
        {
            BankService.AddBonus(employeeWithBonus, employeeBonus);
            Console.WriteLine(
                $"Employee: {employeeWithBonus.FirstName} {employeeWithBonus.LastName}, bonus amount: {employeeWithBonus.Bonus}");
        }
        else
            Console.WriteLine("Employee not found!");
    }
}