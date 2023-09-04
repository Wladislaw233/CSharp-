namespace ServiceTests;

public class ThreadPoolTests
{

    public static void StartThreadPoolTests()
    {
        //ThreadPool.SetMaxThreads(10, 10); - не работает, переделал на semaphoreSlim.
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Последовательное и асинхронное выполнение задач. (17.а)");
        Console.ResetColor();
        
        Console.WriteLine("запуск 15 задач, при доступных 10 потоках (последовательное выполнение).");
        ThreadPoolTest();
        Console.WriteLine("запуск 15 задач, при доступных 10 потоках (асинхронное выполнение).");
        ThreadPoolAsyncTest().Wait();
    }
    
    private static void ThreadPoolTest()
    {
        var semaphoreSlim = new SemaphoreSlim(10);
        
        for (var index = 1; index <= 15; index++)
        {
            semaphoreSlim.Wait();
            Task.Run(() =>
            {
                Console.WriteLine($"Задача {index} стартует. Доступные потоки: {semaphoreSlim.CurrentCount}/10.");
                Thread.Sleep(2000);
                semaphoreSlim.Release();
                Console.WriteLine($"Задача {index} окончена. Доступные потоки: {semaphoreSlim.CurrentCount}/10.");
            }).Wait();
            Thread.Sleep(100);
        }
        
        // дожидаемся выполнения всех задач.
        Thread.Sleep(5000);
    }
    
    private static async Task ThreadPoolAsyncTest()
    {
        var semaphoreSlim = new SemaphoreSlim(10);
        
        var tasks = new List<Task>();
        
        for (var index = 1; index <= 15; index++)
        {
            await semaphoreSlim.WaitAsync();
            tasks.Add(Task.Run(async () =>
            {
                var numberOfTask = index;
                Console.WriteLine($"Задача {numberOfTask} стартует. Доступные потоки: {semaphoreSlim.CurrentCount}/10.");
                await Task.Delay(2000);
                semaphoreSlim.Release();
                Console.WriteLine($"Задача {numberOfTask} окончена. Доступные потоки: {semaphoreSlim.CurrentCount}/10.");
            }));
            await Task.Delay(100);
        }
        
        // дожидаемся выполнения всех задач.
        await Task.WhenAll(tasks);
    }
    
}