using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Znajdek.Api.Data;
using Znajdek.Api.DTOs;
using Znajdek.Api.Models;

namespace Znajdek.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ClaimsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClaimDto>>> GetClaims()
    {
        var claims = await _context.Claims
            .AsNoTracking()
            .Select(claim => new ClaimDto
            {
                Id = claim.Id,
                ItemId = claim.ItemId,
                UserId = claim.UserId,
                VerificationAnswer = claim.VerificationAnswer,
                Status = claim.Status,
                CreatedAt = claim.CreatedAt,
                VerifiedAt = claim.VerifiedAt
            })
            .ToListAsync();

        return Ok(claims);
    }

    [HttpPost]
    public async Task<ActionResult<ClaimDto>> CreateClaim(CreateClaimDto dto)
    {
        var item = await _context.Items
            .FirstOrDefaultAsync(item => item.Id == dto.ItemId);

        if (item == null)
        {
            return BadRequest(new
            {
                message = "Przedmiot o podanym ID nie istnieje."
            });
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(user => user.Id == dto.UserId);

        if (user == null)
        {
            return BadRequest(new
            {
                message = "Użytkownik o podanym ID nie istnieje."
            });
        }

        if (item.Status != "ACTIVE")
        {
            return BadRequest(new
            {
                message = "Dla tego zgłoszenia nie można już wysłać roszczenia."
            });
        }

        var existingClaim = await _context.Claims
            .AnyAsync(claim =>
                claim.ItemId == dto.ItemId &&
                claim.UserId == dto.UserId &&
                claim.Status == "PENDING");

        if (existingClaim)
        {
            return BadRequest(new
            {
                message = "Użytkownik ma już oczekujące roszczenie dla tego zgłoszenia."
            });
        }

        var claim = new Claim
        {
            ItemId = dto.ItemId,
            UserId = dto.UserId,
            VerificationAnswer = dto.VerificationAnswer,
            Status = "PENDING",
            CreatedAt = DateTime.UtcNow
        };

        _context.Claims.Add(claim);
        await _context.SaveChangesAsync();

        var result = new ClaimDto
        {
            Id = claim.Id,
            ItemId = claim.ItemId,
            UserId = claim.UserId,
            VerificationAnswer = claim.VerificationAnswer,
            Status = claim.Status,
            CreatedAt = claim.CreatedAt,
            VerifiedAt = claim.VerifiedAt
        };

        return CreatedAtAction(
            nameof(GetClaim),
            new { id = claim.Id },
            result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClaimDto>> GetClaim(int id)
    {
        var claim = await _context.Claims
            .AsNoTracking()
            .Where(claim => claim.Id == id)
            .Select(claim => new ClaimDto
            {
                Id = claim.Id,
                ItemId = claim.ItemId,
                UserId = claim.UserId,
                VerificationAnswer = claim.VerificationAnswer,
                Status = claim.Status,
                CreatedAt = claim.CreatedAt,
                VerifiedAt = claim.VerifiedAt
            })
            .FirstOrDefaultAsync();

        if (claim == null)
        {
            return NotFound(new
            {
                message = "Nie znaleziono roszczenia."
            });
        }

        return Ok(claim);
    }

    [HttpPut("{id}/verify")]
    public async Task<ActionResult<ClaimDto>> VerifyClaim(
        int id,
        VerifyClaimDto dto)
    {
        var claim = await _context.Claims
            .Include(claim => claim.Item)
            .FirstOrDefaultAsync(claim => claim.Id == id);

        if (claim == null)
        {
            return NotFound(new
            {
                message = "Nie znaleziono roszczenia."
            });
        }

        if (claim.Status != "PENDING")
        {
            return BadRequest(new
            {
                message = "To roszczenie zostało już zweryfikowane."
            });
        }

        claim.Status = dto.Approved ? "APPROVED" : "REJECTED";
        claim.VerifiedAt = DateTime.UtcNow;

        if (dto.Approved)
        {
            claim.Item.Status = "RETURNED";
        }

        await _context.SaveChangesAsync();

        var result = new ClaimDto
        {
            Id = claim.Id,
            ItemId = claim.ItemId,
            UserId = claim.UserId,
            VerificationAnswer = claim.VerificationAnswer,
            Status = claim.Status,
            CreatedAt = claim.CreatedAt,
            VerifiedAt = claim.VerifiedAt
        };

        return Ok(result);
    }
}