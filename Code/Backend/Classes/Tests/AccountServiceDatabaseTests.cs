using Xunit;
using ReMarket.Models;
using ReMarket.Services;
using ReMarket.Utilities;
using MySql.Data.MySqlClient;
using System;

namespace ReMarket.Tests.Database
{
    public class AccountServiceDatabaseTests : IDisposable
    {
        private readonly AccountService _accountService;
        private readonly MockEmailService _emailService = new MockEmailService();
        private readonly string _connectionString;
        private readonly string _testDatabaseName = "remarket_test";

        // Test data
        private const string TestName = "Database Test User";
        private const string TestUsername = "dbuser";
        private const string TestEmail = "dbuser@example.com";
        private const string ValidPassword = "Password123";

        public AccountServiceDatabaseTests()
        {
            // YOUR password instead of your_password if u want to run it
            var baseConnectionString = "server=localhost;user=root;password=your_password;";


            using (var connection = new MySqlConnection(baseConnectionString))
            {
                connection.Open();
                var createDbCommand = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS {_testDatabaseName}", connection);
                createDbCommand.ExecuteNonQuery();
            }


            _connectionString = $"{baseConnectionString}database={_testDatabaseName};";
            _accountService = new AccountService(_emailService, _connectionString);
            InitializeTestDatabase();
        }

        private void InitializeTestDatabase()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var createTableCommand = new MySqlCommand(@"
                    CREATE TABLE IF NOT EXISTS Accounts (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Username VARCHAR(255) NOT NULL,
                        Email VARCHAR(255) NOT NULL UNIQUE,
                        PasswordHash VARCHAR(255) NOT NULL,
                        Name VARCHAR(255) NOT NULL,
                        IsVerified BOOLEAN NOT NULL DEFAULT FALSE
                    )", connection);

                createTableCommand.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var dropDbCommand = new MySqlCommand($"DROP DATABASE IF EXISTS {_testDatabaseName}", connection);
                dropDbCommand.ExecuteNonQuery();
            }
        }

        [Fact]
        public void Register_WithDatabase_CreatesAccountInDatabase()
        { //registering adds to database
            var result = _accountService.Register(new RegistrationRequest(
                Name: TestName,
                Username: TestUsername,
                Email: TestEmail,
                Password: ValidPassword,
                ConfirmPassword: ValidPassword
            ));

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Account);
            Assert.True(result.Account.Id > 0);

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var dbAccount = Account.LoadByEmail(connection, TestEmail);

                Assert.NotNull(dbAccount);
                Assert.Equal(TestName, dbAccount.Name);
                Assert.Equal(TestUsername, dbAccount.Username);
                Assert.Equal(TestEmail, dbAccount.Email.Value);
                Assert.True(PasswordHasher.VerifyPassword(ValidPassword, dbAccount.PasswordHash));
            }
        }

        [Fact]
        public void Login_WithDatabase_AuthenticatesSuccessfully()
        {//login works

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var account = Account.Create(TestName, TestUsername, new Email(TestEmail), ValidPassword);
                account.MarkAsVerified();
                account.SaveToDatabase(connection);
            }

            var result = _accountService.Login(TestEmail, ValidPassword);

            Assert.True(result.IsSuccess);
            Assert.Equal(TestName, result.Account!.Name);
            Assert.Equal(TestEmail, result.Account!.Email.Value);
        }

        [Fact]
        public void Register_DuplicateEmailInDatabase_Fails()
        {//emails in database are unique

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var account = Account.Create(TestName, TestUsername, new Email(TestEmail), ValidPassword);
                account.SaveToDatabase(connection);
            }

            var result = _accountService.Register(new RegistrationRequest(
                Name: "Different Name",
                Username: "differentuser",
                Email: TestEmail,
                Password: ValidPassword,
                ConfirmPassword: ValidPassword
            ));

            Assert.False(result.IsSuccess);
            Assert.Contains("already registered", result.ErrorMessage);
        }

        [Fact]
        public void Login_WithDatabase_BlocksAfter5FailedAttempts()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var account = Account.Create(TestName, TestUsername, new Email(TestEmail), ValidPassword);
                account.MarkAsVerified();
                account.SaveToDatabase(connection);
            }

            for (int i = 0; i < 5; i++)
            {
                var attempt = _accountService.Login(TestEmail, "wrongpassword");
                Assert.False(attempt.IsSuccess);
            }

            var blockedResult = _accountService.Login(TestEmail, ValidPassword);

            Assert.False(blockedResult.IsSuccess);
            Assert.Contains("Too many attempts", blockedResult.ErrorMessage);
        }

        [Fact]
        public void LoadByEmail_ReturnsNullForNonexistentEmail()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var account = Account.LoadByEmail(connection, "nonexistent@example.com");
                Assert.Null(account);
            }
        }
    }
}