using System;
using System.Data.SQLite;
class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=MyDatabase.db;Version=3;";
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string createTableQuery = "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY, Name TEXT)";
            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
            string insertDataQuery = "INSERT INTO Users (Name) VALUES ('Alice')";
            using (var command = new SQLiteCommand(insertDataQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string selectDataQuery = "SELECT * FROM Users";
            using (var command = new SQLiteCommand(selectDataQuery, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}");
                }
            }
        }
    }
}

// storage:
// ---------------------------------------------------------------
// Console.WriteLine("Hello, World!");