﻿using Models;
using Services.Exceptions;
using Services.Storage;

namespace Services;

public class EmployeeService
{
    public static void ValidationEmployee(List<Employee> bankEmployees,Employee employee)
    {
        if (bankEmployees.Contains(employee))
            throw new CustomException("Данный сотрудник уже добавлен в банковскую систему!", nameof(employee));
        if (string.IsNullOrWhiteSpace(employee.FirstName))
            throw new CustomException("Не указано имя сотрудника!", nameof(employee.FirstName));
        if (string.IsNullOrWhiteSpace(employee.LastName))
            throw new CustomException("Не указана фамилия сотрудника!", nameof(employee.LastName));
        if (string.IsNullOrWhiteSpace(employee.PhoneNumber))
            throw new CustomException("Не указан номер сотрудника!", nameof(employee.PhoneNumber));
        if (string.IsNullOrWhiteSpace(employee.Email))
            throw new CustomException("Не указан e-mail сотрудника!", nameof(employee.Email));
        if (string.IsNullOrWhiteSpace(employee.Address))
            throw new CustomException("Не указан адрес сотрудника!", nameof(employee.Email));
        if (employee.Salary == 0)
            throw new CustomException("Не указана зарплата сотрудника!", nameof(employee.Salary));
        if (string.IsNullOrWhiteSpace(employee.Contract))
        {
            employee.Contract = $"{employee.FirstName} {employee.LastName}, дата рождения: {employee.DateOfBirth}";
            Console.WriteLine("Контракт сотрудника был создан!");
        }

        if (employee.DateOfBirth > DateTime.Now || employee.DateOfBirth == DateTime.MinValue ||
            employee.DateOfBirth == DateTime.MaxValue)
            throw new CustomException("Дата рождения сотрудника указана неверно!", nameof(employee.DateOfBirth));

        var age = TestDataGenerator.CalculateAge(employee.DateOfBirth);
        
        if (age < 18)
            throw new CustomException("сотрудника меньше 18 лет!", nameof(employee.Age));

        if (age != employee.Age || employee.Age <= 0)
        {
            employee.Age = age;
            Console.WriteLine("Возраст сотрудника указан неверно и был скорректирован по дате его рождения!");
        }
    }

    public static void BeforeDeletingEmployee(List<Employee> bankEmployees, Employee employee)
    {
        if (!bankEmployees.Contains(employee))
            throw new CustomException("Данного сотрудника не существует в банковской системе!", nameof(employee)); 
    }

    public static IEnumerable<Employee> GetEmployeesByFilters(EmployeeStorage employeeStorage, string firstNameFilter = "",
        string lastNameFilter = "", string phoneNumberFilter = "", string contractFilter = "", int? salaryFilter = null, DateTime? minDateOfBirth = null,
        DateTime? maxDateOfBirth = null)
    {
        IEnumerable<Employee> filteredEmployees = employeeStorage.Data;
        if (!string.IsNullOrWhiteSpace(firstNameFilter))
            filteredEmployees = filteredEmployees.Where(employee  => employee.FirstName.Contains(firstNameFilter));
        if (!string.IsNullOrWhiteSpace(lastNameFilter))
            filteredEmployees = filteredEmployees.Where(employee  => employee.LastName.Contains(lastNameFilter));
        if (!string.IsNullOrWhiteSpace(phoneNumberFilter))
            filteredEmployees = filteredEmployees.Where(employee  => employee.PhoneNumber.Contains(phoneNumberFilter));
        if (!string.IsNullOrWhiteSpace(contractFilter))
            filteredEmployees = filteredEmployees.Where(employee  => employee.Contract.Contains(contractFilter));
        if (salaryFilter.HasValue)
            filteredEmployees = filteredEmployees.Where(employee  => employee.Salary == salaryFilter);
        if (minDateOfBirth.HasValue)
            filteredEmployees = filteredEmployees.Where(employee  => employee.DateOfBirth >= minDateOfBirth);
        if (maxDateOfBirth.HasValue)
            filteredEmployees = filteredEmployees.Where(employee  => employee.DateOfBirth <= maxDateOfBirth);
        return filteredEmployees;
    }
}