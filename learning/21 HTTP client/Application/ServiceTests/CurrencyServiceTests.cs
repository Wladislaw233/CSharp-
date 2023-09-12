using Services;

namespace ServiceTests;

public class CurrencyServiceTests
{
    public static void CurrencyConverterTest()
    {
        var fromCurrencyCode = "USD";
        var toCurrencyCode = "EUR";
        var amount = new decimal(1235.23);
        
        var currencyService = new CurrencyService();

        try
        {
            var result = currencyService.CurrencyConverter(fromCurrencyCode, toCurrencyCode, amount).GetAwaiter()
                .GetResult();

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