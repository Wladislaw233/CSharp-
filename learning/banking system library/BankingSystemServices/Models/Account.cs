using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemServices.Models;

public class Account
{
    public Guid AccountId { get; set; }
    
    public decimal Amount { get; set; }
    
    public string AccountNumber { get; set; }
    
    public Guid ClientId { get; set; }
    
    public Guid CurrencyId { get; set; }
    
    public virtual Client? Client { get; set; }
    
    public virtual Currency? Currency { get; set; }
}