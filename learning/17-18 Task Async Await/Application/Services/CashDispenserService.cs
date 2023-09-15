using BankingSystemServices.Database;
using Microsoft.EntityFrameworkCore;

namespace Services;

public static class CashDispenserService
{
    public static async Task CashOutAsync(Guid clientId)
    {
        await using var bankingSystemDbContext = new BankingSystemDbContext();

        var clientAccounts =
            await bankingSystemDbContext.Accounts.Where(account => account.ClientId.Equals(clientId)).ToListAsync();

        foreach (var account in clientAccounts)
            account.Amount = 0;

        await bankingSystemDbContext.SaveChangesAsync();
    }
}