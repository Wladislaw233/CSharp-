﻿using Models;
using Services;

namespace PracticeWithIComparable;

internal class Program
{
    public static void Main(string[] args)
    {
        var squares = DataGenerator.GenerateRandomSquares(5);
        
        Console.WriteLine("До сортировки:");
        
        Console.WriteLine(string.Join('\n',
            squares.Select(square =>
                $"Длина - {square.Lenght}, высота - {square.Width}, площадь - {square.Lenght * square.Width}")));

        //squares.Sort();
        squares.Sort(new SquareComparer());
        
        Console.WriteLine("После сортировки:");
        
        Console.WriteLine(string.Join('\n',
            squares.Select(square =>
                $"Длина - {square.Lenght}, высота - {square.Width}, площадь - {square.Lenght * square.Width}")));
    }
}