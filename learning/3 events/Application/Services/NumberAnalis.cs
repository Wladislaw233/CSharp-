namespace Services;

public class NumberAnalis
{
    private int Number;
    private double PrecentLimit;
    private readonly List<int> NumberThreed = new();

    public delegate void EventHandlerNumberAnalis(int number, double precent, int inputNumber, double precentLimit);

    public event EventHandlerNumberAnalis NumberBiggerXPrecent;

    private void Analis(int inputNumber)
    {
        double Precent = inputNumber / (Number <= 0 ? 1 : Number) * 100 - 100;
        if (Precent > PrecentLimit && NumberBiggerXPrecent != null)
            NumberBiggerXPrecent(Number, Precent, inputNumber, PrecentLimit);
    }

    public void StartAnalis()
    {
        Console.WriteLine("Введите число лимита:");
        Number = int.Parse(Console.ReadLine());
        Console.WriteLine("Введите процент лимита:");
        PrecentLimit = double.Parse(Console.ReadLine());
        Console.Clear();
        ContinueAnalis();
    }

    private void ContinueAnalis()
    {
        Console.WriteLine($"Число лимита - {Number}, процент лимита - {PrecentLimit}");
        if (NumberThreed.Count > 0) Console.WriteLine(string.Join(",", NumberThreed.ToArray()));
        Console.WriteLine("Введите новое число потока:");
        var inputNumber = int.Parse(Console.ReadLine());
        NumberThreed.Add(inputNumber);
        Console.Clear();
        Analis(inputNumber);
        ContinueAnalis();
    }
}