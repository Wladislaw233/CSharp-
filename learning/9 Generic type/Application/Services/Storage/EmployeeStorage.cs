using System.Collections;
using BankingSystemServices;
using Services.Exceptions;

namespace Services.Storage;

public class EmployeeStorage : IEmployeeStorage, IEnumerable<Employee>
{
    public List<Employee> Data { get; } = new();

    public void Add(Employee employee)
    {
        if (Data.Contains(employee))
            throw new CustomException("Данный сотрудник уже добавлен в банковскую систему!", nameof(employee));
        
        EmployeeService.ValidationEmployee(employee);
        Data.Add(employee);
    }

    public void Update(Employee employee, string? firstName = null, string? lastName = null, int? age = null,
        DateTime? dateOfBirth = null, string? phoneNumber = null, string? address = null, string? email = null,
        string? contract = null, decimal? salary = null, bool? isOwner = null, decimal? bonus = null)
    {
        if (!Data.Contains(employee))
            throw new CustomException($"Сотрудника {employee.FirstName} {employee.LastName} не существует в банковской системе!",
                nameof(employee));
        
        if (firstName != null)
            employee.FirstName = firstName;
        if (lastName != null)
            employee.LastName = lastName;
        if (age != null)
            employee.Age = (int)age;
        if (dateOfBirth != null)
            employee.DateOfBirth = ((DateTime)dateOfBirth).ToUniversalTime();
        if (phoneNumber != null)
            employee.PhoneNumber = phoneNumber;
        if (address != null)
            employee.Address = address;
        if (email != null)
            employee.Email = email;
        if (contract != null)
            employee.Contract = contract;
        if (salary != null)
            employee.Salary = (decimal)salary;
        if (isOwner != null)
            employee.IsOwner = (bool)isOwner;
        if (bonus != null)
            employee.Bonus = (decimal)bonus;

        EmployeeService.ValidationEmployee(employee);
    }

    public void Delete(Employee employee)
    {
        if (!Data.Contains(employee))
            throw new CustomException("Данного сотрудника не существует в банковской системе!", nameof(employee)); 
        Data.Remove(employee);
    }
    
    public IEnumerator<Employee> GetEnumerator()
    {
        return Data.GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}