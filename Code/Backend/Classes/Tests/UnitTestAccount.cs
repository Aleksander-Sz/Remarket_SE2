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
        //do NOT push the password or u will be crying like me just now
        /*       private string connectionString = new MySqlConnectionStringBuilder()
               {
                   Server = "remarket-se2project-ania-f1cd.j.aivencloud.com",
                   Port = 21633,
                   Database = "ReMarket",
                   UserID = "avnadmin",
                   Password = "password,
                   SslMode = MySqlSslMode.VerifyCA,
                   CertificateFile = "/Users/ola/desktop/ca.pem"
               }.ConnectionString;


               [Fact]
               public void SaveToDatabase_ValidAccount_SavesAccount()
               {
                   using var connection = new MySqlConnection(connectionString);
                   connection.Open();

                   var account = new Account("testuser", "testuser@example.com", "TestPassword123");

                   account.SaveToDatabase(connection);

                   Assert.True(account.Id > 0);
               }

               [Fact]
               public void LoadByEmail_ValidEmail_ReturnsAccount()
               {
                   using var connection = new MySqlConnection(connectionString);
                   connection.Open();

                   var account = Account.LoadByEmail(connection, "testuser@example.com");

                   Assert.NotNull(account);
                   Assert.Equal("testuser@example.com", account.Email);
               }

               [Fact]
               public void LoadByEmail_InvalidEmail_ReturnsNull()
               {
                   using var connection = new MySqlConnection(connectionString);
                   connection.Open();

                   var account = Account.LoadByEmail(connection, "nonexistentuser@example.com");

                   Assert.Null(account);
               }
           }*/
    }
}

