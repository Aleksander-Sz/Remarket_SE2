using System;
using Xunit;
using ReMarket.Models;
using MySql.Data.MySqlClient;
using ReMarket.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReMarket.Services;
public class ListingStatusTests
{
    [Fact]
    public void ActiveListing_CanBeMarkedAsSold()
    {

        var listing = new Listing { Status = ListingStatus.Active.ToString() };
        listing.Status = ListingStatus.Archived.ToString();

        Assert.Equal(ListingStatus.Archived.ToString(), listing.Status);
    }

    [Fact]
    public void DraftListing_CanBeMarkedAsActive()
    {

        var listing = new Listing { Status = ListingStatus.Draft.ToString() };
        listing.Status = ListingStatus.Active.ToString();

        Assert.Equal(ListingStatus.Active.ToString(), listing.Status);
    }

    [Fact]
    public void DraftsCannotBeArchived()
    {

        var listing = new Listing { Status = ListingStatus.Draft.ToString() };
        Assert.Throws<InvalidOperationException>(() =>
            listing.Status = ListingStatus.Archived.ToString());
    }

}