using System;
using System.Collections.Generic;
using System.Linq;

namespace Events
{
    public class Queue
    {
        public delegate void MethodContainerEventHandlerQuantityArrayElementsBiggerN(int n);
        public delegate void MethodContainerEventHandlerEmptyArray();

        public event MethodContainerEventHandlerQuantityArrayElementsBiggerN QuantityBiggerN;
        public event MethodContainerEventHandlerEmptyArray EmptyArray;
        
        public void Count(int n)
        {
            List<int> list = new List<int>();
            int i = 0;
            while (true)
            {
                list.Add(i);
                if (list.Count > n)
                {
                    if (QuantityBiggerN != null)
                    {
                        QuantityBiggerN(n);
                        list.Clear();
                        break;
                    }
                }

                i += 1;
            }
            if (list.Count() == 0)
            {
                if (EmptyArray != null)
                {
                    EmptyArray();
                }
            }
            
        }
    }
}