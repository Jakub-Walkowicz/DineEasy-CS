using System.ComponentModel.DataAnnotations;

namespace DineEasy.SharedKernel.Models.Reservation;

public class CreateReservationDto
{
    [Required]
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "Wybierz stolik")]
    public int TableId { get; set; }
    
    [Required(ErrorMessage = "Wybierz datę i godzinę")]
    public DateTime ReservationDateTime { get; set; }
    
    [Required]
    [Range(1, 8, ErrorMessage = "Czas trwania musi być między 1 a 8 godzin")]
    public int Duration { get; set; } = 2;
    
    [Required(ErrorMessage = "Podaj liczbę osób")]
    [Range(1, 20, ErrorMessage = "Liczba osób musi być między 1 a 20")]
    public int PartySize { get; set; }
    
    [MaxLength(500, ErrorMessage = "Uwagi nie mogą przekraczać 500 znaków")]
    public string? SpecialRequests { get; set; } = string.Empty;
}