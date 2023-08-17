namespace Models;

public class Owner : Employee
{
    public bool IsOwner { get; init; }

    public Owner(string firstName, string lastName, DateTime dateOfBirth, int age, string contract, int salary = 0,
        string address = "", string email = "", string phoneNumber = "", bool isOwner = true)
        : base(firstName, lastName, dateOfBirth, age, contract, salary,
            address, email, phoneNumber)
    {
        IsOwner = isOwner;
    }
}