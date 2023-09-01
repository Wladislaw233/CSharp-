using Services;
using Services.Storage;
using ServiceTests;

namespace PracticeWitchGenericType;

internal class Program
{
    public static void Main(string[] args)
    {
        // запуск тестов сервисов для клиентов и сотрудников.
        ClientStorageTests.ClientStorageTest();
        EmployeeStorageTests.EmployeeStorageTest();

        // запуск тестов IEnumerable для клиентов и сотрудников.
        EnumerableTests.EnumerableTest();

        // запуск тестов с черным листом и бонусами.
        GenericTypeTests.BankServiceTest();
    }
}