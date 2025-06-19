
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

        var data = Enumerable.Range(1, 100).Select(i => new Listing { Id = i }).ToList();


        var page = data.Skip(20).Take(10).ToList();

        Assert.Equal(10, page.Count);
        Assert.Equal(21, page.First().Id);
        Assert.Equal(30, page.Last().Id);
    }
}