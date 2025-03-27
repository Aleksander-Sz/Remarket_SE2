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

    public Account(string username, string password, Email email)
    {
        this.username = username;
        this.password = password;
        this.email = email;
    }
        
        public void Register() { throw new NotImplementedException(); }
        public void Login() { throw new NotImplementedException(); }


    }

   
}