using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemServices.Models;

public class Employee : Person
{
    public Guid EmployeeId { get; set; }
    
    public string Contract { get; set; }
    
    public decimal Salary { get; set; }
    
    public string Address { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public bool IsOwner { get; set; }

    public static explicit operator Employee(Client client)
    {
        return new Employee
        {
            EmployeeId = Guid.NewGuid(),
            FirstName = client.FirstName,
            LastName = client.LastName,
            DateOfBirth = client.DateOfBirth,
            Age = client.Age,
            Contract = client.FirstName + " " + client.LastName + ", дата рождения: " + client.DateOfBirth,
            Salary = 0,
            Address = client.Address,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            Bonus = client.Bonus
        };
    }

    public static Employee? CopyEmployee(Employee? employee)
    {
        if (employee is null)
            return null;
        
        return new Employee
        {
            EmployeeId = Guid.NewGuid(),
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            DateOfBirth = employee.DateOfBirth,
            Age = employee.Age,
            Contract = employee.Contract,
            Salary = employee.Salary,
            Address = employee.Address,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            Bonus = employee.Bonus,
            IsOwner = employee.IsOwner
        };
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Employee))
            return false;

        var employee = (Employee)obj;

        return employee.FirstName == FirstName &&
               employee.LastName == LastName &&
               employee.PhoneNumber == PhoneNumber &&
               employee.Address == Address &&
               employee.Email == Email &&
               employee.DateOfBirth.Equals(DateOfBirth) &&
               employee.Age == Age &&
               employee.Contract == Contract &&
               employee.Salary.Equals(Salary) &&
               employee.IsOwner == IsOwner &&
               employee.Bonus == Bonus;
    }

    public override int GetHashCode()
    {
        var hash = 11;
        hash = hash * 17 + FirstName.GetHashCode();
        hash = hash * 17 + LastName.GetHashCode();
        hash = hash * 17 + DateOfBirth.GetHashCode();
        hash = hash * 17 + Age.GetHashCode();
        hash = hash * 17 + PhoneNumber.GetHashCode();
        hash = hash * 17 + Email.GetHashCode();
        hash = hash * 17 + Address.GetHashCode();
        hash = hash * 17 + Contract.GetHashCode();
        hash = hash * 17 + Salary.GetHashCode();
        hash = hash * 17 + IsOwner.GetHashCode();
        hash = hash * 17 + Bonus.GetHashCode();
        
        return hash;
    }
}