namespace ReMarket.Models
{
    //"The Description class provides textual information for listings, 
    //with attributes such as header and paragraph. 
    //Each listing is required to have one description (1)"
    public class Description
    {
        public string Header { get; set; }
        public string Paragraph { get; set; }

        public Description(string header, string paragraph)
    {
        Header = header;
        Paragraph = paragraph;
    }
    }
}