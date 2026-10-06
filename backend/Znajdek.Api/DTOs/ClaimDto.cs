namespace Znajdek.Api.DTOs;

public class ClaimDto
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int UserId { get; set; }
    public string VerificationAnswer { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? VerifiedAt { get; set; }
}