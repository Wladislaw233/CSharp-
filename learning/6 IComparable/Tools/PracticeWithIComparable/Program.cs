using System;

namespace PracticeWithIComparable
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var squares = Services.DataGenerator.GenerateRandomSquares(5);
            Console.WriteLine("До сортировки:");
            Console.WriteLine(string.Join('\n', squares.Select(square => $"Длина - {square.Lenght}, высота - {square.Width}, площадь - {square.Lenght * square.Width}")));
            squares.Sort();
            Console.WriteLine("После сортировки:");
            Console.WriteLine(string.Join('\n', squares.Select(square => $"Длина - {square.Lenght}, высота - {square.Width}, площадь - {square.Lenght * square.Width}")));
            
        }
    }
}