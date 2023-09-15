using BankingSystemServices.Database;
using ServiceTests;

namespace PracticeWithTask;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        // task 16
        var updateRatesTask = RateUpdaterServiceTests.RateUpdaterTest();

        await Task.Delay(1700);

        await CashDispenserServiceTests.CashDispenserServiceTest();

        // task 17 a.

        await ThreadPoolTests.StartThreadPoolTests();

        // task 17 b.

        await using (var bankingSystemDbContext = new BankingSystemDbContext())
        {
            var clientServiceTests = new ClientServiceTests(bankingSystemDbContext);
            await clientServiceTests.ClientServiceTest();

            var employeeServiceTests = new EmployeeServiceTests(bankingSystemDbContext);
            await employeeServiceTests.EmployeeServiceTest();
        }

        await updateRatesTask;
    }
}