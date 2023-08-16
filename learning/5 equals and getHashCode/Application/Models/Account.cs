namespace Models;

public class Account
{
    public Currency Currency { get; init; }
    public double Amount { get; init; }

    public Account(string name = "USD-RUB", double course = 101.93, double amount = 0)
    {
        Currency = new Currency { Name = name, Course = course };
        Amount = amount;
    }
}