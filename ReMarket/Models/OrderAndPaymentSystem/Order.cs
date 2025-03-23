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
        public string ID { get; set; }
        public Address shipTo { get; set; }
        public DateTime shipped { get; set; } //date when shipped
        public string description { get; set; } //this can be wrong, on figure 1 it says it should be of type ListingDescription


        public Order(string ID, Address shipTo, DateTime shipped, string description)
        {
            this.ID = ID;
            this.shipped=shipped;
            this.description=description;
            this.shipTo=shipTo;
        }
    }
}