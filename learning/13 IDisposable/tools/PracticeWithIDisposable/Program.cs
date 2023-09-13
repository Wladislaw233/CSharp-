using Services;
using Microsoft.Extensions.Configuration;

namespace PracticeWithIDisposable;

internal class Program
{
    public static void Main(string[] args)
    {
        var configurationRoot = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        //1
        var bankingSystemDb = new BankingSystemDb(configurationRoot);
        bankingSystemDb.StartOpenConnections();
        
        //2
        bankingSystemDb.CreateConnectionsAndMemory(100);
        Console.WriteLine($"Total Allocated:{ConnectionAndMemory.TotalAllocated}");
        Console.WriteLine($"Total Freed: {ConnectionAndMemory.TotalFreed}");
    }
}