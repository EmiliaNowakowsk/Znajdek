using System.ComponentModel.DataAnnotations;

namespace Znajdek.Api.DTOs;

public class CreateItemDto
{
    [Required(ErrorMessage = "Tytuł zgłoszenia jest wymagany.")]
    [StringLength(100, ErrorMessage = "Tytuł może mieć maksymalnie 100 znaków.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Opis zgłoszenia jest wymagany.")]
    [StringLength(1000, ErrorMessage = "Opis może mieć maksymalnie 1000 znaków.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Typ zgłoszenia jest wymagany.")]
    public string Type { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kategoria jest wymagana.")]
    [StringLength(100, ErrorMessage = "Kategoria może mieć maksymalnie 100 znaków.")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lokalizacja jest wymagana.")]
    [StringLength(200, ErrorMessage = "Lokalizacja może mieć maksymalnie 200 znaków.")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data zgłoszenia jest wymagana.")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Użytkownik zgłaszający jest wymagany.")]
    [Range(1, int.MaxValue, ErrorMessage = "Nieprawidłowe ID użytkownika.")]
    public int CreatedById { get; set; }
}