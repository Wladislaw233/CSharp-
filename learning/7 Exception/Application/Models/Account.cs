namespace Models;

public class Account
{
    public Currency Currency { get; set; }
    public double Amount { get; set; }
    public string AccountNumber { get; set; }

    public Account(string accountNumber, object currency = null, double amount = 0, string code = "USD",
        string name = "USD", double exchangeRate = 1)
    {
        if (currency != null && currency is Currency)
            Currency = (Currency)currency;
        else
            Currency = new Currency
            {
                Code = code,
                Name = name,
                ExchangeRate = exchangeRate
            };
        Amount = amount;
        AccountNumber = accountNumber;
    }
}