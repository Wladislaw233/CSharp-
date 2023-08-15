namespace Services;

public class EventHandlerIsGreaterThanByPercentage
{
    public void HandlerIsGreaterThanByPercentage(int numberLimit, double percent, int inputNumber, double percentLimit)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{inputNumber} > {numberLimit} на {percent}%, что больше лимита ({percentLimit}%).");
        Console.ResetColor();
    }
}