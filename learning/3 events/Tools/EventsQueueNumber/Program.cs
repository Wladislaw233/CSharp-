using Services;

namespace EventsQueueNumber;

internal class Program
{
    public static void Main(string[] args)
    {
        /* реализовать очередь, которая генерирует событие, когда кол-во
        объектов в ней превышает n и событие, когда становится пустой */

        var queue = new NumberQueueAnalyzer();
        queue.EventQueueLimitReached += HandlerQueueLimitReached;
        queue.EventEmptyQueue += HandlerEmptyArray;
        queue.RecursiveQueueInput();
    }

    private static void HandlerEmptyArray()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Список пуст!");
        Console.ResetColor();
    }

    private static void HandlerQueueLimitReached(int numberLimit)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Количество элементов списка превышает {0}", numberLimit);
        Console.ResetColor();
    }
}