namespace ReMarket.Models
{
    public class Photo
    {
    //"The Photo class provides additional details for each listing by storing images.
    //It contains attributes like name and bytes,representing the image data.
    //A listing can have zero or more photos (0..*), reflecting optional visual content for listings.
        public string Name { get; set; }
        public string Bytes { get; set; }
        public bool IsThumbnail { get; set;} //added for the sake of user story 3.1.1

        public Photo(string name, string bytes, bool isThumbnail = false)
        {
            Name = name;
            Bytes = bytes;
            IsThumbnail = isThumbnail;
        }
    }
}
