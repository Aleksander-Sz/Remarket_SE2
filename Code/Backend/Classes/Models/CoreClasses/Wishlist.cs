using System;
using System.Collections.Generic;

namespace ReMarket.Models
{
    //"The Wishlist class allows users to manage a collection of desired items."
    //"Each wishlist has a unique ID and name and is directly linked to the Listing class
    //indicating that the saved items must correspond to active listings."
    public class Wishlist
    {
        public int ID { get; set; }
        public string name { get; set; }

        public Wishlist(int ID, string name)
        {
            this.ID = ID;
            this.name=name;
        }

    }
}