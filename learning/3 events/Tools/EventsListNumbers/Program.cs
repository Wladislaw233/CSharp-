﻿using System;
using Services;

namespace Events
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            /* реализовать очередь, которая генерирует событие, когда кол-во
            объектов в ней превышает n и событие, когда становится пустой */
            
            Queue queue = new Queue();
            EventHandlerEmptyArray eventHandlerEmptyArray = new EventHandlerEmptyArray();
            EventHandlerQuantityArrayElementsBiggerN eventHandlerQuantityArrayElementsBiggerN =
                new EventHandlerQuantityArrayElementsBiggerN();
            queue.EmptyQueue += eventHandlerEmptyArray.Message;
            queue.QuantityInQueueBiggerN += eventHandlerQuantityArrayElementsBiggerN.Message;
            queue.QueueControl();
        }
    }
}