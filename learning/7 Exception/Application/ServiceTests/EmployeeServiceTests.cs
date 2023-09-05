﻿using BankingSystemServices.Models;
using BankingSystemServices.Services;
using Services;
using BankingSystemServices.Exceptions;

namespace ServiceTests;

public class EmployeeServiceTests
{
    public void EmployeeServiceTest()
    {
        AddBankEmployeeTest();
    }
    
    private void AddBankEmployeeTest()
    {
        var bankEmployees = TestDataGenerator.GenerateListWithBankEmployees(3);
        var employeeService = new EmployeeService();
        bankEmployees.Add(TestDataGenerator.GenerateRandomInvalidEmployee(true));
        bankEmployees.Add(TestDataGenerator.GenerateRandomInvalidEmployee());

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("СОТРУДНИКИ");
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Добавление сотрудников:");
        Console.ResetColor();
        try
        {
            foreach (var employee in bankEmployees)
            {
                Console.WriteLine(
                    $"\nПопытка добавления сотрудника: Имя: {employee.FirstName}, фамилия: {employee.LastName}, контракт: {employee.Contract}");
                employeeService.AddBankEmployee(employee);
                Console.WriteLine("Успешно!");
            }
        }
        catch (CustomException exception)
        {
            CustomException.ExceptionHandling("При добавлении сотрудника произошла ошибка: ", exception);
        }

        Console.WriteLine("\nСписок сотрудников после добавления:");
        employeeService.WithdrawEmployees();
    }
}