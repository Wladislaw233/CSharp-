using System.Security.Cryptography;
using System.Text;

namespace Models;

public class Client : Person
{
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }

    public Client(string firstName, string lastName, DateTime dateOfBirth, int age, string phoneNumber = "",
        string email = "", string address = "") : base(firstName, lastName, dateOfBirth, age)
    {
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
    }
    public static Client CopyClient(Client client)
    {
        return new Client(client.FirstName, client.LastName, client.DateOfBirth, client.Age, client.PhoneNumber, client.Email, client.Address);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Client)) return false;

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
        using (var sha256 = SHA256.Create())
        {
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(convertedString));
            return BitConverter.ToInt32(hashBytes);
        }
    }
}