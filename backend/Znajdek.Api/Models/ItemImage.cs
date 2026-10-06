namespace Znajdek.Api.Models;

public class ItemImage
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public Item Item { get; set; } = null!;

    public string FilePath { get; set; } = string.Empty;
}