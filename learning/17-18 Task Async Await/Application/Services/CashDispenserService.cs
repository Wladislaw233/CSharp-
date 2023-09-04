using BankingSystemServices.Database;
using BankingSystemServices.Models;
using BankingSystemServices.Services;

namespace Services;

public class CashDispenserService
{
    private readonly object _locker = new();

    public static async Task CashOutAsync(Client client)
    {
        await using (var bankingSystemDbContext = new BankingSystemDbContext())
        {
            var clientAccounts =
                bankingSystemDbContext.Accounts.Where(account => account.ClientId.Equals(client.ClientId));
            foreach (var account in clientAccounts)
                account.Amount = 0;
            await bankingSystemDbContext.SaveChangesAsync();
        }
    }
}