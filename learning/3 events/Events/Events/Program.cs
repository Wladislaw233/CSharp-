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
            queue.EmptyQueue += eventHandlerEmptyArray.Message;
            queue.QuantityInQueueBiggerN += eventHandlerQuantityArrayElementsBiggerN.Message;
            
            QueueControl(queue);
            
            /*реализовать класс анализирующий поток чисел, и если число
            отличается более чем x - процентов выбрасывает событие*/
            
            NumberAnalis numberAnalis = new NumberAnalis();
            int number = 37;
            int percent = 45; 
            numberAnalis.Analis(number, percent);
            
        }

        async public static void QueueControl(Queue queue,object packageKey = null)
        {
            
            Console.WriteLine($"Количество элементов в очереди - {queue.QuantityInQueue}" + (queue.NumberLimit > 0 ? ", ограничение - " + queue.NumberLimit : ""));
            
            if (packageKey == null || ((ConsoleKey)packageKey != ConsoleKey.P && (ConsoleKey)packageKey != ConsoleKey.M && (ConsoleKey)packageKey != ConsoleKey.E))
            {
                if (queue.NumberLimit == 0)
                {
                    Console.WriteLine("Введите ограничение количества элементов в очереди:");
                    queue.NumberLimit = int.Parse(Console.ReadLine());
                    Console.Clear();
                    QueueControl(queue);
                }
                else
                {
                    Console.WriteLine("Для добавления элементов нажмите 'P', для вычета нажмите 'M'. Для выхода нажмите 'E'");
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    Console.Clear();
                    QueueControl(queue, key.Key);
                }
            }
            else
            {
                ConsoleKey key = (ConsoleKey)packageKey;
                if (key == ConsoleKey.P)
                {
                    Console.WriteLine("Введите добавляемое количество в очередь:");
                    queue.Plus(int.Parse(Console.ReadLine()));
                    QueueControl(queue);
                }
                else if (key == ConsoleKey.M)
                {
                    Console.WriteLine("Введите вычетаемое количество из очереди:");
                    queue.Minus(int.Parse(Console.ReadLine()));
                    QueueControl(queue);
                }
                else if (key != ConsoleKey.E)
                {
                    QueueControl(queue);
                }
                else
                {
                    Console.Clear();
                }
            }
        }
    }
}