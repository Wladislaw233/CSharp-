
using ServiceTests;

namespace PracticeWithTask;

internal class Program
{
    public static void Main(string[] args)
    {
        var updateRatesTask = Task.Run(async () => await RateUpdaterTests.RateUpdaterTest());
        Console.WriteLine("Продолжение выполнения Main..");
        updateRatesTask.Wait();
    }
}