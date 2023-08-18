namespace Models;

public class Person
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public int Age { get; set; }

    protected Person(string firstName, string lastName, DateTime dateOfBirth, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Age = age;
    }
}