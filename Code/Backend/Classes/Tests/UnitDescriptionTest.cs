using System;
using Xunit;
using ReMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReMarket.Services;

namespace Tests;

public class DescriptionTests : IDisposable
{
    private readonly AppDbContext _db;

    public DescriptionTests()
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
    public void CreateDescription_ValidData_ReturnsObject()
    {
        var description = Description.Create("Intro", "This is a paragraph.");
        Assert.Equal("Intro", description.Header);
        Assert.Equal("This is a paragraph.", description.Paragraph);
    }

    [Fact]
    public void CreateDescription_WithoutParagraph_ParagraphIsNull()
    {
        var description = Description.Create("OnlyHeader");
        Assert.Equal("OnlyHeader", description.Header);
        Assert.Null(description.Paragraph);
    }

    [Fact]
    public void SaveToDatabase_NewDescription_SetsId()
    {
        var description = Description.Create("Header", "Body");
        _db.Descriptions.Add(description);
        _db.SaveChanges();

        Assert.True(description.Id > 0);

        _db.Descriptions.Remove(description);
        _db.SaveChanges();
    }

    [Fact]
    public void SaveToDatabase_MissingHeader_ThrowsValidationException()
    {
        var description = new Description { Paragraph = "Some paragraph" };

        _db.Descriptions.Add(description);
        var exception = Record.Exception(() => _db.SaveChanges());

        Assert.NotNull(exception);

        _db.ChangeTracker.Clear();
    }

    [Fact]
    public void SaveToDatabase_TooLongHeader_ThrowsException()
    {
        string longHeader = new string('A', 300);
        var description = Description.Create(longHeader, "Valid paragraph");

        _db.Descriptions.Add(description);
        var exception = Record.Exception(() => _db.SaveChanges());

        Assert.NotNull(exception);

        _db.ChangeTracker.Clear();
    }

    [Fact]
    public void LoadById_ValidId_ReturnsDescription()
    {
        var description = Description.Create("Header", "Body");
        _db.Descriptions.Add(description);
        _db.SaveChanges();

        var loaded = _db.Descriptions.Find(description.Id);
        Assert.NotNull(loaded);
        Assert.Equal(description.Header, loaded.Header);
        Assert.Equal(description.Paragraph, loaded.Paragraph);

        _db.Descriptions.Remove(loaded);
        _db.SaveChanges();
    }

    [Fact]
    public void LoadById_InvalidId_ReturnsNull()
    {
        var loaded = _db.Descriptions.Find(-1);
        Assert.Null(loaded);
    }

    [Fact]
    public void Update_ChangesArePersisted()
    {
        var description = Description.Create("Original", "Initial paragraph");
        _db.Descriptions.Add(description);
        _db.SaveChanges();

        description.Header = "Updated";
        description.Paragraph = "New paragraph";
        _db.Descriptions.Update(description);
        _db.SaveChanges();

        var loaded = _db.Descriptions.Find(description.Id);
        Assert.Equal("Updated", loaded?.Header);
        Assert.Equal("New paragraph", loaded?.Paragraph);

        _db.Descriptions.Remove(loaded!);
        _db.SaveChanges();
    }

    [Fact]
    public void Delete_RemovesFromDatabase()
    {
        var description = Description.Create("To Delete", "Some text");
        _db.Descriptions.Add(description);
        _db.SaveChanges();

        _db.Descriptions.Remove(description);
        _db.SaveChanges();

        var loaded = _db.Descriptions.Find(description.Id);
        Assert.Null(loaded);
    }

    [Fact]
    public void DescriptionEquality_CompareProperties_NotReferenceEqual()
    {
        var d1 = Description.Create("Same", "Text");
        var d2 = Description.Create("Same", "Text");

        Assert.NotSame(d1, d2);
        Assert.Equal(d1.Header, d2.Header);
        Assert.Equal(d1.Paragraph, d2.Paragraph);
    }
}