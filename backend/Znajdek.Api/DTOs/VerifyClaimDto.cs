using System.ComponentModel.DataAnnotations;

namespace Znajdek.Api.DTOs;

public class VerifyClaimDto
{
    [Required]
    public bool Approved { get; set; }

    [StringLength(
        500,
        ErrorMessage = "Komentarz może mieć maksymalnie 500 znaków.")]
    public string Comment { get; set; } = string.Empty;
}