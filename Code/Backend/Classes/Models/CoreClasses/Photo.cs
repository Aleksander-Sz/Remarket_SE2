namespace ReMarket.Models
{
    public class Photo
    {
    //"The Photo class provides additional details for each listing by storing images.
    //It contains attributes like name and bytes,representing the image data.
    //A listing can have zero or more photos (0..*), reflecting optional visual content for listings.
        public string name { get; set; }
        public string bytes { get; set; }

        public Photo(string name, string bytes)
    {
        this.name = name;
        this.bytes = bytes;
    }
    }
}
