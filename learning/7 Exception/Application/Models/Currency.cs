namespace Models;

public struct Currency
{
    public string Code { get; set; }
    public string Name { get; set; }
    public double ExchangeRate { get; set; }

    public Currency(string code, string name, double exchangeRate)
    {
        Code = code;
        Name = name;
        ExchangeRate = exchangeRate;
    }
}