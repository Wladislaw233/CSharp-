namespace Models;

public class Triangle
{
    private readonly double _sideA;

    private readonly double _sideB;

    private readonly double _sideC;

    public Triangle(double sideA, double sideB, double sideC)
    {
        if (IsValidTriangle(sideA, sideB, sideC))
        {
            _sideA = sideA;
            _sideB = sideB;
            _sideC = sideC;
        }
        else
            throw new ArgumentException("Неверно заданы праметры треугольника!");
    }
    
    public static bool IsValidTriangle(double sideA, double sideB, double sideC)
    {
        return sideA + sideB > sideC && sideA + sideC > sideB && sideB + sideC > sideA;
    }
    
    public double CalculateArea()
    {
        var halfSides = (_sideA + _sideB + _sideC) / 2;
        return Math.Sqrt(halfSides * (halfSides - _sideA) * (halfSides - _sideB) * (halfSides - _sideC));
    }
}
