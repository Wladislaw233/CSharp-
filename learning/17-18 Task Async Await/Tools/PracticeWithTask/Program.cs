using System.Net.NetworkInformation;
using BankingSystemServices.Database;
using ServiceTests;

namespace PracticeWithTask;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        // task 16
        var updateRatesTask = RateUpdaterServiceTests.RateUpdaterTest();

        await Task.Delay(1700);

        await CashDispenserServiceTests.CashDispenserServiceTest();

        // task 17 a.

        await ThreadPoolTests.StartThreadPoolTests();

        // task 17 b.

        await using (var bankingSystemDbContext = new BankingSystemDbContext())
        {
            var clientServiceTests = new ClientServiceTests(bankingSystemDbContext);
            await clientServiceTests.ClientServiceTest();

            var employeeServiceTests = new EmployeeServiceTests(bankingSystemDbContext);
            await employeeServiceTests.EmployeeServiceTest();
        }

        await updateRatesTask;
        
        /*Console.WriteLine($"Main - thread id: {Environment.CurrentManagedThreadId}, task id - {Task.CurrentId}");
        await PrintAsync();
        Console.WriteLine($"Main - thread id: {Environment.CurrentManagedThreadId}, task id - {Task.CurrentId}");*/
    }

    /*private static Task PrintAsync()
    {
        Console.WriteLine($"Task 1 start -  {Environment.CurrentManagedThreadId}, task id - {Task.CurrentId}");
        Thread.Sleep(1000);
        return Task.CompletedTask;
    }
    
    private static async Task PrintAsync()
    {
        Console.WriteLine($"Task 1 start - thread id: {Environment.CurrentManagedThreadId}, task id - {Task.CurrentId}");
        
        var res1 = await PingHttp();
        
        var res2 = await PingIcmp();
        
        Console.WriteLine($"Task 1 end (result - {res1}, {res2}) - thread id: {Environment.CurrentManagedThreadId}, task id - {Task.CurrentId}");
    }

    private static async Task<string> PingHttp()
    {
        Console.WriteLine($"Task 2 start - thread id: {Environment.CurrentManagedThreadId}, task id - {Task.CurrentId}");
        
        using var http = new HttpClient();
        var result = await http.GetAsync("https://www.google.com");
        
        Console.WriteLine($"Task 2 end - thread id: {Environment.CurrentManagedThreadId}, task id - {Task.CurrentId}");
        
        return result.StatusCode.ToString();
    }
    
    private static async Task<string> PingIcmp()
    {
        Console.WriteLine($"Task 3 start - thread id: {Environment.CurrentManagedThreadId}, task id - {Task.CurrentId}");
        
        using var ping = new Ping();
        
        var result = await ping.SendPingAsync("www.google.com", 10);
        
        Console.WriteLine($"Task 3 end - thread id: {Environment.CurrentManagedThreadId}, task id - {Task.CurrentId}");
        
        return result.Status.ToString();
    }*/
}