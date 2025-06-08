
using System;
using Xunit;
using ReMarket.Models;
using MySql.Data.MySqlClient;
using System.Data;
using ReMarket.Utilities;
public class PaginationTests
{
    [Fact]
    public void Paginate_ShouldReturnCorrectPage()
    {
        // Arrange
        var data = Enumerable.Range(1, 100).Select(i => new Listing { Id = i }).ToList();

        // Act
        var page = data.Skip(20).Take(10).ToList(); // Page 3 (0-indexed)

        // Assert
        Assert.Equal(10, page.Count);
        Assert.Equal(21, page.First().Id);
        Assert.Equal(30, page.Last().Id);
    }
}