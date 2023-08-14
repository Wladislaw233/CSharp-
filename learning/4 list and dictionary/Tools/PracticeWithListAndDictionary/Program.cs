using System;
using System.Diagnostics;
using Models;
using Services;

namespace PracticeWithListAndDictionary
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            TestDataGenerator services = new TestDataGenerator();

            List<Client> listBankClients = TestDataGenerator.GenerateListWitchThousandBankClients();
            Dictionary<string, Client> dictionaryBankClients = TestDataGenerator.GenerateDictionaryWithBankClients(listBankClients);
            List<Employee> listBankEmployees = TestDataGenerator.GenerateListWithThousandEmployees();
            
            string phoneNumber = listBankClients[501].PhoneNumber;
            Stopwatch stopWatch = new Stopwatch(); 
            
            stopWatch.Start();
            Client foundClientInList = listBankClients.Find(bankClient => bankClient.PhoneNumber == phoneNumber);
            stopWatch.Stop();
            Console.WriteLine($"Время на поиск клиента ({foundClientInList.FirstName} {foundClientInList.LastName}) в списке по номеру телефона - {stopWatch.Elapsed.Milliseconds} мс.");
            
            stopWatch.Start();
            Client foundClientInDictionary;
            bool clientFound = dictionaryBankClients.TryGetValue(phoneNumber, out foundClientInDictionary);
            stopWatch.Stop();
            Console.WriteLine($"Время на поиск клиента (" + (clientFound == true ? foundClientInDictionary.FirstName + " " + foundClientInDictionary.LastName : "Клиент не найден") + ") в словаре по номеру телефона - {stopWatch.Elapsed.Milliseconds} мс.");
            
            
            Console.ReadLine();
        }
    }
}