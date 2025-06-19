using System;
using Xunit;
using ReMarket.Models;
using MySql.Data.MySqlClient;
using ReMarket.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReMarket.Services;

namespace Tests;

public class CategoryTests : IDisposable
{
    private readonly AppDbContext _db;

    public CategoryTests()
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

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
    public void CreateCategory_ValidData_ReturnsCategory()
    {
        var category = Category.Create("Electronics");
        Assert.Equal("Electronics", category.Name);
        Assert.Equal(0, category.Id);
    }

    [Fact]
    public void SaveToDatabase_NewCategory_SetsId()
    {
        var category = Category.Create("Books");
        _db.Categories.Add(category);
        _db.SaveChanges();

        Assert.True(category.Id > 0);

        _db.Categories.Remove(category);
        _db.SaveChanges();
    }

    [Fact]
    public void SaveToDatabase_ExistingCategory_UpdatesName()
    {
        var category = Category.Create("TempCat");
        _db.Categories.Add(category);
        _db.SaveChanges();

        category.Name = "UpdatedCat";
        _db.Categories.Update(category);
        _db.SaveChanges();

        var updated = _db.Categories.Find(category.Id);
        Assert.NotNull(updated);
        Assert.Equal("UpdatedCat", updated.Name);

        _db.Categories.Remove(updated);
        _db.SaveChanges();
    }

    [Fact]
    public void LoadById_ValidId_ReturnsCategory()
    {
        var category = Category.Create("Gadgets");
        _db.Categories.Add(category);
        _db.SaveChanges();

        var loaded = _db.Categories.Find(category.Id);
        Assert.NotNull(loaded);
        Assert.Equal(category.Id, loaded.Id);
        Assert.Equal("Gadgets", loaded.Name);

        _db.Categories.Remove(loaded);
        _db.SaveChanges();
    }

    [Fact]
    public void LoadById_InvalidId_ReturnsNull()
    {
        var category = _db.Categories.Find(-99);
        Assert.Null(category);
    }

    [Fact]
    public void Delete_ValidId_RemovesCategory()
    {
        var category = Category.Create("ToDelete");
        _db.Categories.Add(category);
        _db.SaveChanges();

        _db.Categories.Remove(category);
        _db.SaveChanges();

        var deleted = _db.Categories.Find(category.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public void LoadAll_ReflectsInsertAndDelete()
    {
        var category = Category.Create("CountTest-" + Guid.NewGuid());
        _db.Categories.Add(category);
        _db.SaveChanges();

        var inserted = _db.Categories.FirstOrDefault(c => c.Id == category.Id);
        Assert.NotNull(inserted);

        _db.Categories.Remove(category);
        _db.SaveChanges();

        var deleted = _db.Categories.FirstOrDefault(c => c.Id == category.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public void SaveToDatabase_EmptyName_AllowsInsertButShouldBeValidated()
    {
        var category = Category.Create("");
        _db.Categories.Add(category);
        _db.SaveChanges();

        Assert.True(category.Id > 0);
        Assert.Equal("", category.Name);

        _db.Categories.Remove(category);
        _db.SaveChanges();
    }
}