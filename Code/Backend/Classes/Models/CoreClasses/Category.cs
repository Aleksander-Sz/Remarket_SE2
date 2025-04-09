namespace ReMarket.Models
{
    //"The Category class organizes listings into logical groupings. 
    //It includes an ID and a name, enabling the system to classify and filter listings. 
    //Each category can have multiple associated listings (1..*)" 
    public class Category
    {
        public int ID { get; set; }
        public string name { get; set; }

        public Category(int ID, string name)
    {
        this.ID = ID;
        this.name = name;
    }
    }
}