using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ReMarket.Models
{
    public class Description
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Header { get; set; }
        public string? Paragraph { get; set; }


        public static Description Create(string header, string? paragraph = null)
        {
            return new Description
            {
                Header = header,
                Paragraph = paragraph
            };
        }
    }
}
/*namespace ReMarket.Models
{
    //"The Description class provides textual information for listings, 
    //with attributes such as header and paragraph. 
    //Each listing is required to have one description (1)"
    public class Description
    {
        public int Id { get; private set; }
        public string Header { get; set; }
        public string Paragraph { get; set; }

        public Description(string header, string paragraph)
        {
            Header = header;
            Paragraph = paragraph;
        }

        public Description(int id, string header, string paragraph) : this(header, paragraph)
        {
            Id = id;
        }

        public void SaveToDatabase(MySqlConnection connection)
        {
            using var command = new MySqlCommand(
                "INSERT INTO Description (header, paragraph) VALUES (@Header, @Paragraph); SELECT LAST_INSERT_ID();",
                connection);

            command.Parameters.AddWithValue("@Header", Header);
            command.Parameters.AddWithValue("@Paragraph", Paragraph);

            if (Id == 0)
            {
                var lastId = command.ExecuteScalar();
                Id = Convert.ToInt32(lastId.ToString());
            }
            else
            {
                command.CommandText = "UPDATE Description SET header = @Header, paragraph = @Paragraph WHERE id = @Id";
                command.Parameters.AddWithValue("@Id", Id);
                command.ExecuteNonQuery();
            }
        }

        public static Description? LoadById(MySqlConnection connection, int id)
        {
            using var command = new MySqlCommand(
                "SELECT id, header, paragraph FROM Description WHERE id = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (!reader.Read()) return null;

            return new Description(
                id: reader.GetInt32("id"),
                header: reader.GetString("header"),
                paragraph: reader.GetString("paragraph")
            );
        }

        public static List<Description> LoadAll(MySqlConnection connection)
        {
            var descriptions = new List<Description>();

            using var command = new MySqlCommand(
                "SELECT id, header, paragraph FROM Description",
                connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                descriptions.Add(new Description(
                    id: reader.GetInt32("id"),
                    header: reader.GetString("header"),
                    paragraph: reader.GetString("paragraph")
                ));
            }

            return descriptions;
        }

        public static void Delete(MySqlConnection connection, int id)
        {
            using var command = new MySqlCommand(
                "DELETE FROM Description WHERE id = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
    }

}*/