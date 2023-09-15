using ServiceTests;

namespace PracticeWithHTTPClient;

internal class Program
{
    public static async Task Main(string[] args)
    {
        await CurrencyServiceTests.CurrencyConverterTest();
    }
}