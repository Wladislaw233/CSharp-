using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemServices;

public class Account
{
    [Required]
    [Column("id")]
    public Guid AccountId { get; set; } = Guid.NewGuid();

    [Required] 
    [Column("currency_id")] 
    public Guid CurrencyId { get; set; }
    
    [Column("amount")]
    public decimal Amount { get; set; }
    
    [Required]
    [Column("account_number")]
    public string AccountNumber { get; set; }
    
    [Required]
    [Column("client_id")]
    public Guid ClientId { get; set; }
    
    public virtual Client? Client { get; set; }
    
    public virtual Currency? Currency { get; set; }
}