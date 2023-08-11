using System;

namespace Services
{
    public class EventHandlerQuantityArrayElementsBiggerN
    {
        public void Message(int NumberLimit)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Количество элементов списка превышает {0}", NumberLimit);
            Console.ResetColor();
        }
    }
}