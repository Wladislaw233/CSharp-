using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemServices.Models;

public class Currency
{
    public Guid CurrencyId { get; set; }
    
    public string Code { get; set; }
    
    public string Name { get; set; }
    
    public decimal ExchangeRate { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Currency))
            return false;

        var currency = (Currency)obj;

        return currency.Code == Code &&
               currency.ExchangeRate == ExchangeRate &&
               currency.Name == Name;
    }

    public override int GetHashCode()
    {
        var hash = 14;
        hash = hash * 17 + Name.GetHashCode();
        hash = hash * 17 + ExchangeRate.GetHashCode();
        hash = hash * 17 + Code.GetHashCode();

        return hash;
    }
}