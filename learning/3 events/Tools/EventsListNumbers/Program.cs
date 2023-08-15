using Services;

namespace EventsListNumbers;

internal class Program
{
    public static void Main(string[] args)
    {
        /* реализовать очередь, которая генерирует событие, когда кол-во
        объектов в ней превышает n и событие, когда становится пустой */

        var queue = new QueueNumbersAnalysis();
        var eventHandlerEmptyArray = new EventHandlerEmptyArray();
        var eventHandlerQueueLimitReached = new EventHandlerQueueLimitReached();
        queue.EventQueueLimitReached += eventHandlerQueueLimitReached.HandlerQueueLimitReached;
        queue.EventEmptyQueue += eventHandlerEmptyArray.HandlerEmptyArray;
        queue.RecursiveQueueInput();
    }
}