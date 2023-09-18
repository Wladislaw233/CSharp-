using System.Text.Json;

namespace BankingSystemServices.Models.DTO;

public class ErrorDetailsDto
{
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}