using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class NumberAnalis
    {
        private int Number = 0;
        private double PrecentLimit = 0;
        private List<int> NumberThreed = new List<int>();
        
        public delegate void EventHandlerNumberAnalis(int Number, double Precent, int InputNumber, double PrecentLimit);
        public event EventHandlerNumberAnalis numberAnalis;
        
        private void Analis(int InputNumber)
        {
            double Precent = (InputNumber / (Number <= 0 ? 1 : Number)) * 100 - 100;
            if (Precent > PrecentLimit && numberAnalis != null) numberAnalis(Number, Precent, InputNumber, PrecentLimit);
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
            int InputNumber = int.Parse(Console.ReadLine());
            NumberThreed.Add(InputNumber);
            Console.Clear();
            Analis(InputNumber);
            ContinueAnalis();
        }
    }
}