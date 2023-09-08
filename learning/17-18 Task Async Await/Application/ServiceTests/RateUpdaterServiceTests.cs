using Microsoft.Extensions.DependencyInjection;
using Services;

namespace ServiceTests;

public class RateUpdaterServiceTests
{
    public static async Task RateUpdaterTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Фоновая задача на начисление процентов. (16)");
        Console.ResetColor();
        
        var rateUpdaterService = new RateUpdaterService();
        
        Console.WriteLine("Старт фоновой задачи.");
        
        await rateUpdaterService.UpdateRatesAsync();
    }
}