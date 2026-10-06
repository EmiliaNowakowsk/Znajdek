namespace Znajdek.Api.Models;

public class Claim
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public Item Item { get; set; } = null!;

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public string VerificationAnswer { get; set; } = string.Empty;

    public string Status { get; set; } = "PENDING";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? VerifiedAt { get; set; }
}