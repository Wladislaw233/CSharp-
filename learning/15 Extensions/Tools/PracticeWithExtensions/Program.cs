using Service;

namespace PracticeWithExtensions;

internal static class Program
{
    public static void Main(string[] args)
    {
        const int seconds = 5;
        var timeSpan = seconds.Seconds();
        Console.WriteLine($"Seconds: {timeSpan}");

        const int minutes = 10;
        timeSpan = minutes.Minutes();
        Console.WriteLine($"Minutes: {timeSpan}");

        const int hours = 22;
        timeSpan = hours.Hours();
        Console.WriteLine($"Hours: {timeSpan}");

        const int days = 12;
        timeSpan = days.Days();
        Console.WriteLine($"Days: {timeSpan}");
    }
}