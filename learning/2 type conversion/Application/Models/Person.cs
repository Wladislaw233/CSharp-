namespace Models;

public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Age { get; set; }

    protected Person(string firstName, string lastName, DateTime dateOfBirth, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Age = age;
    }
}