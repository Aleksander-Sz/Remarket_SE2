using System;
using System.Collections.Generic;

namespace ReMarket.Models
{
//"The Listing class is central to the marketplace, representing items posted by sellers."
//"Key attributes include title, price,and status"
//"Each listing belongs to a specific Category, has a Description, and can include multiple Photo objects to showcase the item visually.

    public class Listing
    {
        public int Id { get; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public ListingStatus Status { get; set; } 
        public Description Description { get; set; }
        public Photo? Thumbnail { get; set; }  
        public List<Photo> Photos { get; } = new();  

        public Listing(int id, string title, decimal price, Category category, Description description)
        {
            Id = id;
            Title = title;
            Price = price;
            Category = category;
            Description = description;
            Status = ListingStatus.Draft; 
        }
}
}
