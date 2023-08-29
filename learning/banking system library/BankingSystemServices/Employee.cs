using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemServices;

public class Employee : Person
{
    [Required] 
    [Column("id")] 
    public Guid EmployeeId { get; set; } = Guid.NewGuid();

    [Required] 
    [Column("contract")] 
    public string Contract { get; set; }

    [Required] 
    [Column("salary")] 
    public decimal Salary { get; set; }

    [Required] 
    [Column("address")] 
    public string Address { get; set; }

    [Required] 
    [Column("email")] 
    public string Email { get; set; }

    [Required] 
    [Column("phone_number")] 
    public string PhoneNumber { get; set; }

    [Column("is_owner")] 
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
            PhoneNumber = client.PhoneNumber
        };
    }

    public static Employee? CopyEmployee(Employee? employee)
    {
        if (employee is null)
            return null;
        
        return new Employee
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            DateOfBirth = employee.DateOfBirth,
            Age = employee.Age,
            Contract = employee.Contract,
            Salary = employee.Salary,
            Address = employee.Address,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber
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
               employee.IsOwner == IsOwner;
    }

    public override int GetHashCode()
    {
        var hash = 11;
        hash = hash * 17 + FirstName?.GetHashCode() ?? 0;
        hash = hash * 17 + LastName?.GetHashCode() ?? 0;
        hash = hash * 17 + DateOfBirth.GetHashCode();
        hash = hash * 17 + Age.GetHashCode();
        hash = hash * 17 + PhoneNumber?.GetHashCode() ?? 0;
        hash = hash * 17 + Email?.GetHashCode() ?? 0;
        hash = hash * 17 + Address?.GetHashCode() ?? 0;
        hash = hash * 17 + Contract?.GetHashCode() ?? 0;
        hash = hash * 17 + Salary.GetHashCode();
        hash = hash * 17 + IsOwner.GetHashCode();
        
        return hash;
    }
}