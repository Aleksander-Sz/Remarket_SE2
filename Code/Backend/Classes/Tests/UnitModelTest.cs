using System;
using Xunit;
using ReMarket.Models;
using MySql.Data.MySqlClient;
using ReMarket.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReMarket.Services;
public class ListingTests
{
    [Fact]
    public void Listing_ShouldHaveCorrectProperties()
    {
        var listing = new Listing
        {
            Id = 1,
            Title = "Test Listing",
            Price = 100.50m,
            Status = ListingStatus.Active.ToString()
        };


        Assert.Equal(1, listing.Id);
        Assert.Equal("Test Listing", listing.Title);
        Assert.Equal(100.50m, listing.Price);
        Assert.Equal(ListingStatus.Active.ToString(), listing.Status);
    }

}