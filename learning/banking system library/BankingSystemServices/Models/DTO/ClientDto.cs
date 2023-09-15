using System.ComponentModel.DataAnnotations;

namespace BankingSystemServices.Models.DTO;

/// <summary>
///     Client data transfer model for API. 
/// </summary>
public class ClientDto : PersonDto
{
    /// <summary>
    ///     Client phone number.
    /// </summary>
    [Required]
    public string PhoneNumber { get; set; }

    /// <summary>
    ///     Client email.
    /// </summary>
    [Required]
    public string Email { get; set; }

    /// <summary>
    ///     Client address.
    /// </summary>
    [Required]
    public string Address { get; set; }
}