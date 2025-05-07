using Xunit;
using ReMarket.Models;
using MySql.Data.MySqlClient;
using System;

namespace ReMarket.Tests.Models
{
    public class DescriptionDatabaseTests : IDisposable
    {
        private readonly string _connectionString;
        private readonly string _testDatabaseName = "remarket_test";

        public DescriptionDatabaseTests()
        {//your password
            var baseConnectionString = "server=localhost;user=root;password=your_password;";

            using (var connection = new MySqlConnection(baseConnectionString))
            {
                connection.Open();
                new MySqlCommand($"CREATE DATABASE IF NOT EXISTS {_testDatabaseName}", connection)
                    .ExecuteNonQuery();
            }

            _connectionString = $"{baseConnectionString}database={_testDatabaseName};";

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                new MySqlCommand(@"
                    CREATE TABLE IF NOT EXISTS Description (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        header VARCHAR(255) NOT NULL,
                        paragraph TEXT
                    )", connection).ExecuteNonQuery();

                new MySqlCommand("DELETE FROM Description", connection).ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            using (var connection = new MySqlConnection(
                //here your password
                "server=localhost;user=root;password=Your_password;"))
            {
                connection.Open();
                new MySqlCommand($"DROP DATABASE IF EXISTS {_testDatabaseName}", connection)
                    .ExecuteNonQuery();
            }
        }


        [Fact]
        public void CreateAndSave_NewDescription_WorksCorrectly()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var description = new Description("Test Header", "Test Paragraph");

                Assert.Equal(0, description.Id);

                description.SaveToDatabase(connection);

                Assert.True(description.Id > 0);

                var loaded = Description.LoadById(connection, description.Id);
                Assert.Equal("Test Header", loaded!.Header);
                Assert.Equal("Test Paragraph", loaded!.Paragraph);
            }
        }

        [Fact]
        public void LoadById_ExistingDescription_ReturnsDescription()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var insertCmd = new MySqlCommand(
                    "INSERT INTO Description (header, paragraph) VALUES ('Test Header', 'Test Paragraph')",
                    connection);
                insertCmd.ExecuteNonQuery();

                var getIdCmd = new MySqlCommand("SELECT LAST_INSERT_ID()", connection);
                var id = Convert.ToInt32(getIdCmd.ExecuteScalar().ToString());

                var description = Description.LoadById(connection, id);
                Assert.NotNull(description);
                Assert.Equal(id, description.Id);
                Assert.Equal("Test Header", description.Header);
                Assert.Equal("Test Paragraph", description.Paragraph);
            }
        }

        [Fact]
        public void LoadAll_ReturnsAllDescriptions()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                new MySqlCommand(
                    "INSERT INTO Description (header, paragraph) VALUES ('H1', 'P1'), ('H2', 'P2')",
                    connection).ExecuteNonQuery();

                var descriptions = Description.LoadAll(connection);
                Assert.Equal(2, descriptions.Count);
                Assert.Contains(descriptions, d => d.Header == "H1" && d.Paragraph == "P1");
                Assert.Contains(descriptions, d => d.Header == "H2" && d.Paragraph == "P2");
            }
        }

        [Fact]
        public void Delete_RemovesDescription()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var insertCmd = new MySqlCommand(
                    "INSERT INTO Description (header, paragraph) VALUES ('To Delete', 'Delete Me')",
                    connection);
                insertCmd.ExecuteNonQuery();
                var id = Convert.ToInt32(new MySqlCommand("SELECT LAST_INSERT_ID()", connection)
                    .ExecuteScalar().ToString());

                Description.Delete(connection, id);

                var verifyCmd = new MySqlCommand("SELECT COUNT(*) FROM Description WHERE id = @id", connection);
                verifyCmd.Parameters.AddWithValue("@id", id);
                var count = Convert.ToInt32(verifyCmd.ExecuteScalar());
                Assert.Equal(0, count);
            }
        }
    }
}