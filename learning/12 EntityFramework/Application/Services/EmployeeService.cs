﻿using BankingSystemServices.Database;
using BankingSystemServices.Exceptions;
using BankingSystemServices.Models;
using BankingSystemServices.Services;

namespace Services;

public class EmployeeService
{
    private readonly BankingSystemDbContext _bankingSystemDbContext;

    public EmployeeService(BankingSystemDbContext bankingSystemDbContext)
    {
        _bankingSystemDbContext = bankingSystemDbContext;
    }

    public void AddEmployee(Employee employee)
    {
        ValidateEmployee(employee);

        _bankingSystemDbContext.Employees.Add(employee);

        _bankingSystemDbContext.SaveChanges();
    }

    public void UpdateEmployee(Guid employeeId, Employee newEmployee)
    {
        var employee = GetEmployeeById(employeeId);

        ValidateEmployee(newEmployee, true);

        employee.FirstName = newEmployee.FirstName;
        employee.LastName = newEmployee.LastName;
        employee.DateOfBirth = newEmployee.DateOfBirth;
        employee.PhoneNumber = newEmployee.PhoneNumber;
        employee.Address = newEmployee.Address;
        employee.Contract = newEmployee.Contract;
        employee.Email = newEmployee.Email;
        employee.Salary = newEmployee.Salary;
        employee.IsOwner = newEmployee.IsOwner;
        employee.Age = newEmployee.Age;
        employee.Bonus = newEmployee.Bonus;
        
        _bankingSystemDbContext.SaveChanges();
    }

    public void DeleteEmployee(Guid employeeId)
    {
        var bankEmployee = GetEmployeeById(employeeId);

        _bankingSystemDbContext.Employees.Remove(bankEmployee);

        _bankingSystemDbContext.SaveChanges();
    }

    private Employee GetEmployeeById(Guid employeeId)
    {
        var bankEmployee =
            _bankingSystemDbContext.Employees.SingleOrDefault(employee => employee.EmployeeId.Equals(employeeId));

        if (bankEmployee == null)
            throw new ArgumentException($"The employee with identifier {employeeId} does not exist!",
                nameof(employeeId));

        return bankEmployee;
    }

    private void ValidateEmployee(Employee employee, bool isUpdate = false)
    {
        if (!isUpdate && _bankingSystemDbContext.Employees.Any(e => e.EmployeeId.Equals(employee.EmployeeId)))
            throw new ArgumentException("This employee has already been added to the banking system!",
                nameof(employee));

        if (string.IsNullOrWhiteSpace(employee.FirstName))
            throw new PropertyValidationException("The employee first name is not specified!",
                nameof(employee.FirstName), nameof(Employee));

        if (string.IsNullOrWhiteSpace(employee.LastName))
            throw new PropertyValidationException("The employee last name is not specified!", nameof(employee.LastName),
                nameof(Employee));

        if (string.IsNullOrWhiteSpace(employee.PhoneNumber))
            throw new PropertyValidationException("The employee phone number is not specified!",
                nameof(employee.PhoneNumber), nameof(Employee));

        if (string.IsNullOrWhiteSpace(employee.Email))
            throw new PropertyValidationException("The employee e-mail is not specified!", nameof(employee.Email),
                nameof(Employee));

        if (string.IsNullOrWhiteSpace(employee.Address))
            throw new PropertyValidationException("The employee address is not specified!", nameof(employee.Address),
                nameof(Employee));

        if (employee.Salary == 0)
            throw new PropertyValidationException("The employee salary is not specified!", nameof(employee.Salary),
                nameof(Employee));

        if (string.IsNullOrWhiteSpace(employee.Contract))
            employee.Contract = $"{employee.FirstName} {employee.LastName}, date of birth: {employee.DateOfBirth}";

        if (employee.DateOfBirth > DateTime.Now || employee.DateOfBirth == DateTime.MinValue ||
            employee.DateOfBirth == DateTime.MaxValue)
            throw new PropertyValidationException("The employee's date of birth is incorrect!",
                nameof(employee.DateOfBirth), nameof(Employee));

        var age = TestDataGenerator.CalculateAge(employee.DateOfBirth);

        if (age < 18)
            throw new PropertyValidationException("Employee is under 18 years old!", nameof(employee.Age),
                nameof(Employee));

        if (age != employee.Age || employee.Age <= 0) employee.Age = age;
    }

    public IEnumerable<Employee> EmployeesWithFilterAndPagination(int page, int pageSize, string? firstName = null,
        string? lastName = null, int? age = null,
        DateTime? dateOfBirth = null, string? phoneNumber = null, string? address = null, string? email = null,
        string? contract = null, double? salary = null, bool? isOwner = null)
    {
        IQueryable<Employee> query = _bankingSystemDbContext.Employees;
        
        if (firstName != null)
            query = query.Where(employee => employee.FirstName == firstName);
        
        if (lastName != null)
            query = query.Where(employee => employee.LastName == lastName);
        
        if (age != null)
            query = query.Where(employee => employee.Age.Equals((int)age));
        
        if (dateOfBirth != null)
            query = query.Where(employee => employee.DateOfBirth.Equals(((DateTime)dateOfBirth).ToUniversalTime()));
        
        if (phoneNumber != null)
            query = query.Where(employee => employee.PhoneNumber == phoneNumber);
        
        if (address != null)
            query = query.Where(employee => employee.Address == address);
        
        if (email != null)
            query = query.Where(employee => employee.Email == email);
        
        if (contract != null)
            query = query.Where(employee => employee.Contract == contract);
        
        if (salary != null)
            query = query.Where(employee => employee.Salary.Equals(salary));
        
        if (isOwner != null)
            query = query.Where(employee => employee.IsOwner.Equals(isOwner));

        query = query.OrderBy(employee => employee.FirstName);

        query = query.Skip((page - 1) * pageSize).Take(pageSize);

        return query.ToList();
    }
}