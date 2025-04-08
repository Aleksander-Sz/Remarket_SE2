using System;
using System.Collections.Generic;

namespace ReMarket.Models
{
//"The Listing class is central to the marketplace, representing items posted by sellers."
//"Key attributes include title, price,and status"
//"Each listing belongs to a specific Category, has a Description, and can include multiple Photo objects to showcase the item visually.

    public class Listing
    {
        public string title { get; set; }
        public decimal price { get; set; }
        public ListingStatus status { get; set; }

        public Listing(string title, decimal price, ListingStatus status)
        {
            this.title = title;
            this.price = price;
            this.status = status;
        }
}
}