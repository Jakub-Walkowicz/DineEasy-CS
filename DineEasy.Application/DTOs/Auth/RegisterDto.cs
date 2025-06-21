using System.ComponentModel.DataAnnotations;

namespace DineEasy.SharedKernel.Models.Auth;

public class RegisterDto
{
    [Required(ErrorMessage = "Nazwa użytkownika jest wymagana")]
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy format email")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Hasło jest wymagane")]
    [MinLength(6, ErrorMessage = "Hasło musi mieć minimum 6 znaków")]
    public string Password { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Imię jest wymagane")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    public string LastName { get; set; } = string.Empty;
    
    [Phone(ErrorMessage = "Nieprawidłowy numer telefonu")]
    public string? PhoneNumber { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
}