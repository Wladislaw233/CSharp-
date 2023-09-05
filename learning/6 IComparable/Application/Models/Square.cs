namespace Models;

public class Square : IComparable<Square>
{
    public int Lenght { get; }
    public int Width { get; }

    public Square(int lenght, int width)
    {
        Lenght = lenght;
        Width = width;
    }
    public int CompareTo(Square? square)
    {
        if (square is null) 
            throw new ArgumentException("Некорректное значение параметра");
        
        var thisSquareArea = Lenght * Width;
        var otherSquareArea = square.Lenght * square.Width;
        
        return otherSquareArea.CompareTo(thisSquareArea);
    }
}