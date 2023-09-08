namespace BankingSystemServices.Models.DTO;

public class PersonDto
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public int Age { get; set; }
    
    public decimal Bonus { get; set; }
}