using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemServices;

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
    public decimal ExchangeRate { get; set; }
}