using Services;

namespace EventsStreamNumber;

internal class Program
{
    public static void Main(string[] args)
    {
        /* реализовать класс анализирующий поток чисел, и если число
        отличается более чем x - процентов выбрасывает событие */

        var numberAnalysis = new NumberStreamAnalyzer();
        numberAnalysis.EventIsGreaterThanByPercentage += HandlerIsGreaterThanByPercentage;
        numberAnalysis.StartStreamNumbersAnalysis();
    }

    private static void HandlerIsGreaterThanByPercentage(int numberLimit, double percent, int inputNumber,
        double percentLimit)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{inputNumber} > {numberLimit} на {percent}%, что больше лимита ({percentLimit}%).");
        Console.ResetColor();
    }
}