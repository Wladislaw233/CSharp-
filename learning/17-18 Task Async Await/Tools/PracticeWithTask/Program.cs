
using Services;
using ServiceTests;

namespace PracticeWithTask;

internal class Program
{
    public static void Main(string[] args)
    {
        // task 16
        var updateRatesTask = RateUpdaterServiceTests.RateUpdaterTest();

        Thread.Sleep(1000);
        
        var cashDispenserServiceTask = CashDispenserServiceTests.CashDispenserServiceTest();

        Thread.Sleep(500);
        
        // task 17 a.
        
        ThreadPoolTests.StartThreadPoolTests();
        
        // task 17 b.
        
        ClientServiceTests.ClientServiceTest();
        EmployeeServiceTests.EmployeeServiceTest();
        
        cashDispenserServiceTask.GetAwaiter().GetResult();
        updateRatesTask.GetAwaiter().GetResult();
        
    }
}