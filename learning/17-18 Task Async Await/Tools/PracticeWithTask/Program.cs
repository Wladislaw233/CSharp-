
using ServiceTests;

namespace PracticeWithTask;

internal class Program
{
    public static void Main(string[] args)
    {
        // task 16
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Фоновая задача на начисление процентов. (16)");
        Console.ResetColor();
        var updateRatesTask = RateUpdaterTests.RateUpdaterTest();
        Thread.Sleep(2000);
        
        // task 17 a.
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
        
        updateRatesTask.Wait();
        clientServiceTest.Wait();
        threadPoolTest.Wait();
    }
}