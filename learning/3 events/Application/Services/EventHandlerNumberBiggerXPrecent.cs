namespace Services;

public class EventHandlerNumberBiggerXPrecent
{
    public void Message(int number, double precent, int inputNumber, double precentLimit)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{inputNumber} > {number} на {precent}%, что больше лимита ({precentLimit}%).");
        Console.ResetColor();
    }
}