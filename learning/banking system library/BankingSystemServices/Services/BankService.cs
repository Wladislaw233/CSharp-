using BankingSystemServices.Models;

namespace BankingSystemServices.Services;

public class BankService
{
    private readonly List<Person> _blackList = new();

    public static void CalculateSalary(double profit, double expenses, List<Employee> employees)
    {
        var salary = (profit - expenses) / employees.Count;
        foreach (var employee in employees) employee.Salary = (int)salary;
    }

    public static void AddBonus<T>(T person, decimal bonusAmount) where T : Person
    {
        person.Bonus += bonusAmount;
    }

    public void AddToBlackList<T>(T person) where T : Person
    {
        _blackList.Add(person);
    }

    public bool IsPersonInBlackList<T>(T person) where T : Person
    {
        return _blackList.Contains(person);
    }

    public void WithdrawPersonInBlackList()
    {
        Console.WriteLine("Черный список:");
        foreach (var person in _blackList)
            Console.WriteLine(
                $"Имя: {person.FirstName}, фамилия: {person.LastName}, дата рождения: {person.DateOfBirth.ToString("D")}");
    }
}