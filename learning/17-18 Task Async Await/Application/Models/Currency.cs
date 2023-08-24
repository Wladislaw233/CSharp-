using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Currency
{
    [Required]
    [Column("id")]
    public Guid CurrencyId { get; set; }
    
    [Required]
    [Column("code")]
    public string Code { get; set; }
    
    [Required]
    [Column("name")]
    public string Name { get; set; }
    
    [Required]
    [Column("exchange_rate")]
    public double ExchangeRate { get; set; }
    
    public Currency(string code, string name, double exchangeRate)
    {
        CurrencyId = Guid.NewGuid();
        Code = code;
        Name = name;
        ExchangeRate = exchangeRate;
    }
}