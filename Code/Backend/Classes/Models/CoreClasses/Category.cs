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

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public static Category Create(string name)
        {
            return new Category(0, name);
        }

        public void SaveToDatabase(MySqlConnection connection)
        {
            using var command = new MySqlCommand(
                "INSERT INTO category (name) VALUES (@Name); SELECT LAST_INSERT_ID();",
                connection);

            command.Parameters.AddWithValue("@Name", Name);
            if (Id == 0)
            {
                Id = Convert.ToInt32(command.ExecuteScalar());
            }
            else
            {
                command.CommandText = "UPDATE category SET name = @Name WHERE id = @Id";
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
            }
        }

        public static Category? LoadById(MySqlConnection connection, int id)
        {
            using var command = new MySqlCommand(
                "SELECT id, name FROM category WHERE id = @Id",
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
                "SELECT id, name FROM category",
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
                "DELETE FROM category WHERE id = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
    }
}


