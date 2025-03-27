using System;

namespace ReMarket.Models
{
//"The Account class represents the user account details."
//"It contains attributes such as username, password, and email"
//"Each account is associated with one or more Web User entities"


    public class Account
    {
        public string username { get; set; }
        public string password { get; set; }
        public Email email { get; set; }
        public string PasswordHash { get; set; }

    public Account(string username, string password, Email email)
    {
        this.username = username;
        this.password = password;
        this.email = email;
    }
        
                public void Register()
        {
            Console.WriteLine($"Register user: {Username}");
            // we will add here user to databse
        }

        public bool Login(string inputPassword)
        {
            if (inputPassword == PasswordHash)
            {
                Console.WriteLine($"Logowanie udane dla użytkownika: {Username}");
                return true;
            }
            else
            {
                Console.WriteLine("Błędne hasło!");
                return false;
            }
        }


    }

   
}
