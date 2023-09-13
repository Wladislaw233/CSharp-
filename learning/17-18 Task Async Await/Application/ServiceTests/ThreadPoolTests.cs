namespace ServiceTests;

public static class ThreadPoolTests
{
    public static void StartThreadPoolTests()
    {
        //ThreadPool.SetMaxThreads(10, 10); - doesn't work, changed it to semaphoreSlim.
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Sequential and asynchronous execution of tasks. (17.а)");
        Console.ResetColor();
        
        Console.WriteLine("launching 15 tasks, with 10 threads available (sequential execution).");
        ThreadPoolTest();
        Console.WriteLine("launching 15 tasks with 10 threads available (asynchronous execution).");
        ThreadPoolAsyncTest().GetAwaiter().GetResult();
    }
    
    private static void ThreadPoolTest()
    {
        var semaphoreSlim = new SemaphoreSlim(10);
        
        for (var index = 1; index <= 15; index++)
        {
            semaphoreSlim.Wait();
            var indexOfTask = index;
            Task.Run(() =>
            {
                Console.WriteLine($"The {indexOfTask} task starts. Available streams: {semaphoreSlim.CurrentCount}/10.");
                Thread.Sleep(2000);
                semaphoreSlim.Release();
                Console.WriteLine($"The task {indexOfTask} is finished. Available streams: {semaphoreSlim.CurrentCount}/10.");
            }).Wait();
            Thread.Sleep(100);
        }
        
        // We are waiting for all tasks to be completed.
        Thread.Sleep(5000);
    }
    
    private static async Task ThreadPoolAsyncTest()
    {
        var semaphoreSlim = new SemaphoreSlim(10);
        
        var tasks = new List<Task>();
        
        for (var index = 1; index <= 15; index++)
        {
            await semaphoreSlim.WaitAsync();
            var indexOfTask = index;
            tasks.Add(Task.Run(async () =>
            {
                Console.WriteLine($"The {indexOfTask} task starts. Available streams: {semaphoreSlim.CurrentCount}/10.");
                await Task.Delay(2000);
                semaphoreSlim.Release();
                Console.WriteLine($"The task {indexOfTask} is finished. Available streams: {semaphoreSlim.CurrentCount}/10.");
            }));
            await Task.Delay(100);
        }
        
        // We are waiting for all tasks to be completed.
        await Task.WhenAll(tasks);
    }
    
}