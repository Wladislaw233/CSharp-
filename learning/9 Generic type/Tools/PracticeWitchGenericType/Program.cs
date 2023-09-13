using Services;
using Services.Storage;
using ServiceTests;

namespace PracticeWitchGenericType;

internal static class Program
{
    public static void Main(string[] args)
    {
        /*// launching service tests for clients and employees.
        ClientStorageTests.ClientStorageTest();
        EmployeeStorageTests.EmployeeStorageTest();*/

        // running IEnumerable tests for clients and employees.
        EnumerableTests.GetClientsByFiltersTest();
        EnumerableTests.GetEmployeesByFiltersTest();

        // running tests with a black sheet and bonuses.
        GenericTypeTests.BankServiceTest();
    }
}