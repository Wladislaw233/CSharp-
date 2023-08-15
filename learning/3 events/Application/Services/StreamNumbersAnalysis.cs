namespace Services;

public class StreamNumbersAnalysis
{
    private int _numberLimit;
    private double _percentLimit;
    private readonly List<int> _streamOfNumbers = new();

    public delegate void MethodContainerEventHandlerIsGreaterThanByPercentage(int numberLimit, double percent, int inputNumber,
        double percentLimit);

    public event MethodContainerEventHandlerIsGreaterThanByPercentage? EventIsGreaterThanByPercentage;

    private void InputNumberIsGreaterThanByPercentage(int inputNumber)
    {
        var percent = inputNumber / (_numberLimit <= 0 ? 1 : _numberLimit) * 100 - 100;
        if (percent > _percentLimit && EventIsGreaterThanByPercentage != null)
            EventIsGreaterThanByPercentage(_numberLimit, percent, inputNumber, _percentLimit);
    }

    public void StartStreamNumbersAnalysis()
    {
        Console.WriteLine("Введите число лимита:");
        _numberLimit = int.Parse(Console.ReadLine());
        Console.WriteLine("Введите процент лимита:");
        _percentLimit = double.Parse(Console.ReadLine());
        Console.Clear();
        ContinueStreamNumbersAnalysis();
    }

    private void ContinueStreamNumbersAnalysis()
    {
        Console.WriteLine($"Число лимита - {_numberLimit}, процент лимита - {_percentLimit}");
        if (_streamOfNumbers.Count > 0) Console.WriteLine(string.Join(",", _streamOfNumbers.ToArray()));
        Console.WriteLine("Введите новое число потока:");
        var inputNumber = int.Parse(Console.ReadLine());
        _streamOfNumbers.Add(inputNumber);
        Console.Clear();
        InputNumberIsGreaterThanByPercentage(inputNumber);
        if (!(inputNumber <= 0))
            ContinueStreamNumbersAnalysis();
    }
}