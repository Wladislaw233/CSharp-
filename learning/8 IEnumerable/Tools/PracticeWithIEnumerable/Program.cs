using Services;
using Services.Storage;
using ServiceTests;

namespace PracticeWithIEnumerable;

internal class Program
{
    public static void Main(string[] args)
    {
        EnumerableTests.GetClientsByFiltersTest();
        EnumerableTests.GetEmployeesByFiltersTest();
    }
}