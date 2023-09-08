namespace BankingSystemServices.Models.DTO;

public class EmployeeDto : PersonDto
{
    public string Contract { get; set; }
    
    public decimal Salary { get; set; }
    
    public string Address { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public bool IsOwner { get; set; }
}