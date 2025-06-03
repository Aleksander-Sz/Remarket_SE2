using System;
using Xunit;
using ReMarket.Models;
using MySql.Data.MySqlClient;
using System.Data;
using ReMarket.Utilities;

namespace Tests;

public class UnitTestAccount
{
    public class AccountTests
    {
        [Fact]
        public void CreateAccount_ValidData_ReturnsAccount()
        {
            string username = "testuser";
            string email = "testuser@example.com";
            string password = "TestPassword123";

            var account = Account.Create(username, email, password);

            Assert.Equal(username, account.Username);
            Assert.Equal(email, account.Email);
            Assert.True(PasswordHasher.VerifyPassword(password, account.Password));
        }

        [Fact]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            string plainPassword = "Test123!";
            var account = Account.Create("user", "user@example.com", plainPassword);

            Assert.True(account.VerifyPassword(plainPassword));
        }

        [Fact]
        public void VerifyPassword_InvalidPassword_ReturnsFalse()
        {
            string plainPassword = "Wrong!";
            string hashedPassword = PasswordHasher.HashPassword("Right");

            bool result = PasswordHasher.VerifyPassword(plainPassword, hashedPassword);

            Assert.False(result);
        }

        //invalid create account fail
        [Theory]
        [InlineData("", "TestPassword123", "Username is required")]
        [InlineData("user", "", "Password must be at least 8 characters")]
        [InlineData("user", "short1", "Password must be at least 8 characters")]
        [InlineData("user", "alllowercase1", "Password must contain uppercase and lowercase letters")]
        [InlineData("user", "ALLUPPERCASE1", "Password must contain uppercase and lowercase letters")]
        [InlineData("user", "NoNumberPassword", "Password must contain at least one number")]
        public void CreateAccount_InvalidData_ThrowsArgumentException(string username, string password, string expectedErrorMessage)
        {
            var ex = Assert.Throws<ArgumentException>(() => Account.Create(username, "test@example.com", password));
            Assert.Contains(expectedErrorMessage, ex.Message);
        }

        //is password auto hashed
        [Fact]
        public void Create_ShouldHashPassword()
        {
            string password = "TestPass123";
            var account = Account.Create("user", "user@example.com", password);

            Assert.NotEqual(password, account.Password);
        }

        [Fact]
        public void Constructor_StoresPlainPassword_Dangerous()
        {
            string password = "PlainPass123";
            var account = new Account("user", "user@example.com", password);

            Assert.Equal(password, account.Password);
        }





        // DATA BASE TESTING, will be added later
        //[Fact]
        //public void SaveToDatabase_ValidAccount_SavesAccount()
        //{

        //    var connection = new MySqlConnection("server=localhost;port=3306;database=ReMarket;user=root;password=toor1234");
        //    connection.Open();

        //    var account = new Account("testuser", "testuser@example.com", "TestPassword123");


        //    account.SaveToDatabase(connection);

        //    Assert.True(account.Id > 0);

        //    connection.Close();
        //}

        //    // Testowanie ładowania konta po emailu
        //[Fact]
        //public void LoadByEmail_ValidEmail_ReturnsAccount()
        //{
        //    // Arrange
        //    var connection = new MySqlConnection("server=localhost;port=3306;database=ReMarket;user=root;password=toor1234");
        //    connection.Open();

        //    // Act
        //    var account = Account.LoadByEmail(connection, "testuser@example.com");

        //    // Assert
        //    Assert.NotNull(account);

        //    connection.Close();
        //}

        //    [Fact]
        //    public void LoadByEmail_InvalidEmail_ReturnsNull()
        //    {
        //        // Arrange
        //        var connection = new MySqlConnection("server=localhost;port=3306;database=ReMarket;user=root;password=toor1234");
        //        connection.Open();

        //        // Act
        //        var account = Account.LoadByEmail(connection, "nonexistentuser@example.com");

        //        // Assert
        //        Assert.Null(account);

        //        connection.Close();
        //    }
    }
}
