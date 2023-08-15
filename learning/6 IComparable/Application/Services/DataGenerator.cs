using Models;

namespace Services;

public class DataGenerator
{
    private static Random _random = new Random();
    public static List<Square> GenerateRandomSquares(int numbersOfSquares = 10)
    {
        var squares = new List<Square>(); 
        for (var index = 0; index < numbersOfSquares; index++)
            squares.Add(new Square(_random.Next(1,100), _random.Next(1,100)));
        return squares;
    }
}