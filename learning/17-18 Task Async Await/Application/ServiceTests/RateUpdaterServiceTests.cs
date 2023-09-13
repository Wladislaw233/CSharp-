using Services;

namespace ServiceTests;

public static class RateUpdaterServiceTests
{
    public static async Task RateUpdaterTest()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Background task for calculating interest. (16)");
        Console.ResetColor();
        
        Console.WriteLine("Start a background task.");

        await RateUpdaterService.UpdateRatesAsync();
    }
}