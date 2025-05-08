using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ReMarket.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}

/*namespace ReMarket.Models

{
    //"The Category class organizes listings into logical groupings. 
    //It includes an ID and a name, enabling the system to classify and filter listings. 
    //Each category can have multiple associated listings (1..*)" 
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Category(int id, string name)
        {
            ID = id;
            Name = name;
        }
        public static Category Create(string name)
        {
            return new Category(0, name);
        }

        public void SaveToDatabase(MySqlConnection connection)
        {
            using var command = new MySqlCommand(
                "INSERT INTO Category (name) VALUES (@Name); SELECT LAST_INSERT_ID();",
                connection);

            command.Parameters.AddWithValue("@Name", Name);
            if (ID == 0)
            {
                ID = Convert.ToInt32(command.ExecuteScalar());
            }
            else
            {
                command.CommandText = "UPDATE Category SET name = @Name WHERE id = @Id";
                command.Parameters.AddWithValue("@Id", ID);
                command.ExecuteNonQuery();
            }
        }

        public static Category? LoadById(MySqlConnection connection, int id)
        {
            using var command = new MySqlCommand(
                "SELECT id, name FROM Category WHERE id = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (!reader.Read()) return null;

            return new Category(
                id: reader.GetInt32("id"),
                name: reader.GetString("name")
            );
        }


        public static List<Category> LoadAll(MySqlConnection connection)
        {
            var categories = new List<Category>();

            using var command = new MySqlCommand(
                "SELECT id, name FROM Category",
                connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                categories.Add(new Category(
                    id: reader.GetInt32("id"),
                    name: reader.GetString("name")
                ));
            }

            return categories;
        }

        public static void Delete(MySqlConnection connection, int id)
        {
            using var command = new MySqlCommand(
                "DELETE FROM Category WHERE id = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
    }
}*/