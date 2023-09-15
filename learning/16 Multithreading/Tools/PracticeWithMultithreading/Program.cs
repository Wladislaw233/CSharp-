
using ServiceTests;

namespace PracticeWithMultithreading;

internal static class Program
{
    public static void Main(string[] args)
    {
        var threadAndTaskTests = new ThreadAndTaskTests();
        threadAndTaskTests.ThreadTest();
    }
}