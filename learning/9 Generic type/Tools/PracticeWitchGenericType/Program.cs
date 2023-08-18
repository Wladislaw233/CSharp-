namespace PracticeWitchGenericType;
using ServiceTests;

internal class Program
{
    public static void Main(string[] args)
    {
        // запуск тестов сервисов для клиентов и сотрудников.
        var clientServiceTests = new ClientServiceTests();
        clientServiceTests.AddClientTest();
        clientServiceTests.AddClientAccountTest();
        clientServiceTests.UpdateClientAccountTest();

        var employeeServiceTests = new EmployeeServiceTests();
        employeeServiceTests.AddBankEmployeeTest();
        
        // запуск тестов IEnumerable для клиентов и сотрудников.
        EnumerableTests.GetClientsByFiltersTest();
        EnumerableTests.GetEmployeesByFiltersTest();
    }
}