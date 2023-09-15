using ServiceTests;

namespace PracticeWithFileStream;

internal static class Program
{
    public static void Main(string[] args)
    {
        var exportToolsTests = new ExportToolTests();
        exportToolsTests.ExportCsvServiceTest();
    }
}