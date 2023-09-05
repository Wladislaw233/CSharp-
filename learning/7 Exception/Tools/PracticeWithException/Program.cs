using ServiceTests;

namespace PracticeWithException;

internal class Program
{
    public static void Main(string[] args)
    {
        // клиенты.
        var clientServiceTests = new ClientServiceTests();
        clientServiceTests.ClientServiceTest();
        
        // сотрудники.
        EmployeeServiceTests.EmployeeServiceTest();
    }
}