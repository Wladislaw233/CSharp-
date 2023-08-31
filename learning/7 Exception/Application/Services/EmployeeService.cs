using BankingSystemServices.Services;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;

namespace Services;

public class EmployeeService
{
    private readonly List<Employee> _bankEmployees = new();

    public void AddBankEmployee(Employee employee)
    {
        ValidationEmployee(employee);
        _bankEmployees.Add(employee);
    }

    public void WithdrawEmployees()
    {
        Console.WriteLine(string.Join('\n',
            _bankEmployees.Select(employee =>
                $"Имя: {employee.FirstName}, фамилия: {employee.LastName}, контракт: {employee.Contract}")));
    }

    private void ValidationEmployee(Employee employee)
    {
        if (_bankEmployees.Contains(employee))
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
            employee.Contract = TestDataGenerator.GenerateEmployeeContract(employee);
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
}