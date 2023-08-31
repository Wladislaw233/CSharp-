using ServiceTests;

namespace PracticeWithException;

internal class Program
{
    public static void Main(string[] args)
    {
        var clientServiceTests = new ClientServiceTests();
        clientServiceTests.ClientServiceTest();
        
        var employeeServiceTests = new EmployeeServiceTests();
        employeeServiceTests.EmployeeServiceTest();
    }
}