using System;
using Services;

namespace Events
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            /* реализовать класс анализирующий поток чисел, и если число
            отличается более чем x - процентов выбрасывает событие */
            
            NumberAnalis numberAnalis = new NumberAnalis();
            EventHandlerNumberBiggerXPrecent eventHandlerNumberBiggerXPrecent = new EventHandlerNumberBiggerXPrecent();
            numberAnalis.numberAnalis += eventHandlerNumberBiggerXPrecent.Message;
            numberAnalis.StartAnalis();
        }
    }
}