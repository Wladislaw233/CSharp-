using ServiceTests;

namespace PracticeWithException;

internal class Program
{
    public static void Main(string[] args)
    {
        var clientServiceTests = new ClientServiceTests();
        clientServiceTests.AddClientTest();
        clientServiceTests.AddClientAccountTest();
        clientServiceTests.UpdateClientAccountTest();
        
        var employeeServiceTests = new EmployeeServiceTests();
        employeeServiceTests.AddBankEmployeeTest();
    }
}