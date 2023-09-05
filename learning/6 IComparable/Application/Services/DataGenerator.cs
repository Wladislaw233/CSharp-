using Models;

namespace Services;

public class DataGenerator
{
    public static List<Square> GenerateRandomSquares(int numbersOfSquares = 10)
    {
        var rnd = new Random();
        var squares = new List<Square>();
        for (var index = 0; index < numbersOfSquares; index++)
            squares.Add(new Square(rnd.Next(1, 100), rnd.Next(1, 100)));
        return squares;
    }
}