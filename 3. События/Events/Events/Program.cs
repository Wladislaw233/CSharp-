using System;

namespace Events
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            /* реализовать очередь, которая генерирует событие, когда кол-во
            объектов в ней превышает n и событие, когда становится пустой*/
            
            Queue queue = new Queue();
            EventHandlerEmptyArray eventHandlerEmptyArray = new EventHandlerEmptyArray();
            EventHandlerQuantityArrayElementsBiggerN eventHandlerQuantityArrayElementsBiggerN =
                new EventHandlerQuantityArrayElementsBiggerN();
            queue.EmptyArray += eventHandlerEmptyArray.Message;
            queue.QuantityBiggerN += eventHandlerQuantityArrayElementsBiggerN.Message;
            
            queue.Count(45);
            
            /*реализовать класс анализирующий поток чисел, и если число
            отличается более чем x - процентов выбрасывает событие*/
            
            NumberAnalis numberAnalis = new NumberAnalis();
            int number = 37;
            int percent = 45; 
            numberAnalis.Analis(number, percent);
        }
    }
}