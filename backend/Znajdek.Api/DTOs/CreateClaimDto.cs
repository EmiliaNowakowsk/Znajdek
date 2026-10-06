using System.ComponentModel.DataAnnotations;

namespace Znajdek.Api.DTOs;

public class CreateClaimDto
{
    [Required(ErrorMessage = "ID przedmiotu jest wymagane.")]
    [Range(1, int.MaxValue, ErrorMessage = "Nieprawidłowe ID przedmiotu.")]
    public int ItemId { get; set; }

    [Required(ErrorMessage = "ID użytkownika jest wymagane.")]
    [Range(1, int.MaxValue, ErrorMessage = "Nieprawidłowe ID użytkownika.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Informacja weryfikacyjna jest wymagana.")]
    [StringLength(
        1000,
        ErrorMessage = "Informacja weryfikacyjna może mieć maksymalnie 1000 znaków.")]
    public string VerificationAnswer { get; set; } = string.Empty;
}