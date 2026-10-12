using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Znajdek.Api.Controllers;
using Znajdek.Api.Data;
using Znajdek.Api.DTOs;
using Znajdek.Api.Models;

namespace Znajdek.Api.Tests;

public class ClaimsControllerTests
{
    [Fact]
    public async Task VerifyClaim_Approved_ChangesClaimAndItemStatus()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new ApplicationDbContext(options);

        var user = new User
        {
            Id = 1,
            Name = "Test User",
            Email = "test@example.com",
            Role = "User"
        };

        var item = new Item
        {
            Id = 1,
            Title = "Czarny portfel",
            Description = "Portfel znaleziony przy wejściu.",
            Type = "FOUND",
            Category = "Portfel",
            Location = "Budynek A",
            Date = DateTime.UtcNow,
            Status = "ACTIVE",
            CreatedById = 1
        };

        var claim = new Claim
        {
            Id = 1,
            ItemId = 1,
            UserId = 1,
            VerificationAnswer = "Mała naklejka w środku.",
            Status = "PENDING",
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(user);
        context.Items.Add(item);
        context.Claims.Add(claim);

        await context.SaveChangesAsync();

        var controller = new ClaimsController(context);

        var dto = new VerifyClaimDto
        {
            Approved = true
        };

        // Act
        var result = await controller.VerifyClaim(1, dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedClaim = Assert.IsType<ClaimDto>(okResult.Value);

        Assert.Equal("APPROVED", returnedClaim.Status);

        var savedClaim = await context.Claims.FindAsync(1);
        var savedItem = await context.Items.FindAsync(1);

        Assert.Equal("APPROVED", savedClaim!.Status);
        Assert.Equal("RETURNED", savedItem!.Status);
        Assert.NotNull(savedClaim.VerifiedAt);
    }
    [Fact]
public async Task VerifyClaim_Rejected_KeepsItemActive()
{
    // Arrange
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    await using var context = new ApplicationDbContext(options);

    var user = new User
    {
        Id = 1,
        Name = "Test User",
        Email = "test@example.com",
        Role = "User"
    };

    var item = new Item
    {
        Id = 1,
        Title = "Czarny plecak",
        Description = "Plecak znaleziony w budynku.",
        Type = "FOUND",
        Category = "Plecak",
        Location = "Budynek A",
        Date = DateTime.UtcNow,
        Status = "ACTIVE",
        CreatedById = 1
    };

    var claim = new Claim
    {
        Id = 1,
        ItemId = 1,
        UserId = 1,
        VerificationAnswer = "To jest informacja testowa.",
        Status = "PENDING",
        CreatedAt = DateTime.UtcNow
    };

    context.Users.Add(user);
    context.Items.Add(item);
    context.Claims.Add(claim);

    await context.SaveChangesAsync();

    var controller = new ClaimsController(context);

    var dto = new VerifyClaimDto
    {
        Approved = false
    };

    // Act
    var result = await controller.VerifyClaim(1, dto);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var returnedClaim = Assert.IsType<ClaimDto>(okResult.Value);

    Assert.Equal("REJECTED", returnedClaim.Status);

    var savedClaim = await context.Claims.FindAsync(1);
    var savedItem = await context.Items.FindAsync(1);

    Assert.Equal("REJECTED", savedClaim!.Status);
    Assert.Equal("ACTIVE", savedItem!.Status);
    Assert.NotNull(savedClaim.VerifiedAt);
}
[Fact]
public async Task VerifyClaim_AlreadyVerified_ReturnsBadRequest()
{
    // Arrange
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    await using var context = new ApplicationDbContext(options);

    var user = new User
    {
        Id = 1,
        Name = "Test User",
        Email = "test@example.com",
        Role = "User"
    };

    var item = new Item
    {
        Id = 1,
        Title = "Czarny portfel",
        Description = "Portfel znaleziony przy wejściu.",
        Type = "FOUND",
        Category = "Portfel",
        Location = "Budynek A",
        Date = DateTime.UtcNow,
        Status = "RETURNED",
        CreatedById = 1
    };

    var claim = new Claim
    {
        Id = 1,
        ItemId = 1,
        UserId = 1,
        VerificationAnswer = "Informacja testowa.",
        Status = "APPROVED",
        CreatedAt = DateTime.UtcNow,
        VerifiedAt = DateTime.UtcNow
    };

    context.Users.Add(user);
    context.Items.Add(item);
    context.Claims.Add(claim);

    await context.SaveChangesAsync();

    var controller = new ClaimsController(context);

    var dto = new VerifyClaimDto
    {
        Approved = true
    };

    // Act
    var result = await controller.VerifyClaim(1, dto);

    // Assert
    var badRequestResult = Assert.IsType<BadRequestObjectResult>(
        result.Result);

    Assert.NotNull(badRequestResult.Value);

    var savedClaim = await context.Claims.FindAsync(1);
    var savedItem = await context.Items.FindAsync(1);

    Assert.Equal("APPROVED", savedClaim!.Status);
    Assert.Equal("RETURNED", savedItem!.Status);
}
[Fact]
public async Task CreateClaim_ValidData_CreatesPendingClaim()
{
    // Arrange
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    await using var context = new ApplicationDbContext(options);

    var user = new User
    {
        Id = 1,
        Name = "Test User",
        Email = "test@example.com",
        Role = "User"
    };

    var item = new Item
    {
        Id = 1,
        Title = "Znaleziony telefon",
        Description = "Telefon znaleziony w budynku.",
        Type = "FOUND",
        Category = "Telefon",
        Location = "Budynek A",
        Date = DateTime.UtcNow,
        Status = "ACTIVE",
        CreatedById = 1
    };

    context.Users.Add(user);
    context.Items.Add(item);

    await context.SaveChangesAsync();

    var controller = new ClaimsController(context);

    var dto = new CreateClaimDto
    {
        ItemId = 1,
        UserId = 1,
        VerificationAnswer = "Na telefonie jest charakterystyczna rysa."
    };

    // Act
    var result = await controller.CreateClaim(dto);

    // Assert
    var createdResult = Assert.IsType<CreatedAtActionResult>(
        result.Result);

    var returnedClaim = Assert.IsType<ClaimDto>(
        createdResult.Value);

    Assert.Equal("PENDING", returnedClaim.Status);
    Assert.Equal(1, returnedClaim.ItemId);
    Assert.Equal(1, returnedClaim.UserId);

    var savedClaim = await context.Claims.FindAsync(returnedClaim.Id);

    Assert.NotNull(savedClaim);
    Assert.Equal("PENDING", savedClaim.Status);
    Assert.Equal(
        "Na telefonie jest charakterystyczna rysa.",
        savedClaim.VerificationAnswer);
}
[Fact]
public async Task CreateClaim_ItemDoesNotExist_ReturnsBadRequest()
{
    // Arrange
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    await using var context = new ApplicationDbContext(options);

    var user = new User
    {
        Id = 1,
        Name = "Test User",
        Email = "test@example.com",
        Role = "User"
    };

    context.Users.Add(user);
    await context.SaveChangesAsync();

    var controller = new ClaimsController(context);

    var dto = new CreateClaimDto
    {
        ItemId = 999,
        UserId = 1,
        VerificationAnswer = "Informacja testowa."
    };

    // Act
    var result = await controller.CreateClaim(dto);

    // Assert
    var badRequestResult = Assert.IsType<BadRequestObjectResult>(
        result.Result);

    Assert.NotNull(badRequestResult.Value);

    var claimsCount = await context.Claims.CountAsync();

    Assert.Equal(0, claimsCount);
}
}