namespace Services;

public class EventHandlerQueueLimitReached
{
    public void HandlerQueueLimitReached(int numberLimit)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Количество элементов списка превышает {0}", numberLimit);
        Console.ResetColor();
    }
}