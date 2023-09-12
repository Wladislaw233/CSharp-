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
    
    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Account))
            return false;

        var account = (Account)obj;

        return account.AccountNumber == AccountNumber &&
               account.CurrencyId.Equals(CurrencyId) &&
               account.Amount == Amount &&
               account.ClientId.Equals(ClientId);
    }

    public override int GetHashCode()
    {
        var hash = 14;
        hash = hash * 17 + AccountNumber.GetHashCode();
        hash = hash * 17 + Amount.GetHashCode();
        hash = hash * 17 + ClientId.GetHashCode();
        hash = hash * 17 + CurrencyId.GetHashCode();
        
        return hash;
    }
}