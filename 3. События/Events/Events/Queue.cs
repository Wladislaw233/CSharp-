using System;
using System.Collections.Generic;
using System.Linq;

namespace Events
{
    public class Queue
    {
        public delegate void MethodContainerEventHandlerQuantityArrayElementsBiggerN(int n);
        public delegate void MethodContainerEventHandlerEmptyArray();

        public int QuantityInQueue = 0;
        public int NumberLimit;
        public event MethodContainerEventHandlerQuantityArrayElementsBiggerN QuantityInQueueBiggerN;
        public event MethodContainerEventHandlerEmptyArray EmptyQueue;

        public void Plus(int quantityPlus)
        {
            QuantityInQueue += quantityPlus;
            EventHandler();
        }

        public void Minus(int quantityMinus)
        {
            QuantityInQueue = QuantityInQueue < quantityMinus  ? QuantityInQueue = 0 : QuantityInQueue -= quantityMinus;
            EventHandler();
        }
        public void EventHandler()
        {
            Console.Clear();
            if (QuantityInQueue == 0)
            {
                if (EmptyQueue != null) EmptyQueue();
            }
            else if (QuantityInQueue > NumberLimit)
            {
                if (QuantityInQueueBiggerN != null) QuantityInQueueBiggerN(NumberLimit);
            }
        }
    }
}