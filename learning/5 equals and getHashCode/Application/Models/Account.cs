namespace Models;

public class Account
{
    public Currency Currency { get; set; }
    public double Amount { get; set; }

    public Account(string code = "USD", string name = "USD", double exchangeRate = 1, double amount = 0)
    {
        Currency = new Currency
        {
            Code = code,
            Name = name,
            ExchangeRate = exchangeRate
        };
        Amount = amount;
    }
}