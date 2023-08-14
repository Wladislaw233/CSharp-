namespace Services;

public class Queue
{
    public delegate void MethodContainerEventHandlerQuantityArrayElementsBiggerN(int numberLimit);

    public delegate void MethodContainerEventHandlerEmptyArray();

    private int QuantityInQueue;
    private int NumberLimit;
    public event MethodContainerEventHandlerQuantityArrayElementsBiggerN QuantityInQueueBiggerN;
    public event MethodContainerEventHandlerEmptyArray EmptyQueue;

    public void QueueControl(object packageKey = null)
    {
        Console.WriteLine($"Количество элементов в очереди - {QuantityInQueue}" +
                          (NumberLimit > 0 ? ", ограничение - " + NumberLimit : ""));

        if (packageKey == null || ((ConsoleKey)packageKey != ConsoleKey.P && (ConsoleKey)packageKey != ConsoleKey.M &&
                                   (ConsoleKey)packageKey != ConsoleKey.E))
        {
            if (NumberLimit == 0)
            {
                Console.WriteLine("Введите ограничение количества элементов в очереди:");
                NumberLimit = int.Parse(Console.ReadLine());
                Console.Clear();
                QueueControl();
            }
            else
            {
                Console.WriteLine(
                    "Для добавления элементов нажмите 'P', для вычета нажмите 'M'. Для выхода нажмите 'E'");
                var key = Console.ReadKey(true);
                Console.Clear();
                QueueControl(key.Key);
            }
        }
        else
        {
            var key = (ConsoleKey)packageKey;
            if (key == ConsoleKey.P)
            {
                Console.WriteLine("Введите добавляемое количество в очередь:");
                Plus(int.Parse(Console.ReadLine()));
                QueueControl();
            }
            else if (key == ConsoleKey.M)
            {
                Console.WriteLine("Введите вычетаемое количество из очереди:");
                Minus(int.Parse(Console.ReadLine()));
                QueueControl();
            }
            else if (key != ConsoleKey.E)
            {
                QueueControl();
            }
            else
            {
                Console.Clear();
            }
        }
    }

    private void Plus(int quantityPlus)
    {
        QuantityInQueue += quantityPlus;
        EventHandler();
    }

    private void Minus(int quantityMinus)
    {
        QuantityInQueue = QuantityInQueue < quantityMinus ? QuantityInQueue = 0 : QuantityInQueue -= quantityMinus;
        EventHandler();
    }

    private void EventHandler()
    {
        Console.Clear();
        if (QuantityInQueue == 0 && EmptyQueue != null) EmptyQueue();
        else if (QuantityInQueue > NumberLimit && QuantityInQueueBiggerN != null) QuantityInQueueBiggerN(NumberLimit);
    }
}