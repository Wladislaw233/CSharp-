using BankingSystemServices.Database;
using Microsoft.EntityFrameworkCore;

namespace Services;

public static class RateUpdaterService
{
    public static async Task UpdateRatesAsync()
    {
        while (true)
        {
            await UpdateRatesForAllClientsAsync();
            await Task.Delay(TimeSpan.FromDays(30));
        }
        // ReSharper disable once FunctionNeverReturns
    }

    private static async Task UpdateRatesForAllClientsAsync()
    {
        Console.WriteLine("Start processing...");
        await using var bankingSystemDbContext = new BankingSystemDbContext();

        var accounts = await bankingSystemDbContext.Accounts.ToListAsync();

        foreach (var account in accounts)
            account.Amount *= new decimal(1.01);

        await bankingSystemDbContext.SaveChangesAsync();
        await bankingSystemDbContext.DisposeAsync();
        Console.WriteLine("End of processing. Interest accrued.\nNext processing in 30 days.");
    }
}