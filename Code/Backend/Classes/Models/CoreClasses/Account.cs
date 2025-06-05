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
        public string Password { get; private set; }
        public int Id { get; private set; }
        public char Role { get; private set; } = 'U';


        public Account(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }


        public static Account Create(string username, string email, string password)
        {
            ValidateInputs(username, password);
            string hashedPassword = PasswordHasher.HashPassword(password);
            return new Account(username, email, hashedPassword);
        }


        private static void ValidateInputs(string username, string password)
        {

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
            return PasswordHasher.VerifyPassword(inputPassword, Password);
        }


        public void SaveToDatabase(MySqlConnection connection)
        {
            using var command = new MySqlCommand(@"
                INSERT INTO Accounts (Username, Email, PasswordHash, Name, IsVerified)
                VALUES (@Username, @Email, @PasswordHash, @Name, @IsVerified);
                SELECT LAST_INSERT_ID();", connection);

            command.Parameters.AddWithValue("@Username", Username);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@PasswordHash", Password);

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
                username: reader.GetString("Username"),
                email: reader.GetString("Email"),
                password: reader.GetString("PasswordHash")
            )
            {
                Id = reader.GetInt32("Id"),
            };

            return account;
        }
    }


}

