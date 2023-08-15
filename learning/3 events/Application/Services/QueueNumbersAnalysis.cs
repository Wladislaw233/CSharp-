namespace Services;

public class QueueNumbersAnalysis
{
    public delegate void MethodContainerEventHandlerQueueLimitReached(int numberLimit);
    public delegate void MethodContainerEventHandlerEmptyArray();

    public event MethodContainerEventHandlerQueueLimitReached? EventQueueLimitReached;
    public event MethodContainerEventHandlerEmptyArray? EventEmptyQueue;
    
    private int _quantityInQueue;
    private int _numberLimit;
    private ConsoleKey _pressedKey;

    private void AddInQueue(int quantityPlus)
    {
        _quantityInQueue += quantityPlus;
        OnQueueLimitReached();
    }

    private void RemoveFromQueue(int quantityMinus)
    {
        _quantityInQueue = _quantityInQueue < quantityMinus ? _quantityInQueue = 0 : _quantityInQueue -= quantityMinus;
        OnQueueLimitReached();
    }

    private void OnQueueLimitReached()
    {
        Console.Clear();
        if (_quantityInQueue == 0 && EventEmptyQueue != null) EventEmptyQueue();
        else if (_quantityInQueue > _numberLimit && EventQueueLimitReached != null) EventQueueLimitReached(_numberLimit);
    }
    
    public void RecursiveQueueInput()
    {
        Console.WriteLine($"Количество элементов в очереди - {_quantityInQueue}" +
                          (_numberLimit > 0 ? ", ограничение - " + _numberLimit : ""));
        
        if ((_pressedKey != ConsoleKey.P && _pressedKey != ConsoleKey.M && _pressedKey != ConsoleKey.E))
        {
            if (_numberLimit == 0)
            {
                Console.WriteLine("Введите ограничение количества элементов в очереди:");
                _numberLimit = int.Parse(Console.ReadLine());
                Console.Clear();
                RecursiveQueueInput();
            }
            else
            {
                Console.WriteLine("Для добавления элементов нажмите 'P', для вычета нажмите 'M'. Для выхода нажмите 'E'");
                _pressedKey = Console.ReadKey(true).Key;
                Console.Clear();
                RecursiveQueueInput();
            }
        }
        else
        {
            if (_pressedKey == ConsoleKey.P)
            {
                Console.WriteLine("Введите добавляемое количество в очередь:");
                AddInQueue(int.Parse(Console.ReadLine()));
                RecursiveQueueInput();
            }
            else if (_pressedKey == ConsoleKey.M)
            {
                Console.WriteLine("Введите вычетаемое количество из очереди:");
                RemoveFromQueue(int.Parse(Console.ReadLine()));
                RecursiveQueueInput();
            }
            else if (_pressedKey != ConsoleKey.E)
            {
                RecursiveQueueInput();
            }
            else
            {
                Console.Clear();
            }
        }
    }
}