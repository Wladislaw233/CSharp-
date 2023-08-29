using System.Diagnostics;
using BankingSystemServices;
using BankingSystemServices.Services;

namespace PracticeWithListAndDictionary;

internal class Program
{
    private static readonly List<Client> ListBankClients = TestDataGenerator.GenerateListWitchBankClients(1000);

    private static readonly Dictionary<string, Client> DictionaryBankClients =
        TestDataGenerator.GenerateDictionaryWithBankClients(ListBankClients);

    private static readonly List<Employee> ListBankEmployees = TestDataGenerator.GenerateListWithBankEmployees(1000);

    private static readonly Stopwatch Stopwatch = new();

    public static void Main(string[] args)
    {
        var phoneNumber = ListBankClients[501].PhoneNumber;

        // 2.а
        SearchClientInListByPhoneNumber(phoneNumber);

        // 2.б
        SearchClientInDictionaryByPhoneNumber(phoneNumber);

        // 2.в
        PrintTheNumberOfClientsUnder42();

        // 2.г
        SearchEmployeeInListWithMinimumSalary();

        // 2.д.1
        PrintTimeToFindLastClientUsingFirstOrDefault();

        // 2.д.2
        PrintTimeToFindLastClientUsingTryGetValue();
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
    }

    private static void SearchClientInDictionaryByPhoneNumber(string phoneNumber)
    {
        Stopwatch.Start();
        var clientFound = DictionaryBankClients.TryGetValue(phoneNumber, out var foundClientInDictionaryByPhone);
        Stopwatch.Stop();
        if (clientFound && foundClientInDictionaryByPhone != null)
            Console.WriteLine(
                $"Время на поиск клиента ({foundClientInDictionaryByPhone.FirstName} {foundClientInDictionaryByPhone.LastName})" +
                $" в словаре по номеру телефона - {Stopwatch.Elapsed.TotalMilliseconds} мс.");
    }

    private static void PrintTheNumberOfClientsUnder42()
    {
        Console.WriteLine(
            $"Количество клиентов в списке возраст которых меньше 42 - {ListBankClients.Count(bankClient => bankClient.Age < 42)}");
    }

    private static void SearchEmployeeInListWithMinimumSalary()
    {
        var foundEmployeeInListWithMinimumSalary = ListBankEmployees.MinBy(bankEmployee => bankEmployee.Salary);
        if (foundEmployeeInListWithMinimumSalary != null)
            Console.WriteLine(
                $"Сотрудник с самой маленькой зарплатой - {foundEmployeeInListWithMinimumSalary.FirstName} {foundEmployeeInListWithMinimumSalary.LastName}" +
                $", {foundEmployeeInListWithMinimumSalary.Salary} $");
    }

    private static void PrintTimeToFindLastClientUsingFirstOrDefault()
    {
        Stopwatch.Start();
        var foundDictionaryElementWithLastClient =
            DictionaryBankClients.FirstOrDefault(dictionaryElement => dictionaryElement.Key == "00000000");
        Stopwatch.Stop();
        if (foundDictionaryElementWithLastClient.Key != null)
            Console.WriteLine(
                $"Время на поиск последнего клиента ({foundDictionaryElementWithLastClient.Value.FirstName} " +
                $"{foundDictionaryElementWithLastClient.Value.LastName})" +
                $"в словаре методом FirstOrDefault - {Stopwatch.Elapsed.TotalMilliseconds} мс.");
    }

    private static void PrintTimeToFindLastClientUsingTryGetValue()
    {
        Stopwatch.Start();
        DictionaryBankClients.TryGetValue("00000000", out var foundLastDictionaryElementByKey);
        Stopwatch.Stop();
        if (foundLastDictionaryElementByKey != null)
            Console.WriteLine(
                $"Время на поиск последнего клиента ({foundLastDictionaryElementByKey.FirstName} {foundLastDictionaryElementByKey.LastName})" +
                $" в словаре по ключу - {Stopwatch.Elapsed.TotalMilliseconds} мс.");
    }
}