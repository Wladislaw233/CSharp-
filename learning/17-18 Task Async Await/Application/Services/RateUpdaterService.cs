using System.Diagnostics;
using BankingSystemServices.Database;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class RateUpdaterService
{
    private int _chunkSize;

    public async Task UpdateRatesAsync()
    {
        while (true)
        {
            await UpdateRatesForAllClientsAsync();
            await Task.Delay(TimeSpan.FromDays(30));
        }
    }

    private async Task UpdateRatesForAllClientsAsync()
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        Console.WriteLine("Start processing...");

        const int totalThread = 10; // not bigger 80.

        /*await using var bankingSystemDbContext = new BankingSystemDbContext();
        var accounts = await bankingSystemDbContext.Accounts.ToListAsync();
        foreach (var account in accounts)
        {
            account.Amount *= new decimal(1.01);
        }
        await bankingSystemDbContext.SaveChangesAsync();*/

        await using (var bankingSystemDbContext = new BankingSystemDbContext())
        {
            _chunkSize = await bankingSystemDbContext.Accounts.CountAsync() / totalThread + 1;
        }

        Func<int, Task> asyncFunc = async index => await ProcessingAccountsAsync(index);

        var tasks = new Task[totalThread];

        Parallel.For(0, totalThread, index => tasks[index] = asyncFunc.Invoke(index));

        await Task.WhenAll(tasks);

        stopWatch.Stop();
        Console.WriteLine($"Update time: {stopWatch.Elapsed.TotalMilliseconds} ms.");
        Console.WriteLine("End of processing. Interest accrued.\nNext processing in 30 days.");
    }

    private async Task ProcessingAccountsAsync(int index)
    {
        await using var dbContext = new BankingSystemDbContext();

        var chunk = await dbContext.Accounts
            .OrderBy(account => account.AccountNumber)
            .Skip(_chunkSize * index)
            .Take(_chunkSize)
            .ToListAsync();

        foreach (var account in chunk)
            account.Amount *= new decimal(1.01);

        await dbContext.SaveChangesAsync();
    }
}