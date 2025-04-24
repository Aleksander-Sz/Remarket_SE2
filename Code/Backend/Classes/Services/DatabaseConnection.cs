using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ReMarket.Services
{
    internal class DatabaseConnection
    {
        private readonly string _connectionString;
        private readonly string _databaseName;
        public DatabaseConnection(string databaseName, string user, string password)
        {
            _databaseName = databaseName;
            //YOUR PASSWORD IF U R RUNNING IT
            var baseConnectionString = "server=localhost;user=" + user + ";password=" + password + ";";

            // Check if the database exists
            if (!DatabaseExists(baseConnectionString, _databaseName))
            {
                // If database doesn't exist, create it
                CreateDatabase(baseConnectionString);
            }
            else
            {
                // If the database exists, run the SQL script
                ExecuteSqlScript(baseConnectionString);
            }

            _connectionString = $"{baseConnectionString}database={_databaseName};";
        }
        private bool DatabaseExists(string baseConnectionString, string databaseName)
        {
            // Connect to MySQL server without specifying a database
            using (var connection = new MySqlConnection(baseConnectionString))
            {
                try
                {
                    connection.Open();
                    // Check if the database exists
                    var command = new MySqlCommand($"SHOW DATABASES LIKE '{databaseName}'", connection);
                    var result = command.ExecuteScalar();
                    return result != null;
                }
                catch (Exception ex)
                {
                    // Log the error if needed, but assume it means the database doesn't exist
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
        private void CreateDatabase(string baseConnectionString)
        {
            using (var connection = new MySqlConnection(baseConnectionString))
            {
                connection.Open();
                // Create the database if it doesn't exist
                new MySqlCommand($"CREATE DATABASE IF NOT EXISTS {_databaseName}", connection).ExecuteNonQuery();
                Console.WriteLine($"Database {_databaseName} created.");
            }
        }
        private void ExecuteSqlScript(string baseConnectionString)
        {
            // Build the connection string for the existing database
            var fullConnectionString = $"{baseConnectionString}database={_databaseName};";
            using (var connection = new MySqlConnection(fullConnectionString))
            {
                connection.Open();
                string sqlScript = File.ReadAllText("Database Creation.sql"); // Specify the path to your SQL file
                var command = new MySqlCommand(sqlScript, connection);
                command.ExecuteNonQuery();
                Console.WriteLine("SQL script executed successfully.");
            }
        }
    }
}
