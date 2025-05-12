using System;

namespace ReMarket.Models
{
    //"The Web User class is an abstraction for users interacting with the system."
    //"It is associated with a unique loginId"
    //"has a one-to-one relationship with the Wishlist, 
    //which enables users to save listings for future consideration.
    public class WebUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string loginId { get; set; }

        public WebUser(int id, string email, string passwordHash, string loginId)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
            loginId = loginId;
        }

    }
}