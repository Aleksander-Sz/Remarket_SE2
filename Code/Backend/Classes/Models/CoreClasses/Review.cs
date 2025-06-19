using System;

namespace ReMarket.Models
{
//"The Review class allows buyers to provide feedback on listings. 
//It includes attributes such as title, score (e.g., a numerical rating), and description for detailed comments. 
//Reviews are linked directly to listings, showing that reviews pertain to specific items."
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Score { get; set; }
        public string Description { get; set; } //this description has nothing to do with class Description
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int ListingId { get; set; }
        public Listing Listing { get; set; }


        public Review(string title, int score, string description)
        {
            this.Title = title;
            this.Description=description;
            this.Score=score;
        }
    }
}