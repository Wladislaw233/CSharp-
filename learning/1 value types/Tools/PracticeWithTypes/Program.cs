﻿using Models;

namespace PracticeWithTypes
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Person person = new Person { FirstName = "Ivan", LastName = "Ivanov", Age = 25};
            Employee employee = new Employee { Person = person};
            UpdateEmployeeContarct(employee);
            Console.WriteLine("{0}", employee.Contract);
            Currency currency = new Currency();
            ChangeCurrency(ref currency);
            Console.WriteLine("Валюта: {0} {1}", currency.Name, currency.Course);
            Console.ReadLine();
        }

        static void ChangeCurrency(ref Currency currency)
        {
            currency.Name = "RUB";
            currency.Course = 16.35;
        }
        
        static void UpdateEmployeeContarct(Employee employee)
        {
            employee.Contract = "Контракт сотрудника: " 
                                + employee.Person.FirstName + " " 
                                + employee.Person.LastName + ", возврат: " + employee.Person.Age;
        }
    }    
}