using System;
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
            List<Employee> listBankEmployers = TestDataGenerator.GenerateListWithThousandEmployers();
            Console.ReadLine();
        }
    }
}