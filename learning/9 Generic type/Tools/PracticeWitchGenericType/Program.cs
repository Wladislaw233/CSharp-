using ServiceTests;

namespace PracticeWitchGenericType;

internal class Program
{
    public static void Main(string[] args)
    {
        // запуск тестов сервисов для клиентов и сотрудников.
        var clientServiceTests = new ClientStorageTests();
        clientServiceTests.AddClientTest();
        clientServiceTests.AddClientAccountTest();
        clientServiceTests.UpdateClientAccountTest();

        var employeeServiceTests = new EmployeeStorageTests();
        employeeServiceTests.AddBankEmployeeTest();

        // запуск тестов IEnumerable для клиентов и сотрудников.
        EnumerableTests.GetClientsByFiltersTest();
        EnumerableTests.GetEmployeesByFiltersTest();

        // запуск тестов с черным листом и бонусами.
        GenericTypeTests.BankServiceTest();
    }
}