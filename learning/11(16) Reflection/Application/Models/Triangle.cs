namespace Models;

// ReSharper disable once ClassNeverInstantiated.Global
public class Triangle
{
    // ReSharper disable once NotAccessedField.Local
    private readonly double _sideA;

    // ReSharper disable once NotAccessedField.Local
    private readonly double _sideB;

    // ReSharper disable once NotAccessedField.Local
    private readonly double _sideC;

    // ReSharper disable once PublicConstructorInAbstractClass
    public Triangle(double sideA, double sideB, double sideC)
    {
        if (IsValidTriangle(sideA, sideB, sideC))
        {
            _sideA = sideA;
            _sideB = sideB;
            _sideC = sideC;
        }
        else
            throw new ArgumentException("The triangle parameters are set incorrectly!");
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static bool IsValidTriangle(double sideA, double sideB, double sideC)
    {
        return sideA + sideB > sideC && sideA + sideC > sideB && sideB + sideC > sideA;
    }
}
