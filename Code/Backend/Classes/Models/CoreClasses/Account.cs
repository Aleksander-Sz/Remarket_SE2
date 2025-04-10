using System;
using ReMarket.Utilities;

namespace ReMarket.Models
{
//"The Account class represents the user account details."
//"It contains attributes such as username, password, and email"
//"Each account is associated with one or more Web User entities"


    public class Account
    {
        public string  Username { get; private set; }
        public Email Email { get; private set; }
        public string PasswordHash { get; private set; }

        public string Name { get; set; }
        public bool IsVerified { get; private set; } = false;
        public List<WebUser> WebUsers { get; set; } = new List<WebUser>();


        public Account(string name, string username, Email email, string passwordHash)
        {
            Name = name;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
        }
        public static Account Create(string name, string username, Email email, string password)
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



        //Im moving this to AccountServices.cs
       /*
        public void Register()
        {
            Console.WriteLine($"Register user: {Username}");
            // we will add here user to databse
        }

        public bool Login(string inputPassword)
        {
            if (inputPassword == PasswordHash)
            {
                Console.WriteLine($"Loging of a user: {Username}");
                return true;
            }
            else
            {
                Console.WriteLine("Wrong password!");
                return false;
            }
        }*/


    }
}
