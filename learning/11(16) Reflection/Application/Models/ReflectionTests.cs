
using System.Reflection;

namespace Models;

public static class ReflectionTests
{
    private static Type? _triangleType;
    public static void ReflectionTest()
    {
        var triangle = CreateObjectByStringName();
        if (triangle != null)
        {
            CallMethodWithParameters(triangle);
            GetPrivateValues(triangle);
        }
        else
            Console.WriteLine("Failed to create an instance of the triangle class using reflection.");

    }

    private static Triangle? CreateObjectByStringName()
    {
        Console.WriteLine("Trying to create an instance of the triangle class using reflection...");
        
        _triangleType = Type.GetType("Models.Triangle");

        if (_triangleType == null) 
            return null;
        
        var instanceTriangle = Activator.CreateInstance(_triangleType, 2, 3, 2);

        if (instanceTriangle is not Triangle triangle) 
            return null;
        
        Console.WriteLine("an instance of the triangle class was created with sides 2, 3, 2.");
        
        return triangle;
    }

    private static void CallMethodWithParameters(Triangle triangle)
    {
        Console.WriteLine("\nCalling the triangle class method to check the possibility of the existence of a triangle IsValidTriangle...");
        
        var methodInfo = _triangleType?.GetMethod("IsValidTriangle", new[] { typeof(double), typeof(double), typeof(double) });

        if (methodInfo != null)
        {
            var args = new object[] { 4, 6, 5 };
            var result = methodInfo.Invoke(triangle, args);
            if (result != null)
                Console.WriteLine($"Does a triangle with sides 4, 6, 5 exist? - {(bool)result}");
        }
        else 
            Console.WriteLine("The class method was not found!");
    }

    private static void GetPrivateValues(Triangle triangle)
    {
        Console.WriteLine("\nTrying to get the closed property _sideA of the triangle class and its value...");

        var fieldInfo = _triangleType?.GetField("_sideA", BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo != null)
        {
            var fieldValue = fieldInfo.GetValue(triangle);
            Console.WriteLine(fieldValue != null
                ? $"Property value _sideA - {(double)fieldValue}"
                : "Failed to get property value.");
        }
        else
            Console.WriteLine("Failed to get a private class property.");
    }
}