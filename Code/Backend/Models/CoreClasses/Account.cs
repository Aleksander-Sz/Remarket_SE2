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
           