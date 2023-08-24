using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Models;

public class Employee : Person
{
    [Required]
    [Column("id")]
    public Guid EmployeeId { get; set; }
    
    [Required]
    [Column("contract")]
    public string Contract { get; set; }
    
    [Required]
    [Column("salary")]
    public double Salary { get; set; }
    
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

    public Employee(string firstName, string lastName, DateTime dateOfBirth, int age, string contract, double salary = 0,
        string address = "", string email = "", string phoneNumber = "", bool isOwner = false)
        : base(firstName, lastName, dateOfBirth, age)
    {
        Contract = contract;
        Salary = salary;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
        EmployeeId = Guid.NewGuid();
        IsOwner = isOwner;
    }

    // explicit - при явном преобразовании
    // implicit - при не явном преобразовании
    public static explicit operator Employee(Client client)
    {
        return new Employee(
            client.FirstName,
            client.LastName,
            client.DateOfBirth,
            client.Age,
            client.FirstName + " " + client.LastName + ", дата рождения: " + client.DateOfBirth,
            0,
            client.Address,
            client.Email,
            client.PhoneNumber
        );
    }

    public static Employee CopyEmployee(Employee employee)
    {
        return new Employee(employee.FirstName,
            employee.LastName,
            employee.DateOfBirth,
            employee.Age,
            employee.Contract,
            employee.Salary,
            employee.Address,
            employee.Email,
            employee.PhoneNumber);
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
               employee.DateOfBirth == DateOfBirth &&
               employee.Age == Age &&
               employee.Contract == Contract &&
               employee.Salary == Salary;
    }

    public override int GetHashCode()
    {
        var convertedString = FirstName + LastName + PhoneNumber + Address + Email + DateOfBirth + Age + Contract + Salary;
        byte[] hashBytes;
        using (var sha256 = SHA256.Create())
        {
            hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(convertedString));
        }
        return BitConverter.ToInt32(hashBytes);
    }
}