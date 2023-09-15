using BankingSystemServices.Database;
using ServiceTests;

namespace PracticeWithEntityFramework;

internal static class Program
{
    public static void Main(string[] args)
    {
        using var bankingSystemDbContext = new BankingSystemDbContext();

        var clientServiceTests = new ClientServiceTests(bankingSystemDbContext);
        clientServiceTests.ClientServiceTest();

        var employeeServiceTests = new EmployeeServiceTests(bankingSystemDbContext);
        employeeServiceTests.EmployeeServiceTest();
    }
}