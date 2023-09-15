using System.Globalization;
using System.Text.RegularExpressions;

namespace Services;

public static class CurrencyService
{
    private const string BaseUri = "https://api.apilayer.com/fixer";
    private const string ApiKey = "tlV2586VfaaAFaKWdoaG71Hg4fpBZreS";

    public static async Task<decimal> CurrencyConverter(string fromCurrencyCode, string toCurrencyCode,
        decimal amount)
    {
        var regex = new Regex("\\s+");

        fromCurrencyCode = regex.Replace(fromCurrencyCode, "");
        toCurrencyCode = regex.Replace(toCurrencyCode, "");

        using var httpClient = new HttpClient();
        {
            httpClient.DefaultRequestHeaders.Add("apikey", ApiKey);

            var amountStr = Convert.ToString(amount, CultureInfo.InvariantCulture).Replace(",", ".");
            
            var methodUri = $"{BaseUri}/convert?" +
                            $"from={fromCurrencyCode}" +
                            $"&to={toCurrencyCode}" +
                            $"&amount={amountStr}";

            var responseMessage = await httpClient.GetAsync(methodUri);

            responseMessage.EnsureSuccessStatusCode();

            var message = await responseMessage.Content.ReadAsStringAsync();

            var match = Regex.Match(message,
                             "\"result\":\\s(.*)\\w",
                                    RegexOptions.Singleline);

            if (match is not { Success: true, Groups.Count: > 1 })
                throw new HttpRequestException("Error while retrieving value.");

            var resultAmountStr = match.Groups[1].Value.Replace(".", ",");
            
            var result = Convert.ToDouble(resultAmountStr);
            
            return new decimal(result);
        }
    }
}