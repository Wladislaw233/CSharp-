
using System.Reflection;

namespace Models;

public class ReflectionTests
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
            Console.WriteLine("Не удалось создать экземпляр класса triangle при помощи рефлексии.");

    }

    private static Triangle? CreateObjectByStringName()
    {
        Console.WriteLine("Попытка создать экземпляр класса triangle при помощи рефлексии...");
        
        _triangleType = Type.GetType("Models.Triangle");
        
        if (_triangleType != null)
        {
            var instanceTriangle = Activator.CreateInstance(_triangleType, 2, 3, 2);

            if (instanceTriangle is Triangle)
            {
                Console.WriteLine("был создан экземпляр класса triangle со сторонами 2, 3, 2.");
                return (Triangle)instanceTriangle;
            }
        }
        return null;
    }

    private static void CallMethodWithParameters(Triangle triangle)
    {
        Console.WriteLine("\nВызов метода класса triangle на проверку возможности существования треугольника IsValidTriangle...");
        
        var methodInfo = _triangleType.GetMethod("IsValidTriangle", new[] { typeof(double), typeof(double), typeof(double) });

        if (methodInfo != null)
        {
            var args = new object[] { 4, 6, 5 };
            var result = methodInfo.Invoke(triangle, args);
            if (result != null)
                Console.WriteLine($"Треугольник со сторонами 4, 6, 5 существует? - {(bool)result}");
        }
        else 
            Console.WriteLine("Метод класса не был найден!");
    }

    private static void GetPrivateValues(Triangle triangle)
    {
        Console.WriteLine("\nПопытка получения закрыто свойства _sideA класса triangle и его значение...");

        var fieldInfo = _triangleType.GetField("_sideA", BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo != null)
        {
            var fieldValue = fieldInfo.GetValue(triangle);
            if (fieldValue != null)
                Console.WriteLine($"Значение свойства _sideA - {(double)fieldValue}");
            else
                Console.WriteLine("Не удалось получить значение свойства.");
        }
        else
            Console.WriteLine("Неудалось получить закрытое свойство класса.");
    }
}