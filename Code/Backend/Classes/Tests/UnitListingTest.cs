using System;
using System.Linq;
using Xunit;
using ReMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReMarket.Services;

namespace Tests;

public class ListingTests : IDisposable
{
    private readonly AppDbContext _db;

    public ListingTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .Options;

        _db = new AppDbContext(options);
    }

    public void Dispose()
    {
        _db.Dispose();
    }

    [Fact]
    public void CreateListing_ValidFields_SetsDefaults()
    {
        var listing = new Listing
        {
            Title = "iPhone X",
            Price = 499.99m,
            Status = "Draft",
            Category = Category.Create("Phones"),
            Description = Description.Create("Phone", "Good condition"),
        };

        Assert.Equal("Draft", listing.Status);
        Assert.Equal(499.99m, listing.Price);
        Assert.Equal("iPhone X", listing.Title);
    }

    [Fact]
    public void Listing_StatusTransition_DraftToArchived_Throws()
    {
        var listing = new Listing
        {
            Title = "MacBook",
            Price = 899.99m,
            Status = "Draft",
            Category = Category.Create("Laptops"),
            Description = Description.Create("Laptop", "Lightly used"),
        };

        Assert.Throws<InvalidOperationException>(() => listing.Status = "Archived");
    }

    [Fact]
    public void SaveToDatabase_NewListing_SetsId()
    {
        var category = Category.Create("Electronics");
        var description = Description.Create("TV", "4K OLED");

        _db.Categories.Add(category);
        _db.Descriptions.Add(description);
        _db.SaveChanges();

        var listing = new Listing
        {
            Title = "LG OLED TV",
            Price = 1099.99m,
            CategoryId = category.Id,
            DescriptionId = description.Id,
            Status = "Active"
        };

        _db.Listings.Add(listing);
        _db.SaveChanges();

        Assert.True(listing.Id > 0);

        _db.Listings.Remove(listing);
        _db.Categories.Remove(category);
        _db.Descriptions.Remove(description);
        _db.SaveChanges();
    }

    [Fact]
    public void SaveToDatabase_TooHighPrice_PrecisionMaintained()
    {
        var category = Category.Create("Luxury");
        var description = Description.Create("Watch", "Luxury timepiece");

        _db.Categories.Add(category);
        _db.Descriptions.Add(description);
        _db.SaveChanges();

        var listing = new Listing
        {
            Title = "Rolex",
            Price = 12345678.90m,
            CategoryId = category.Id,
            DescriptionId = description.Id,
            Status = "Active"
        };

        _db.Listings.Add(listing);
        _db.SaveChanges();

        var loaded = _db.Listings.Find(listing.Id);
        Assert.Equal(12345678.90m, loaded?.Price);

        _db.Listings.Remove(loaded!);
        _db.Categories.Remove(category);
        _db.Descriptions.Remove(description);
        _db.SaveChanges();
    }

    [Fact]
    public void LoadById_ValidId_ReturnsListing()
    {
        var category = Category.Create("Appliances");
        var description = Description.Create("Fridge", "Large capacity");

        _db.Categories.Add(category);
        _db.Descriptions.Add(description);
        _db.SaveChanges();

        var listing = new Listing
        {
            Title = "Samsung Fridge",
            Price = 899.99m,
            CategoryId = category.Id,
            DescriptionId = description.Id,
            Status = "Active"
        };

        _db.Listings.Add(listing);
        _db.SaveChanges();

        var loaded = _db.Listings.Find(listing.Id);
        Assert.NotNull(loaded);
        Assert.Equal("Samsung Fridge", loaded?.Title);

        _db.Listings.Remove(loaded!);
        _db.Categories.Remove(category);
        _db.Descriptions.Remove(description);
        _db.SaveChanges();
    }

    [Fact]
    public void Delete_RemovesListing()
    {
        var category = Category.Create("Furniture");
        var description = Description.Create("Chair", "Comfortable");

        _db.Categories.Add(category);
        _db.Descriptions.Add(description);
        _db.SaveChanges();

        var listing = new Listing
        {
            Title = "Office Chair",
            Price = 149.99m,
            CategoryId = category.Id,
            DescriptionId = description.Id,
            Status = "Draft"
        };

        _db.Listings.Add(listing);
        _db.SaveChanges();

        _db.Listings.Remove(listing);
        _db.SaveChanges();

        var loaded = _db.Listings.Find(listing.Id);
        Assert.Null(loaded);

        _db.Categories.Remove(category);
        _db.Descriptions.Remove(description);
        _db.SaveChanges();
    }
}