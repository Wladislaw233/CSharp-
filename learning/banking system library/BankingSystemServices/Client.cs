using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemServices;

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
    
    public static Client? CopyClient(Client? client)
    {
        if (client is null)
            return null;
        return new Client()
            {
                ClientId = Guid.NewGuid(),
                FirstName = client.FirstName, 
                LastName = client.LastName, 
                DateOfBirth = client.DateOfBirth, 
                Age = client.Age, 
                PhoneNumber = client.PhoneNumber, 
                Email = client.Email, 
                Address = client.Address
            };
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
               client.DateOfBirth.Equals(DateOfBirth) &&
               client.Age == Age;
    }

    public override int GetHashCode()
    {
        var hash = 14;
        hash = hash * 17 + FirstName?.GetHashCode() ?? 0;
        hash = hash * 17 + LastName?.GetHashCode() ?? 0;
        hash = hash * 17 + DateOfBirth.GetHashCode();
        hash = hash * 17 + Age.GetHashCode();
        hash = hash * 17 + PhoneNumber?.GetHashCode() ?? 0;
        hash = hash * 17 + Email?.GetHashCode() ?? 0;
        hash = hash * 17 + Address?.GetHashCode() ?? 0;
        
        return hash;
    }
}