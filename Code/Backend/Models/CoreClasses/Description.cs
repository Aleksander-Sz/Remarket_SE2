namespace ReMarket.Models
{
    //"The Description class provides textual information for listings, 
    //with attributes such as header and paragraph. 
    //Each listing is required to have one description (1)"
    public class Description
    {
        public string header { get; set; }
        public string paragraph { get; set; }

        public Description(string header, string paragraph)
    {
        this.header = header;
        this.paragraph = paragraph;
    }
    }
}