using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Account
{
    [Required]
    [Column("id")]
    public Guid AccountId { get; set; }

    [Required] 
    [Column("currency_id")] 
    public Guid CurrencyId { get; set; }
    
    [Column("amount")]
    public double Amount { get; set; }
    
    [Required]
    [Column("account_number")]
    public string AccountNumber { get; set; }
    [Required]
    [Column("client_id")]
    public Guid ClientId { get; set; }
    public virtual Client? Client { get; set; }
    public virtual Currency? Currency { get; set; }
    
    public Account(Guid currencyId, Guid clientId, string accountNumber, double amount)
    {
        CurrencyId = currencyId;
        AccountId = Guid.NewGuid();
        Amount = amount;
        AccountNumber = accountNumber;
        ClientId = clientId;
    }
}