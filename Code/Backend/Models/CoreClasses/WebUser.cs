using System;

namespace ReMarket.Models
{
    //"The Web User class is an abstraction for users interacting with the system."
    //"It is associated with a unique loginId"
    //"has a one-to-one relationship with the Wishlist, 
    //which enables users to save listings for future consideration.
    public abstract class WebUser
    {
        public string loginId { get; set; }

        public WebUser(string loginID)
        {
            this.loginId=loginID;
        }
    }
}