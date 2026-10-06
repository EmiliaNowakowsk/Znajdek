using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Znajdek.Api.Data;
using Znajdek.Api.DTOs;
using Znajdek.Api.Models;

namespace Znajdek.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ItemsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/items
    [HttpGet]
public async Task<ActionResult<IEnumerable<ItemDto>>> GetItems(
    string? search,
    string? type,
    string? category,
    string? location,
    string? status)
{
    var query = _context.Items
        .AsNoTracking()
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(item =>
            item.Title.Contains(search) ||
            item.Description.Contains(search));
    }

    if (!string.IsNullOrWhiteSpace(type))
    {
        query = query.Where(item =>
            item.Type == type);
    }

    if (!string.IsNullOrWhiteSpace(category))
    {
        query = query.Where(item =>
            item.Category == category);
    }

    if (!string.IsNullOrWhiteSpace(location))
    {
        query = query.Where(item =>
            item.Location.Contains(location));
    }

    if (!string.IsNullOrWhiteSpace(status))
    {
        query = query.Where(item =>
            item.Status == status);
    }

    var items = await query
        .Select(item => new ItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            Type = item.Type,
            Category = item.Category,
            Location = item.Location,
            Date = item.Date,
            Status = item.Status,
            CreatedById = item.CreatedById
        })
        .ToListAsync();

    return Ok(items);
}

    // GET: api/items/1
    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItem(int id)
    {
        var item = await _context.Items
            .AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new ItemDto
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Type = item.Type,
                Category = item.Category,
                Location = item.Location,
                Date = item.Date,
                Status = item.Status,
                CreatedById = item.CreatedById
            })
            .FirstOrDefaultAsync();

        if (item == null)
        {
            return NotFound(new
            {
                message = "Nie znaleziono zgłoszenia."
            });
        }

        return Ok(item);
    }

    // POST: api/items
    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItem(CreateItemDto dto)
    {
        if (dto.Type != "LOST" && dto.Type != "FOUND")
{
    return BadRequest(new
    {
        message = "Typ zgłoszenia musi być LOST albo FOUND."
    });
}
        var userExists = await _context.Users
    .AnyAsync(user => user.Id == dto.CreatedById);

if (!userExists)
{
    return BadRequest(new
    {
        message = "Użytkownik o podanym ID nie istnieje."
    });
}
        var item = new Item
{
    Title = dto.Title,
    Description = dto.Description,
    Type = dto.Type,
    Category = dto.Category,
    Location = dto.Location,
    Date = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc),
    Status = "ACTIVE",
    CreatedById = dto.CreatedById
};

        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        var result = new ItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            Type = item.Type,
            Category = item.Category,
            Location = item.Location,
            Date = item.Date,
            Status = item.Status,
            CreatedById = item.CreatedById
        };

        return CreatedAtAction(
            nameof(GetItem),
            new { id = item.Id },
            result);
    }
}