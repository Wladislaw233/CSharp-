using System;

namespace Events
{
    public class NumberAnalis
    {
        public delegate void EventHandlerNumberAnalis();
        public event EventHandlerNumberAnalis numberAnalis;

        public void Analis(int number, int percent)
        {
            Random random = new Random();

            while (true)
            {
                int newNumber = random.Next(number, number * 2);
                
                if ((newNumber/number) * 100 - 100 > percent)
                {
                    if (numberAnalis != null) 
                    {numberAnalis(); break;}
                    else break;
                }
            }
        }

    }
}