using ReMarket.Models;
using ReMarket.Utilities;

namespace ReMarket.Services
{
    public class AccountService
    {
        private readonly List<Account> _accounts = new List<Account>();
        private readonly IEmailService _emailService;

        public AccountService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public RegistrationResult Register(RegistrationRequest request)
        {
            try
            {
                if (_accounts.Any(a => a.Email.Value == request.Email))
                    {
                    return RegistrationResult.Failure("Email is already registered");
                    }
                if (request.Password != request.ConfirmPassword)
                    return RegistrationResult.Failure("Passwords do not match");


                var email = new Email(request.Email);
                var account = Account.Create(
                    name: request.Name,
                    username: request.Username,
                    email: email,
                    password: request.Password 
                );

                _accounts.Add(account);
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

    public interface IEmailService
    {
        //this needs to be implemented later
        void SendConfirmationEmail(string email, string name);
    }
}