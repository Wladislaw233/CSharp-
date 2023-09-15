using System.ComponentModel.DataAnnotations;

namespace BankingSystemServices.Models.DTO;

public class EmployeeDto : PersonDto
{
    /// <summary>
    ///     Employee contract.
    /// </summary>
    [Required]
    public string Contract { get; set; }

    /// <summary>
    ///     Employee salary.
    /// </summary>
    [Required]
    public decimal Salary { get; set; }

    /// <summary>
    ///     Employee address.
    /// </summary>
    [Required]
    public string Address { get; set; }

    /// <summary>
    ///     Employee email.
    /// </summary>
    [Required]
    public string Email { get; set; }

    /// <summary>
    ///     Employee phone number.
    /// </summary>
    [Required]
    public string PhoneNumber { get; set; }

    /// <summary>
    ///     flag whether the employee is the owner of the bank.
    /// </summary>
    public bool IsOwner { get; set; }
}