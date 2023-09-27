namespace ServiceTests;

public static class ThreadPoolTests
{
    private const int TotalThreads = 16; // min - 16.
    private const int TotalTask = 15;

    public static async Task StartThreadPoolTests()
    {
        ThreadPool.SetMaxThreads(TotalThreads, TotalThreads);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Sequential and asynchronous execution of tasks. (17.а)");
        Console.ResetColor();

        Console.WriteLine("launching 15 tasks, with 10 threads available (sequential execution).");
        //await ThreadPoolTest();

        Console.WriteLine("launching 15 tasks with 10 threads available (asynchronous execution).");
        await ThreadPoolAsyncTest();
    }

    private static async Task ThreadPoolTest()
    {
        for (var index = 1; index <= TotalTask; index++)
        {
            var indexOfTask = index;
            await Task.Run(async () => await DoWorkAsync(indexOfTask));
            await Task.Delay(100);
        }
    }

    private static async Task ThreadPoolAsyncTest()
    {
        var tasks = new List<Task>();

        for (var index = 1; index <= TotalTask; index++)
        {
            var indexOfTask = index;
            tasks.Add(Task.Run(async () => await DoWorkAsync(indexOfTask)));
            await Task.Delay(100);
        }

        // We are waiting for all tasks to be completed.
        await Task.WhenAll(tasks);
    }

    private static async Task DoWorkAsync(int indexOfTask)
    {
        // ReSharper disable once NotAccessedVariable
        ThreadPool.GetAvailableThreads(out var workerThreads, out var completionPortThreads);
        Console.WriteLine($"The {indexOfTask} task starts. Available streams: {workerThreads}/{TotalThreads}.");
        Thread.Sleep(20000);
        ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
        Console.WriteLine($"The task {indexOfTask} is finished. Available streams: {workerThreads}/{TotalThreads}.");
    }
}