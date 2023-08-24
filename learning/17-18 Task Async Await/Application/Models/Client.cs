using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Models;

public class Client : Person
{
    [Required]
    [Column("id")]
    public Guid ClientId { get; set; }
    
    [Required]
    [Column("phone_number")]
    public string PhoneNumber { get; set; }
    
    [Required]
    [Column("email")]
    public string Email { get; set; }
    
    [Required]
    [Column("address")]
    public string Address { get; set; }

    public Client(string firstName, string lastName, DateTime dateOfBirth, int age, string phoneNumber,
        string email, string address) : base(firstName, lastName, dateOfBirth, age)
    {
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
        ClientId = Guid.NewGuid();
    }
    public static Client CopyClient(Client client)
    {
        return new Client(client.FirstName, client.LastName, client.DateOfBirth, client.Age, client.PhoneNumber, client.Email, client.Address);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Client))
            return false;

        var client = (Client)obj;

        return client.FirstName == FirstName &&
               client.LastName == LastName &&
               client.PhoneNumber == PhoneNumber &&
               client.Address == Address &&
               client.Email == Email &&
               client.DateOfBirth == DateOfBirth &&
               client.Age == Age;
    }
    
    public override int GetHashCode()
    {
        var convertedString = FirstName + LastName + PhoneNumber + Address + Email + DateOfBirth + Age;
        byte[] hashBytes;
        using (var sha256 = SHA256.Create())
        {
            hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(convertedString));
        }
        return BitConverter.ToInt32(hashBytes);
    }
}