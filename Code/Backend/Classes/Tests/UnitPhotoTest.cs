using System;
using System.Linq;
using Xunit;
using ReMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using ReMarket.Services;

namespace Tests;

public class PhotoTests : IDisposable
{
    private readonly AppDbContext _db;

    public PhotoTests()
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
    public void CreatePhoto_WithValidBytes_InitializesCorrectly()
    {
        byte[] testBytes = new byte[] { 0x01, 0x02, 0x03 };
        var photo = new Photo(1, testBytes);
        Assert.Equal(1, photo.Id);
        Assert.Equal(testBytes, photo.Bytes);
        Assert.Empty(photo.ListingPhotos);
    }



    [Fact]
    public void SaveToDatabase_NewPhoto_SetsId()
    {
        byte[] testBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

        var photo = new Photo(0, testBytes);
        _db.Photos.Add(photo);
        _db.SaveChanges();

        Assert.True(photo.Id > 0);

        Assert.True(photo.Id > 0);
        _db.Photos.Remove(photo);
        _db.SaveChanges();
    }

    [Fact]
    public void LoadById_ValidId_ReturnsPhoto()
    {
        byte[] testBytes = new byte[] { 0x01, 0x02, 0x03 };
        var photo = new Photo(0, testBytes);
        _db.Photos.Add(photo);
        _db.SaveChanges();
        var loaded = _db.Photos.Find(photo.Id);

        Assert.NotNull(loaded);
        Assert.Equal(photo.Id, loaded.Id);
        Assert.Equal(testBytes, loaded.Bytes);

        _db.Photos.Remove(loaded);
        _db.SaveChanges();
    }

    [Fact]
    public void UpdatePhoto_ChangesArePersisted()
    {
        // Arrange
        byte[] originalBytes = new byte[] { 0x01, 0x02, 0x03 };
        byte[] updatedBytes = new byte[] { 0x04, 0x05, 0x06 };

        var photo = new Photo(0, originalBytes);
        _db.Photos.Add(photo);
        _db.SaveChanges();

        photo.Bytes = updatedBytes;
        _db.Photos.Update(photo);
        _db.SaveChanges();
        var loaded = _db.Photos.Find(photo.Id);
        Assert.Equal(updatedBytes, loaded!.Bytes);
        _db.Photos.Remove(loaded!);
        _db.SaveChanges();
    }

    [Fact]
    public void DeletePhoto_RemovesFromDatabase()
    {

        byte[] testBytes = new byte[] { 0x01, 0x02, 0x03 };
        var photo = new Photo(0, testBytes);
        _db.Photos.Add(photo);
        _db.SaveChanges();


        _db.Photos.Remove(photo);
        _db.SaveChanges();


        var loaded = _db.Photos.Find(photo.Id);
        Assert.Null(loaded);
    }


    [Fact]
    public void Photo_WithEmptyBytes_CanBeCreated()
    {

        byte[] emptyBytes = Array.Empty<byte>();


        var photo = new Photo(1, emptyBytes);
        Assert.NotNull(photo.Bytes);
        Assert.Empty(photo.Bytes);
    }
}