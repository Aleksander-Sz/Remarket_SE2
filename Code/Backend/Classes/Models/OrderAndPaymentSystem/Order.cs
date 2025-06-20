using System;

namespace ReMarket.Models
{

//"The Order class manages the transactional aspect of the platform. 
//Attributes include ID, shipTo (an Address object), shipped (a date when sent), 
//and description (which uses the ListingDescription (description as string and an array of photos(files)) data type). 
//Each order is associated with one or more listings (1..*), 
//indicating that orders can encompass multiple items.
    public class Order
    {
        public int Id { get; set; }
        public string ShipTo { get; set; }
        public DateTime Shipped { get; set; } //date when shipped
        public string Description { get; set; } //this can be wrong, on figure 1 it says it should be of type ListingDescription
        public int SellerId { get; set; }
        public Account Seller {  get; set; }
        public int BuyerId { get; set; }
        public Account Buyer { get; set; }

        public Order() { }

        public Order(int Id, string shipTo, DateTime shipped, string description)
        {
            this.Id = Id;
            this.Shipped=shipped;
            this.Description=description;
            this.ShipTo=shipTo;
        }
    }
}