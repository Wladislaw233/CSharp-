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
            
            var owners = new List<Employee>();

            owners.Add(new Owner("Иван", "Иванов", DateTime.Now, 22, "Иван Иванов, дата рождения: " + DateTime.Now));
            owners.Add(new Owner("Петр", "Петров", DateTime.Now, 45, "Петр Петров, дата рождения: " + DateTime.Now));
            
            var bankService = new BankService();

            Console.WriteLine("Введите прибыль банка:");
            var profit = double.Parse(Console.ReadLine());
            
            Console.WriteLine("Введите расход банка:");
            var expenses = double.Parse(Console.ReadLine());

            bankService.CalculateSalaryOfBankOwners(profit, expenses, owners);
            foreach (var owner in owners)
                Console.WriteLine(
                    $"У владельца {owner.FirstName} {owner.LastName} зарплата: {owner.Salary} $");
            // пункт б.

            var client = new Client("Василий", "Пупкин", DateTime.Now, 34, "+8775532112", "test@mail.ru",
                "Tiraspol, str. Lenina, 12/2");
           
            var convertedEmployee = (Employee)client;

            Console.WriteLine(
                $"Клиент банка: " +
                $"\n\t {client.FirstName} {client.LastName}, " +
                $"\n\t e-mail: {client.Email}," +
                $"\n\t адрес: {client.Address}," +
                $"\n\t номер телефона: {client.PhoneNumber} " +
                $"\n Сотрудник банка полученный путем преобразования клиента: " +
                $"\n\t {convertedEmployee.FirstName} {convertedEmployee.LastName}, " +
                $"\n\t e-mail: {convertedEmployee.Email}, " +
                $"\n\t адрес: {convertedEmployee.Address}, " +
                $"\n\t номер телефона: {convertedEmployee.PhoneNumber}");
        }
    }
}