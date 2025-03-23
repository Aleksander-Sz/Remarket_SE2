using System;

namespace ReMarket.Models
{
//"The Review class allows buyers to provide feedback on listings. 
//It includes attributes such as title, score (e.g., a numerical rating), and description for detailed comments. 
//Reviews are linked directly to listings, showing that reviews pertain to specific items."
    public class Review
    {
        public string title { get; set; }
        public int score { get; set; }
        public string description { get; set; } //this description has nothing to do with class Description


        public Review(string title, int score, string description)
        {
            this.title = title;
            this.description=description;
            this.score=score;
        }
    }
}