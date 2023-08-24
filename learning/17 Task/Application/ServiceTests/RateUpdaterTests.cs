﻿using Microsoft.Extensions.DependencyInjection;
using RateUpdater;

namespace ServiceTests;

public class RateUpdaterTests
{
    public static async Task RateUpdaterTest()
    {
        var serviceCollection = new ServiceCollection()
            .AddSingleton<IRateUpdaterService, RateUpdaterService>()
            .BuildServiceProvider();

        var rateUpdaterService = serviceCollection.GetRequiredService<IRateUpdaterService>();
        Console.WriteLine("Старт фоновой задачи.");
        var updateRatesTask = Task.Run(async () => await rateUpdaterService.UpdateRatesAsync());
        await updateRatesTask;
    }
}