using System.Collections;
using BankingSystemServices.Models;
using BankingSystemServices.Exceptions;

namespace Services.Storage;

public class EmployeeStorage : IEmployeeStorage, IEnumerable<Employee>
{
    public List<Employee> EmployeeList { get; } = new();

    public void Add(Employee employee)
    {
        EmployeeList.Add(employee);
    }

    public void Update(Employee employee, Employee newEmployee)
    {
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
    }

    public void Delete(Employee employee)
    {
        EmployeeList.Remove(employee);
    }

    public IEnumerator<Employee> GetEnumerator()
    {
        return EmployeeList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}