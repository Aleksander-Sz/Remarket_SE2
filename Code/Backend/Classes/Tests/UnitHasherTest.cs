using System;
using Xunit;
using ReMarket.Models;
using MySql.Data.MySqlClient;
using ReMarket.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReMarket.Services;
public class PasswordHasherTests
{
    [Fact]
    public void HashPassword_ShouldReturnDifferentHashEachTime()
    {

        const string password = "securePassword123";


        var hash1 = BCrypt.Net.BCrypt.HashPassword(password);
        var hash2 = BCrypt.Net.BCrypt.HashPassword(password);


        Assert.NotEqual(hash1, hash2);
        Assert.True(BCrypt.Net.BCrypt.Verify(password, hash1));
        Assert.True(BCrypt.Net.BCrypt.Verify(password, hash2));
    }
}