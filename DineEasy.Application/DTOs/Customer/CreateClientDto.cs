using System.ComponentModel.DataAnnotations;

namespace DineEasy.Application.DTOs.Customer;

public class CreateCustomerDto
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [MaxLength(20)]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;
}