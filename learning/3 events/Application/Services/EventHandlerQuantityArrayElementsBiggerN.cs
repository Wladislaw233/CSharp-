using System;

namespace Services
{
    public class EventHandlerQuantityArrayElementsBiggerN
    {
        public void Message(int numberLimit)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Количество элементов списка превышает {0}", numberLimit);
            Console.ResetColor();
        }
    }
}