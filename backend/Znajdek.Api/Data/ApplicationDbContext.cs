using Microsoft.EntityFrameworkCore;
using Znajdek.Api.Models;

namespace Znajdek.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Item> Items => Set<Item>();

    public DbSet<ItemImage> ItemImages => Set<ItemImage>();

    public DbSet<Claim> Claims => Set<Claim>();
}