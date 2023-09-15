using Services;

namespace ServiceTests;

public static class CurrencyServiceTests
{
    public static async Task CurrencyConverterTest()
    {
        const string fromCurrencyCode = "USD";
        const string toCurrencyCode = "EUR";
        var amount = new decimal(1235.23);
        
        try
        {
            var result = await CurrencyService.CurrencyConverter(fromCurrencyCode, toCurrencyCode, amount);

            Console.WriteLine($"Convert {amount:F} {fromCurrencyCode} to {toCurrencyCode}: {result:F}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Something went wrong: " +
                              $"\nMessage: {ex.Message}" +
                              $"\nStatus code: {ex.StatusCode}" +
                              $"\nStacktrace: {ex.StackTrace}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong: " +
                              $"\nMessage: {ex.Message}" +
                              $"\nStacktrace: {ex.StackTrace}");
        }
    }
}