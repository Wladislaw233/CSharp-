using Service;

namespace PracticeWithExtensions;

internal class Program
{
    public static void Main(string[] args)
    {
        int seconds = 5;
        TimeSpan timeSpan = seconds.Seconds();
        Console.WriteLine($"TimeSpan: {timeSpan}");

        int minutes = 10;
        timeSpan = minutes.Minutes();
        Console.WriteLine($"TimeSpan: {timeSpan}"); 
    }
}