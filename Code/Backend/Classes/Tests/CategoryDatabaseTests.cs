using Xunit;
using ReMarket.Models;
using MySql.Data.MySqlClient;
using System;

namespace ReMarket.Tests.Models
{
    //those tests are for categories. They work and the rest of tables was implemented similarly.
    public class CategoryDatabaseTests : IDisposable
    {
        private readonly string _connectionString;
        private readonly string _testDatabaseName = "remarket_test";

        public CategoryDatabaseTests()
        {
            //YOUR PASSWORD IF U R RUNNING IT
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
                    CREATE TABLE IF NOT EXISTS Category (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        name VARCHAR(100) NOT NULL
                    )", connection).ExecuteNonQuery();

                new MySqlCommand("DELETE FROM Category", connection).ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            using (var connection = new MySqlConnection(
                //YOUR PASSWORD IF U R RUNNING IT
                "server=localhost;user=root;password=your_password;"))
            {
                connection.Open();
                new MySqlCommand($"DROP DATABASE IF EXISTS {_testDatabaseName}", connection)
                    .ExecuteNonQuery();
            }
        }

        [Fact]
        public void SaveToDatabase_NewCategory_SavesCorrectly()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var category = new Category(0, "Electronics");
                category.SaveToDatabase(connection);

                Assert.True(category.ID > 0);
                var cmd = new MySqlCommand("SELECT name FROM Category WHERE id = @id", connection);
                cmd.Parameters.AddWithValue("@id", category.ID);
                var savedName = cmd.ExecuteScalar()?.ToString();

                Assert.Equal("Electronics", savedName);
            }
        }

        [Fact]
        public void LoadById_ExistingCategory_ReturnsCategory()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var insertCmd = new MySqlCommand(
                    "INSERT INTO Category (name) VALUES ('Books')",
                    connection);
                insertCmd.ExecuteNonQuery();

                var getIdCmd = new MySqlCommand("SELECT LAST_INSERT_ID()", connection);
                var lastId = getIdCmd.ExecuteScalar();
                int id = Convert.ToInt32(lastId.ToString());

                var category = Category.LoadById(connection, id);
                Assert.NotNull(category);
                Assert.Equal(id, category.ID);
                Assert.Equal("Books", category.Name);
            }
        }

        [Fact]
        public void LoadAll_ReturnsAllCategories()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                new MySqlCommand(
                    "INSERT INTO Category (name) VALUES ('Clothing'), ('Furniture')",
                    connection).ExecuteNonQuery();

                var categories = Category.LoadAll(connection);
                Assert.Equal(2, categories.Count);
                Assert.Contains(categories, c => c.Name == "Clothing");
                Assert.Contains(categories, c => c.Name == "Furniture");
            }
        }
    }
}