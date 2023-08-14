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
            // пункт а.
            var person1 = new Person { FirstName = "Ivan", LastName = "Ivanov", DateOfBirth = DateTime.Now };
            var person2 = new Person { FirstName = "Stas", LastName = "Petrov", DateOfBirth = DateTime.Now };
            var person3 = new Person { FirstName = "Oleg", LastName = "Zubrov", DateOfBirth = DateTime.Now };

            var employees = new List<Employee>();
            employees.Add(new Employee
                { Person = person1, Owner = true, Contract = person1.FirstName + " " + person1.LastName });
            employees.Add(new Employee
                { Person = person2, Owner = false, Contract = person2.FirstName + " " + person2.LastName });
            employees.Add(new Employee
                { Person = person3, Owner = true, Contract = person3.FirstName + " " + person3.LastName });

            List<Employee> owners = (from employee in employees
                where employee.Owner
                select employee).ToList();

            var bankService = new BankService();

            Console.WriteLine("Введите прибыль банка:");
            var profit = double.Parse(Console.ReadLine());
            
            Console.WriteLine("Введите расход банка:");
            var expenses = double.Parse(Console.ReadLine());

            bankService.CalculateSalaryOfBankOwners(profit, expenses, owners);
            foreach (var employee in owners)
                Console.WriteLine(
                    $"У владельца {employee.Person.FirstName} {employee.Person.LastName} зарплата: {employee.Salary} $");
            // пункт б.
            var client = new Client
            {
                FirstName = "Petya", LastName = "Petrov", Address = "Tiraspol, str. Lenina, 12/2",
                Email = "test@mail.ru", PhoneNumber = "123456789"
            };
            var convertedEmployee = (Employee)client;

            Console.WriteLine(
                $"Клиент банка: " +
                $"\n\t {client.FirstName} {client.LastName}, " +
                $"\n\t e-mail: {client.Email}," +
                $"\n\t адрес: {client.Address}," +
                $"\n\t номер телефона: {client.PhoneNumber} " +
                $"\n Сотрудник банка полученный путем преобразования клиента: " +
                $"\n\t {convertedEmployee.Person.FirstName} {convertedEmployee.Person.LastName}, " +
                $"\n\t e-mail: {convertedEmployee.Email}, " +
                $"\n\t адрес: {convertedEmployee.Address}, " +
                $"\n\t номер телефона: {convertedEmployee.PhoneNumber}");
        }
    }
}