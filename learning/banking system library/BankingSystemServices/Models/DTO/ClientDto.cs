namespace BankingSystemServices.Models.DTO;

public class ClientDto : PersonDto
{
    public string PhoneNumber { get; set; }
    
    public string Email { get; set; }
    
    public string Address { get; set; }
}