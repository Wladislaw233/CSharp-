using ServiceTests;

namespace PracticeWithEntityFramework;

internal class Program
{
    public static void Main(string[] args)
    {
        ClientServiceTests.ClientServiceTest();
        EmployeeServiceTests.EmployeeServiceTest();
    }
}