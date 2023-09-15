using BankingSystemServices.Models;
using BankingSystemServices.Services;


namespace PracticeWithTypes
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // пункт а.
            var testDataGenerator = new TestDataGenerator();
            var bankEmployees = testDataGenerator.GenerateListWithBankEmployees();
            var owners = bankEmployees.FindAll(employee => employee.IsOwner);
            
            Console.WriteLine("Введите прибыль банка:");
            var profit = double.Parse(Console.ReadLine());
            
            Console.WriteLine("Введите расход банка:");
            var expenses = double.Parse(Console.ReadLine());

            BankService.CalculateSalary(profit, expenses, owners);
            foreach (var owner in owners)
                Console.WriteLine(
                    $"У владельца {owner.FirstName} {owner.LastName} зарплата: {owner.Salary} $");
            
            // пункт б.

            var client = testDataGenerator.GenerateRandomBankClient();
           
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