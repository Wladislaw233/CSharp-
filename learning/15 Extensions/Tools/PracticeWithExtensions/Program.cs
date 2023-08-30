using Service;

namespace PracticeWithExtensions;

internal class Program
{
    public static void Main(string[] args)
    {
        var seconds = 5;
        var timeSpan = seconds.Seconds();
        Console.WriteLine($"Секунды: {timeSpan}");

        var minutes = 10;
        timeSpan = minutes.Minutes();
        Console.WriteLine($"Минуты: {timeSpan}");

        var hours = 22;
        timeSpan = hours.Hours();
        Console.WriteLine($"Часы: {timeSpan}");

        var days = 12;
        timeSpan = days.Days();
        Console.WriteLine($"Дни: {timeSpan}");
    }
}