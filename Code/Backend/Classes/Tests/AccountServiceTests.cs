/*using Xunit;
using ReMarket.Models;
using ReMarket.Services;
using ReMarket.Utilities;
using System;
using System.Linq;

namespace ReMarket.Tests
{
    public class AccountServiceTests
    {
        private readonly AccountService _accountService;
        private readonly MockEmailService _emailService = new MockEmailService();

        // Our test data
        private const string TestName = "Jan Kowalski";
        private const string TestUsername = "jankow";
        private const string TestEmail = "jan@example.com";
        private const string ValidPassword = "Password123";

        public AccountServiceTests()
        {
            _accountService = new AccountService(_emailService);
        }

        [Fact]
        public void Register_ValidData_CreatesAccount()
        { //can we register an account

            var result = RegisterTestAccount();


            Assert.True(result.IsSuccess);
            Assert.Equal(TestName, result.Account!.Name);
            Assert.Equal(TestUsername, result.Account.Username);
            Assert.Equal(TestEmail, result.Account.Email.ToString()); 
            Assert.True(PasswordHasher.VerifyPassword(ValidPassword, result.Account.PasswordHash));
        }

        [Fact]
        public void Register_DuplicateEmail_Fails()
        { //can we stop registration of accounts with the same email

            var firstRegistration = RegisterTestAccount();
            Assert.True(firstRegistration.IsSuccess);


            var duplicateResult = _accountService.Register(new RegistrationRequest(
                Name: "Different Name",
                Username: "differentuser",
                Email: TestEmail, 
                Password: ValidPassword,
                ConfirmPassword: ValidPassword
            ));


            Assert.False(duplicateResult.IsSuccess);
            Assert.Contains("already registered", duplicateResult.ErrorMessage!);
        }

        [Theory]
        [InlineData("", "ValidPass123", "Name is required")] //no name
        [InlineData("Jan Kowalski", "short", "at least 8 characters")] //short password
        [InlineData("Jan Kowalski", "lowercaseonly", "uppercase")] // No uppercase
        [InlineData("Jan Kowalski", "UPPERCASEONLY", "lowercase")] // No lowercase
        [InlineData("Jan Kowalski", "NoNumbersHere", "at least one number")] // No numbers
        [InlineData("Jan Kowalski", "Password123", "do not match", "DifferentPassword123")] // Mismatch of passwords
        public void Register_InvalidData_FailsWithMessage(
            string name, string badPassword, string expectedError, string? confirmPassword = null)
        {       //Password validation
            confirmPassword ??= badPassword;

            var result = _accountService.Register(new RegistrationRequest(
                Name: name,
                Username: "testuser",
                Email: "test@example.com",
                Password: badPassword,
                ConfirmPassword: confirmPassword
            ));

            Assert.False(result.IsSuccess);
            Assert.Contains(expectedError, result.ErrorMessage!);
        }

        [Fact]
        public void Register_InvalidEmail_Fails()
        { //check if email is an email

            var result = _accountService.Register(new RegistrationRequest(
                Name: TestName,
                Username: TestUsername,
                Email: "not-an-email",
                Password: ValidPassword,
                ConfirmPassword: ValidPassword
            ));

            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid email", result.ErrorMessage!);
        }

        [Fact]
        public void PasswordHashing_WorksCorrectly()
        { //check password hashing
            var password = "SecurePass123";
            
            var hash = PasswordHasher.HashPassword(password);
            var verificationResult = PasswordHasher.VerifyPassword(password, hash);
            var wrongPasswordResult = PasswordHasher.VerifyPassword("wrongpassword", hash);

            Assert.NotNull(hash);
            Assert.NotEqual(password, hash); 
            Assert.True(verificationResult);
            Assert.False(wrongPasswordResult);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsAccount()
        {//correct login
            var registration = RegisterTestAccount();
            registration.Account!.MarkAsVerified(); 


            var result = _accountService.Login(TestEmail, ValidPassword);
            

            Assert.True(result.IsSuccess);
            Assert.Equal(TestName, result.Account!.Name);
            Assert.Equal(TestEmail, result.Account.Email);
            Assert.True(PasswordHasher.VerifyPassword(ValidPassword, result.Account.PasswordHash));
        }

        [Fact]
        public void Login_WrongPassword_Fails()
        {//wrong password login

            RegisterTestAccount();
            var result = _accountService.Login(TestEmail, "WrongPassword123");
            
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid email or password", result.ErrorMessage);
        }

        [Fact]
        public void Login_BlocksAfter5FailedAttempts()
        {//rate limmiting

            RegisterTestAccount();

            for (int i = 0; i < 5; i++)
            {
                var attempt = _accountService.Login(TestEmail, "wrongpass");
                Assert.False(attempt.IsSuccess);
            }
            
            var blockedResult = _accountService.Login(TestEmail, ValidPassword);
            
            Assert.False(blockedResult.IsSuccess);
            Assert.Contains("Too many attempts", blockedResult.ErrorMessage);
        }

        [Fact]
        public void Login_UnverifiedAccount_Rejects()
        {//unveryfied account cannot log in
            RegisterTestAccount(); 

            var result = _accountService.Login(TestEmail, ValidPassword);
            Assert.False(result.IsSuccess);
            Assert.Equal("Please verify your email first", result.ErrorMessage);
        }

        [Fact]
        public void Login_UnknownAccount_Fails()
        {//account has to be registered
            var result = _accountService.Login(TestEmail, ValidPassword);
            
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid email or password", result.ErrorMessage);
        }

        private RegistrationResult RegisterTestAccount()
        {
            return _accountService.Register(new RegistrationRequest(
                Name: TestName,
                Username: TestUsername,
                Email: TestEmail,
                Password: ValidPassword,
                ConfirmPassword: ValidPassword
            ));
        }

        
    }

    public class MockEmailService : IEmailService
    {
        public void SendConfirmationEmail(string email, string name)
        {
            // May be usefull when we are actually sending emails
            Console.WriteLine($"Mock email sent to {email} for {name}");
        }
    }
}
*/