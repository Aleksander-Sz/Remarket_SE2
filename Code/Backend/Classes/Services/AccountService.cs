using ReMarket.Models;
using ReMarket.Utilities;

using MySql.Data.MySqlClient;
using System.Data;


namespace ReMarket.Services
{
    public class AccountService
    {
        private readonly List<Account> _accounts = new List<Account>();
        private readonly Dictionary<string, int> _failedAttempts = new();
        private readonly IEmailService _emailService;

        private readonly string _connectionString = "server=localhost;user=root;password=yourpassword;database=remarket;";
        private readonly bool _useDatabase;


        public AccountService(IEmailService emailService)
        {
            _emailService = emailService;
            _useDatabase = false;
        }

        public AccountService(IEmailService emailService, string connectionString)
        {
            _emailService = emailService;
            _connectionString = connectionString;
            _useDatabase = true;
        }

        public RegistrationResult Register(RegistrationRequest request)
        {
            try
            {
                if (request.Password != request.ConfirmPassword)
                    return RegistrationResult.Failure("Passwords do not match");

                var email = new Email(request.Email);

                Account? existing = null;
                if (_useDatabase)
                {
                    using var connection = new MySqlConnection(_connectionString);
                    connection.Open();
                    existing = Account.LoadByEmail(connection, request.Email);
                }
                else
                {
                    existing = _accounts.FirstOrDefault(a => a.Email.Value == request.Email);
                }

                if (existing != null)
                    return RegistrationResult.Failure("Email is already registered");

                var account = Account.Create(
                    name: request.Name,
                    username: request.Username,
                    email: email,
                    password: request.Password
                );

                if (_useDatabase)
                {
                    using var connection = new MySqlConnection(_connectionString);
                    connection.Open();
                    account.SaveToDatabase(connection);
                }
                else
                {
                    _accounts.Add(account);
                }

                _emailService.SendConfirmationEmail(account.Email.ToString(), account.Name);
                return RegistrationResult.Success(account);
            }
            catch (ArgumentException ex)
            {
                return RegistrationResult.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return RegistrationResult.Failure("Registration failed: " + ex.Message);
            }
        }

        public LoginResult Login(string email, string password)
        {
            if (_failedAttempts.TryGetValue(email, out int attempts) && attempts >= 5)
                return LoginResult.Failure("Too many attempts. Try again later.");

            Account? account = null;

            if (_useDatabase)
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();
                account = Account.LoadByEmail(connection, email);
            }
            else
            {
                account = _accounts.FirstOrDefault(a => a.Email.Value == email);
            }

            if (account == null || !account.VerifyPassword(password))
            {
                _failedAttempts[email] = _failedAttempts.GetValueOrDefault(email, 0) + 1;
                return LoginResult.Failure("Email or password is wrong");
            }

            if (!account.IsVerified)
            {
                return LoginResult.Failure("Please verify your email first");
            }

            _failedAttempts.Remove(email);
            return LoginResult.Success(account);

        }
    }

    public record RegistrationRequest(
        string Name,
        string Username,
        string Email,
        string Password,
        string ConfirmPassword
    );

    public record RegistrationResult(bool IsSuccess, Account? Account, string? ErrorMessage)
    {
        public static RegistrationResult Success(Account account)
            => new(true, account, null);

        public static RegistrationResult Failure(string error)
            => new(false, null, error);
    }

    public record LoginResult(bool IsSuccess, Account? Account, string? ErrorMessage)
    {
        public static LoginResult Success(Account account) => new(true, account, null);
        public static LoginResult Failure(string error) => new(false, null, error);
    }

    public interface IEmailService
    {
        //this needs to be implemented later
        void SendConfirmationEmail(string email, string name);
    }

    public class DummyEmailService : IEmailService
    {
        public void SendConfirmationEmail(string email, string name)
        {
            // No-op for now
        }
    }


}