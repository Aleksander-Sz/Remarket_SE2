namespace ReMarket.Models
{

//"The ListingStatus enumeration defines the possible states for a listing,
// such as Active, Archived, and Draft. 
//This helps sellers manage their inventory efficiently by specifying the visibility of their listings."
    public enum ListingStatus
    {
        Active,
        Archived,
        Draft
    }

//"The OrderStatus enumeration categorizes orders into states like Shipping and Shipped. 
//This ensures clarity in the order lifecycle and allows buyers to track progress effectively"

    public enum OrderStatus
    {
        Shipping,
        Shipped
    }
}