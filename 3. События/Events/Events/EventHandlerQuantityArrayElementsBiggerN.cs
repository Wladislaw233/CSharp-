using System;

namespace Events
{
    public class EventHandlerQuantityArrayElementsBiggerN
    {
        public void Message(int n)
        {
            Console.WriteLine("Количество элементов списка превышает {0}", n);
        }
    }
}