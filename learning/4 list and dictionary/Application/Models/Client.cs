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
}