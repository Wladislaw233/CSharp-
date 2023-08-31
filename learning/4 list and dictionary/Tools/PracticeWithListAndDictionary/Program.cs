using System.Diagnostics;
using BankingSystemServices.Services;
using BankingSystemServices.Models;

namespace PracticeWithListAndDictionary;

internal class Program
{
    private static readonly List<Client> ListBankClients = TestDataGenerator.GenerateListWithBankClients(1000);

    private static readonly Dictionary<string, Client> DictionaryBankClients =
        TestDataGenerator.GenerateDictionaryWithBankClients(ListBankClients);

    private static readonly List<Employee> ListBankEmployees = TestDataGenerator.GenerateListWithBankEmployees(1000);

    private static readonly Stopwatch Stopwatch = new();

    public static void Main(string[] args)
    {
        // номер телефона для осуществления тестов.
        var randomNumber = TestDataGenerator.RandomNumber(0, ListBankClients.Count - 1);
        var phoneNumber = ListBankClients.Skip(randomNumber).Last().PhoneNumber;
        
        // добавление последнего клиента в словарь для осуществления тестов.
        var lastClientInDictionary = TestDataGenerator.GenerateRandomBankClient();
        DictionaryBankClients.Add(lastClientInDictionary.PhoneNumber, lastClientInDictionary);
        
        // 2.а замер времени выполнения поиска клиента по его номеру телефона среди элементов списка.
        SearchClientInListByPhoneNumber(phoneNumber);

        // 2.б замер времени выполнения поиска клиента по его номеру телефона, среди элементов словаря.
        SearchClientInDictionaryByPhoneNumber(phoneNumber);

        // 2.в выборку клиентов, возраст которых ниже 42.
        PrintTheNumberOfClientsUnder42();

        // 2.г поиск сотрудника с минимальной заработной платой.
        SearchEmployeeInListWithMinimumSalary();

        // сравнить скорость поиска по словарю двумя методами
        // 2.д.1 при помощи метода FirstOrDefault(ищем последний элемент коллекции)
        PrintTimeToFindLastClientUsingFirstOrDefault(lastClientInDictionary.PhoneNumber);

        // 2.д.2 при помощи выборки по ключу последнего элемента коллекции.
        PrintTimeToFindLastClientUsingTryGetValue(lastClientInDictionary.PhoneNumber);
    }

    private static void SearchClientInListByPhoneNumber(string phoneNumber)
    {
        Stopwatch.Start();
        var foundClientInListByPhone = ListBankClients.Find(bankClient => bankClient.PhoneNumber == phoneNumber);
        Stopwatch.Stop();
        if (foundClientInListByPhone != null)
            Console.WriteLine(
                $"Время на поиск клиента ({foundClientInListByPhone.FirstName} {foundClientInListByPhone.LastName}) " +
                $"в списке по номеру телефона - {Stopwatch.Elapsed.TotalMilliseconds} мс.");
        else
            Console.WriteLine("Клиент не найден по номеру телефона в списке.");
    }

    private static void SearchClientInDictionaryByPhoneNumber(string phoneNumber)
    {
        Stopwatch.Reset();
        Stopwatch.Start();
        var clientFound = DictionaryBankClients.TryGetValue(phoneNumber, out var foundClientInDictionaryByPhone);
        Stopwatch.Stop();
        if (clientFound && foundClientInDictionaryByPhone != null)
            Console.WriteLine(
                $"Время на поиск клиента ({foundClientInDictionaryByPhone.FirstName} {foundClientInDictionaryByPhone.LastName})" +
                $" в словаре по номеру телефона - {Stopwatch.Elapsed.TotalMilliseconds} мс.");
        else
            Console.WriteLine("Клиент не найден по номеру телефона в словаре.");
    }

    private static void PrintTheNumberOfClientsUnder42()
    {
        var count = ListBankClients.Count(bankClient => bankClient.Age < 42);
        Console.WriteLine(
            $"Количество клиентов в списке возраст которых меньше 42 - {count}");
    }

    private static void SearchEmployeeInListWithMinimumSalary()
    {
        var foundEmployeeInListWithMinimumSalary = ListBankEmployees.MinBy(bankEmployee => bankEmployee.Salary);
        if (foundEmployeeInListWithMinimumSalary != null)
            Console.WriteLine(
                $"Сотрудник с самой маленькой зарплатой - {foundEmployeeInListWithMinimumSalary.FirstName} {foundEmployeeInListWithMinimumSalary.LastName}" +
                $", {foundEmployeeInListWithMinimumSalary.Salary} $");
        else
            Console.WriteLine("Сотрудник с минимальной зп не найден.");
    }

    private static void PrintTimeToFindLastClientUsingFirstOrDefault(string lastClientPhoneNumber)
    {
        Stopwatch.Reset();
        Stopwatch.Start();
        var foundDictionaryElementWithLastClient =
            DictionaryBankClients.FirstOrDefault(dictionaryElement => dictionaryElement.Key == lastClientPhoneNumber);
        Stopwatch.Stop();
        if (foundDictionaryElementWithLastClient.Key != null)
            Console.WriteLine(
                $"Время на поиск последнего клиента ({foundDictionaryElementWithLastClient.Value.FirstName} " +
                $"{foundDictionaryElementWithLastClient.Value.LastName})" +
                $"в словаре методом FirstOrDefault - {Stopwatch.Elapsed.TotalMilliseconds} мс.");
        else
            Console.WriteLine("Последний клиент в словаре не найден.");
    }

    private static void PrintTimeToFindLastClientUsingTryGetValue(string lastClientPhoneNumber)
    {
        Stopwatch.Reset();
        Stopwatch.Start();
        DictionaryBankClients.TryGetValue(lastClientPhoneNumber, out var foundLastDictionaryElementByKey);
        Stopwatch.Stop();
        if (foundLastDictionaryElementByKey != null)
            Console.WriteLine(
                $"Время на поиск последнего клиента ({foundLastDictionaryElementByKey.FirstName} {foundLastDictionaryElementByKey.LastName})" +
                $" в словаре по ключу - {Stopwatch.Elapsed.TotalMilliseconds} мс.");
        else
            Console.WriteLine("Последний клиент в словаре не найден.");
    }
}