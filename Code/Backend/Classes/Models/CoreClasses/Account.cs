using System;
using MySql.Data.MySqlClient;
using System.Data;
using ReMarket.Utilities;
namespace ReMarket.Models
{
    //"The Account class represents the user account details."
    //"It contains attributes such as username, password, and email"
    //"Each account is associated with one or more Web User entities"

    //IS VERYFIED AND NAME NEED TO BE ADDED TO THE DATABASE! 
    //PHOTO FOREIGN KEY IS NOT IMPLEMENTED YET

    public class Account
    {
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        public string Name { get; set; }
        public bool IsVerified { get; private set; } = false;
        public List<WebUser> WebUsers { get; set; } = new List<WebUser>();
        public int Id { get; private set; }


        public Account(string name, string username, string email, string passwordHash)
        {
            Name = name;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
        }
        public static Account Create(string name, string username, string email, string password)
        {
            ValidateInputs(name, username, password);

            return new Account(name, username, email, PasswordHasher.HashPassword(password))
            {
                Name = name,
                Username = username,
                Email = email,
                PasswordHash = PasswordHasher.HashPassword(password)
            };
        }

        private static void ValidateInputs(string name, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required");

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required");

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters");

            if (!password.Any(char.IsUpper) || !password.Any(char.IsLower))
                throw new ArgumentException("Password must contain uppercase and lowercase letters");

            if (!password.Any(char.IsDigit))
                throw new ArgumentException("Password must contain at least one number");
            if (!password.Any(char.IsDigit))
                throw new ArgumentException("Password must contain at least one number");
        }
        public bool VerifyPassword(string inputPassword)
        {
            return PasswordHasher.VerifyPassword(inputPassword, PasswordHash);
        }
        //this is to be done after email is confirmed
        public void MarkAsVerified() => IsVerified = true;


        public void SaveToDatabase(MySqlConnection connection)
        {
            using var command = new MySqlCommand(@"
                INSERT INTO Accounts (Username, Email, PasswordHash, Name, IsVerified)
                VALUES (@Username, @Email, @PasswordHash, @Name, @IsVerified);
                SELECT LAST_INSERT_ID();", connection);

            command.Parameters.AddWithValue("@Username", Username);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@PasswordHash", PasswordHash);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@IsVerified", IsVerified);

            Id = Convert.ToInt32(command.ExecuteScalar());
        }

        public static Account? LoadByEmail(MySqlConnection connection, string email)
        {
            using var command = new MySqlCommand(@"
                SELECT Id, Username, Email, PasswordHash, Name, IsVerified
                FROM Accounts
                WHERE Email = @Email", connection);

            command.Parameters.AddWithValue("@Email", email);

            using var reader = command.ExecuteReader();

            if (!reader.Read()) return null;

            var account = new Account(
                name: reader.GetString("Name"),
                username: reader.GetString("Username"),
                email: reader.GetString("Email"),
                passwordHash: reader.GetString("PasswordHash")
            )
            {
                Id = reader.GetInt32("Id"),
                IsVerified = reader.GetBoolean("IsVerified")
            };

            return account;
        }
    }


}

