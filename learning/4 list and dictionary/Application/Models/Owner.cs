namespace Models;

public class Owner : Employee
{
    public bool ItsOwner { get; set; }

    public Owner(string firstName, string lastName, DateTime dateOfBirth, int age, string contract, int salary = 0,
        string address = "", string email = "", string phoneNumber = "", bool itsOwner = true)
        : base(firstName, lastName, dateOfBirth, age, contract, salary,
            address, email, phoneNumber)
    {
        ItsOwner = itsOwner;
    }
}