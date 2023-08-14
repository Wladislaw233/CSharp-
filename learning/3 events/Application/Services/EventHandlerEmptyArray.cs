namespace Services;

public class EventHandlerEmptyArray
{
    public void Message()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Список пуст!");
        Console.ResetColor();
    }
}