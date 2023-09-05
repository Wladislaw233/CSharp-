using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemServices.Models;

public class Person
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public int Age { get; set; }
    
    public decimal Bonus { get; set; }
}