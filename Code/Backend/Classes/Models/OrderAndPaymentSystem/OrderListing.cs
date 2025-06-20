using System;

namespace ReMarket.Models
{

    //"The Order class manages the transactional aspect of the platform. 
    //Attributes include ID, shipTo (an Address object), shipped (a date when sent), 
    //and description (which uses the ListingDescription (description as string and an array of photos(files)) data type). 
    //Each order is associated with one or more listings (1..*), 
    //indicating that orders can encompass multiple items.
    public class OrderListing
    {
        public int OrderId { get; set; }
        public int ListingId { get; set; }
        public Order Order { get; set; }
        public Listing Listing { get; set; }
    }


}