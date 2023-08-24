
using ServiceTests;

namespace PracticeWithTask;

internal class Program
{
    public static async Task Main(string[] args)
    {
        await RateUpdaterTests.RateUpdaterTest();
    }
}