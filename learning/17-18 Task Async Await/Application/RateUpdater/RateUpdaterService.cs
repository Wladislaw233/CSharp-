using Microsoft.EntityFrameworkCore;
using Services.Database;

namespace RateUpdater;

public class RateUpdaterService : IRateUpdaterService
{
    public async Task UpdateRatesAsync()
    {
        while (true)
        {
            await UpdateRatesForAllClientsAsync();
            await Task.Delay(TimeSpan.FromDays(30));
        }
    }

    private static async Task UpdateRatesForAllClientsAsync()
    {
        Console.WriteLine("Начало обработки...");
        await using var bankingSystemDbContext = new BankingSystemDbContext();

        var accounts = await bankingSystemDbContext.Accounts.ToListAsync();

        foreach (var account in accounts)
            account.Amount *= 1.01;

        await bankingSystemDbContext.SaveChangesAsync();
        await bankingSystemDbContext.DisposeAsync();
        Console.WriteLine("Конец обработки. Проценты начислены.\nСледующая обработка через 30 дней.");
    }
}