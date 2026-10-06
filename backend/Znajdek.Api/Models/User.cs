namespace Znajdek.Api.Models;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = "User";

    public ICollection<Item> Items { get; set; } = new List<Item>();

    public ICollection<Claim> Claims { get; set; } = new List<Claim>();
}