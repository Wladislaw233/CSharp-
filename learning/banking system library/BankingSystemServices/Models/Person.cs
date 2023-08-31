using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemServices.Models;

public class Person
{
    [Required]
    [Column("first_name")]
    public string FirstName { get; set; }
    
    [Required]
    [Column("last_name")]
    public string LastName { get; set; }
    
    [Required]
    [Column("date_of_birth")]
    public DateTime DateOfBirth { get; set; }
    
    [Required]
    [Column("age")]
    public int Age { get; set; }
    
    [Column("bonus")]
    public decimal Bonus { get; set; }
}