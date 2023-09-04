
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

        cashDispenserServiceTask.Wait();
        updateRatesTask.Wait();
        
        
        
       /* // task 17 a.
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("17 пункт а. разбор работы ассинхроного запуска задач.");
        Console.ResetColor();
        var threadPoolTest= ThreadPoolTests.ThreadPoolTest();
        Thread.Sleep(17000);
        
        // task 17 b.
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("17 пункт б. проверка ClientService после доработки с учетом async и await.");
        Console.ResetColor();
        var clientServiceTest = ClientServiceTests.ClientServiceTest();
        
        
        clientServiceTest.Wait();
        threadPoolTest.Wait();*/
    }
}