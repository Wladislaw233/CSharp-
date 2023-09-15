using ServiceTests;

namespace PracticeWithSerialization;

internal static class Program
{
    public static void Main(string[] args)
    {
        var exportToolTests = new ExportToolTests();
        exportToolTests.ExportJsonServiceTest();
    }
}