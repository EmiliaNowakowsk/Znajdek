namespace Znajdek.Api.Models;

public class Item
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Type { get; set; } = "LOST";

    public string Category { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Status { get; set; } = "ACTIVE";

    public int CreatedById { get; set; }

    public User CreatedBy { get; set; } = null!;

    public ICollection<ItemImage> Images { get; set; } = new List<ItemImage>();

    public ICollection<Claim> Claims { get; set; } = new List<Claim>();
}