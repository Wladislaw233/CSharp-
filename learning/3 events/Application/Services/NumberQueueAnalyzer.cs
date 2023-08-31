namespace Services;

public class NumberQueueAnalyzer
{
    public delegate void DelegateQueueLimitReached(int numberLimit);

    public delegate void DelegateEmptyArray();

    public event DelegateQueueLimitReached? EventQueueLimitReached;
    public event DelegateEmptyArray? EventEmptyQueue;

    private int _numberLimit;
    private ConsoleKey _pressedKey;
    private Queue<int> _queue = new();
    private readonly Random _random = new();

    private void AddInQueue(int quantityPlus)
    {
        for (var index = 0; index < quantityPlus; index++)
            _queue.Enqueue(_random.Next(1, 100));
        OnQueueLimitReached();
    }

    private void RemoveFromQueue(int quantityMinus)
    {
        var limit = quantityMinus > _queue.Count ? _queue.Count : quantityMinus;
        for (var index = 0; index < limit; index++)
            _queue.Dequeue();
        OnQueueLimitReached();
    }

    private void OnQueueLimitReached()
    {
        Console.Clear();
        if (_queue.Count == 0 && EventEmptyQueue != null) 
            EventEmptyQueue();
        else if (_queue.Count > _numberLimit && EventQueueLimitReached != null) 
            EventQueueLimitReached(_numberLimit);
    }

    public void RecursiveQueueInput()
    {
        if (_queue.Count == 0)
            Console.WriteLine($"Количество элементов в очереди - {_queue.Count}" +
                              $"\nОчередь: {string.Join(", ", _queue)} " +
                              $"\nОграничение - {_numberLimit}");

        if (_pressedKey != ConsoleKey.P && _pressedKey != ConsoleKey.M && _pressedKey != ConsoleKey.E)
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
                Console.WriteLine(
                    "Для добавления элементов нажмите 'P', для вычета нажмите 'M'. Для выхода нажмите 'E'");
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
                _pressedKey = default;
                RecursiveQueueInput();
            }
            else if (_pressedKey == ConsoleKey.M)
            {
                Console.WriteLine("Введите вычетаемое количество из очереди:");
                RemoveFromQueue(int.Parse(Console.ReadLine()));
                _pressedKey = default;
                RecursiveQueueInput();
            }
            else if (_pressedKey != ConsoleKey.E)
            {
                _pressedKey = default;
                RecursiveQueueInput();
            }
            else
            {
                Console.Clear();
            }
        }
    }
}