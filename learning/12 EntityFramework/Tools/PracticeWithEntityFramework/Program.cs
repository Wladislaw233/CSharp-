using ServiceTests;

namespace PracticeWithEntityFramework;

internal static class Program
{
    public static void Main(string[] args)
    {
        ClientServiceTests.ClientServiceTest();
        EmployeeServiceTests.EmployeeServiceTest();
    }
}