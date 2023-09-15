using System.Reflection;
using BankingSystemServices.Services;
using Models;

namespace ReflectionTests
{

    public class ReflectionTriangleTests
    {
        private Type? _triangleType;
        private Triangle? _triangle;

        public void ReflectionTest()
        {
            try
            {
                _triangle = CreateObjectByStringName();
                if (_triangle != null)
                {
                    CallMethodWithParameters();
                    GetPrivateValues();
                }
                else
                    Console.WriteLine("Failed to create an instance of the triangle class using reflection.");
            }
            catch (Exception e)
            {
                var mess = ExceptionHandlingService.GeneralExceptionHandler(e,"Error when using reflection.");
                Console.WriteLine(mess);
            }
        }

        private Triangle? CreateObjectByStringName()
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

        private void CallMethodWithParameters()
        {
            Console.WriteLine(
                "\nCalling the triangle class method to check the possibility of the existence of a triangle IsValidTriangle...");

            var methodInfo = _triangleType?.GetMethod("IsValidTriangle",
                new[] { typeof(double), typeof(double), typeof(double) });

            if (methodInfo != null)
            {
                var args = new object[] { 4, 6, 5 };

                var result = methodInfo.Invoke(_triangle, args);

                if (result != null)
                    Console.WriteLine($"Does a triangle with sides 4, 6, 5 exist? - {(bool)result}");
            }
            else
                Console.WriteLine("The class method was not found!");
        }

        private void GetPrivateValues()
        {
            Console.WriteLine("\nTrying to get the closed property _sideA of the triangle class and its value...");

            var fieldInfo = _triangleType?.GetField("_sideA", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                var fieldValue = fieldInfo.GetValue(_triangle);
                Console.WriteLine(fieldValue != null
                    ? $"Property value _sideA - {(double)fieldValue}"
                    : "Failed to get property value.");
            }
            else
                Console.WriteLine("Failed to get a private class property.");
        }
    }
}
