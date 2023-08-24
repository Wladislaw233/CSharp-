namespace ServiceTests;

public class ThreadPoolTests
{
    public static async Task ThreadPoolTest()
    {
        ThreadPool.SetMaxThreads(10,10);

        var tasks = new Task[15];
        
        for (var index = 0; index < 15; index++)
        {
            tasks[index] = RunTask(index);
            await Task.Delay(1000);
        }
        
        await Task.WhenAll(tasks);

        Console.WriteLine("Все задачи выполнены");
    }

    private static async Task RunTask(int taskId)
    {
        Console.WriteLine($"Задача {taskId} запущена.");

        // Задержка внутри задачи
        await Task.Delay(2000);
        
        Console.WriteLine($"Задача {taskId} завершена.");
        
        ThreadPool.GetAvailableThreads(out var availableThreads, out var availableIOThreads);
        Console.WriteLine($"Доступное количество потоков в пуле: {availableThreads}");
    }
}