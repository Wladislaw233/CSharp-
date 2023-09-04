namespace Models;

public class SquareComparer : IComparer<Square>
{
    public int Compare(Square? firstSquare, Square? secondSquare)
    {
        if (firstSquare == null || secondSquare == null)
            throw new Exception("Неверно переданы параметры!");
        
        var firstSquareArea = firstSquare.Lenght * firstSquare.Width;
        var secondSquareArea = secondSquare.Lenght * secondSquare.Width;

        if (firstSquareArea > secondSquareArea)
            return 1;
        if (firstSquareArea < secondSquareArea)
            return -1;
        return 0;
    }
}