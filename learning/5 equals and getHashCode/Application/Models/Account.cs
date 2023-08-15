namespace Models;

public class Account 
{
    public Currency Currency { get; set; }
    public double Amount { get; set; }

    public Account(string name = "USD-RUB", double course = 101.93, double amount = 0)
    {
        Currency = new Currency() { Name = name, Course = course };
        Amount = amount;
    }

    public static string ConvertToStringAccountsList(List<Account> clientAccounts)
    {
        var result = "";
        foreach (var account in clientAccounts)
            result += "Курс: " + account.Currency.Name + " " + account.Currency.Course + "р., баланс: " +
                      account.Amount + " р.\n";
        return result;
    }
}