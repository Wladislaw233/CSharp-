using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemServices.Models;

public class Currency
{
    public Guid CurrencyId { get; set; }
    
    public string Code { get; set; }
    
    public string Name { get; set; }
    
    public decimal ExchangeRate { get; set; }
}