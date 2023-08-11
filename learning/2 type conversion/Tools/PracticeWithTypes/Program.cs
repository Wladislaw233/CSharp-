using System;
using System.Collections.Generic;
using System.Diagnostics;
using Models;
using Services;
using System.Linq;

namespace PracticeWithTypes
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Person person1 = new Person { FirstName = "Ivan", LastName = "Ivanov", DateOfBirth = DateTime.Now };
            Person person2 = new Person { FirstName = "Stas", LastName = "Petrov", DateOfBirth = DateTime.Now };
            Person person3 = new Person { FirstName = "Oleg", LastName = "Zubrov", DateOfBirth = DateTime.Now };
            
            Employee[] employees = new[] { new Employee { Person = person1, Owner = true, Contract = person1.FirstName + " " + person1.LastName}
                ,new Employee { Person = person2, Owner = false, Contract = person2.FirstName + " " + person2.LastName}
                ,new Employee { Person = person3, Owner = true, Contract = person3.FirstName + " " + person3.LastName}};
            
            List<Employee> Owners = (from employee in employees
                where employee.Owner == true
                select employee).ToList();
            
            BankService bankService = new BankService();
            
            Console.WriteLine("Введите прибыль банка:");
            double profit = double.Parse(Console.ReadLine());
            
            Console.WriteLine("Введите расход банка:");
            double experence = double.Parse(Console.ReadLine());
            
            bankService.CalculateSalary(profit, experence, Owners);
            foreach (Employee employee in Owners)
            {
                Console.WriteLine($"У владельца {employee.Person.FirstName} {employee.Person.LastName} зарплата: {employee.Salary} $");
            }
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}