using Models;

namespace Services;

public class DataGenerator
{
    private static readonly Random Rnd = new();

    public static List<Square> GenerateRandomSquares(int numbersOfSquares = 10)
    {
        var squares = new List<Square>();
        for (var index = 0; index < numbersOfSquares; index++)
            squares.Add(new Square(Rnd.Next(1, 100), Rnd.Next(1, 100)));
        return squares;
    }
}