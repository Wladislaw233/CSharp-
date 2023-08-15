using Services;

namespace EventsStreamNumbers;

internal class Program
{
    public static void Main(string[] args)
    {
        /* реализовать класс анализирующий поток чисел, и если число
        отличается более чем x - процентов выбрасывает событие */

        var numberAnalysis = new StreamNumbersAnalysis();
        var eventHandlerNumberBiggerXPercent = new EventHandlerIsGreaterThanByPercentage();
        numberAnalysis.EventIsGreaterThanByPercentage += eventHandlerNumberBiggerXPercent.HandlerIsGreaterThanByPercentage;
        numberAnalysis.StartStreamNumbersAnalysis();
    }
}