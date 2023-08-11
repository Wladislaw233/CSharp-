using System;

namespace Services
{
    public class EventHandlerNumberBiggerXPrecent
    {
        public void Message(int Number, double Precent, int InputNumber, double PrecentLimit)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{InputNumber} > {Number} на {Precent}%, что больше лимита ({PrecentLimit}%).");
            Console.ResetColor();
        }
    }
}