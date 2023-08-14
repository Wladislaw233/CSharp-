﻿using System.Diagnostics;
using Services;

namespace PracticeWithListAndDictionary;

internal class Program
{
    public static void Main(string[] args)
    {
        // 1.a
        var listBankClients = TestDataGenerator.GenerateListWitchThousandBankClients();
        // 1.б
        var dictionaryBankClients = TestDataGenerator.GenerateDictionaryWithBankClients(listBankClients);
        // 1.в
        var listBankEmployees = TestDataGenerator.GenerateListWithThousandEmployees();

        var phoneNumber = listBankClients[501].PhoneNumber;
        var stopWatch = new Stopwatch();

        // 2.а
        stopWatch.Start();
        var foundClientInListByPhone = listBankClients.Find(bankClient => bankClient.PhoneNumber == phoneNumber);
        stopWatch.Stop();
        Console.WriteLine(
            $"Время на поиск клиента ({foundClientInListByPhone.FirstName} {foundClientInListByPhone.LastName}) " +
            $"в списке по номеру телефона - {stopWatch.Elapsed.TotalMilliseconds} мс.");

        // 2.б
        stopWatch.Start();
        var clientFound = dictionaryBankClients.TryGetValue(phoneNumber, out var foundClientInDictionaryByPhone);
        stopWatch.Stop();
        Console.WriteLine("Время на поиск клиента (" +
                          (clientFound
                              ? foundClientInDictionaryByPhone.FirstName + " " + foundClientInDictionaryByPhone.LastName
                              : "Клиент не найден") +
                          $") в словаре по номеру телефона - {stopWatch.Elapsed.TotalMilliseconds} мс.");

        // 2.в
        var numberOfFoundClientsInListByAge = listBankClients.Where(bankClient => bankClient.Age < 42).Count();
        Console.WriteLine(
            $"Количество клиентов в списке возраст которых меньше 42 - {numberOfFoundClientsInListByAge}");

        // 2.г
        var foundEmployeeInListWithMinimumSalary = listBankEmployees.MinBy(bankEmployee => bankEmployee.Salary);
        Console.WriteLine(
            $"Сотрудник с самой маленькой зарплатой - {foundEmployeeInListWithMinimumSalary.FirstName} {foundEmployeeInListWithMinimumSalary.LastName}, {foundEmployeeInListWithMinimumSalary.Salary} $");

        // 2.д.1
        stopWatch.Start();
        var foundDictionaryElementWithLastClient =
            dictionaryBankClients.FirstOrDefault(dictionaryElement => dictionaryElement.Key == "00000000");
        stopWatch.Stop();
        Console.WriteLine("Время на поиск последнего клиента (" +
                          (foundDictionaryElementWithLastClient.Key != null
                              ? foundDictionaryElementWithLastClient.Value.FirstName + " " +
                                foundDictionaryElementWithLastClient.Value.LastName
                              : "Клиент не найден") +
                          $") в словаре методом FirstOrDefault - {stopWatch.Elapsed.TotalMilliseconds} мс.");

        // 2.д.2
        stopWatch.Start();
        dictionaryBankClients.TryGetValue("00000000", out var foundDictionaryElementWithLastClientByKey);
        stopWatch.Stop();
        Console.WriteLine("Время на поиск последнего клиента (" +
                          (foundDictionaryElementWithLastClientByKey != null
                              ? foundDictionaryElementWithLastClientByKey.FirstName + " " +
                                foundDictionaryElementWithLastClientByKey.LastName
                              : "Клиент не найден") +
                          $") в словаре по ключу - {stopWatch.Elapsed.TotalMilliseconds} мс.");

        Console.ReadLine();
    }
}