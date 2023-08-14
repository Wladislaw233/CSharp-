using Services;

namespace Events;

internal class Program
{
    public static void Main(string[] args)
    {
        /* реализовать класс анализирующий поток чисел, и если число
        отличается более чем x - процентов выбрасывает событие */

        var numberAnalis = new NumberAnalis();
        var eventHandlerNumberBiggerXPrecent = new EventHandlerNumberBiggerXPrecent();
        numberAnalis.NumberBiggerXPrecent += eventHandlerNumberBiggerXPrecent.Message;
        numberAnalis.StartAnalis();
    }
}