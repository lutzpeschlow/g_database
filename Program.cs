using System;
using System.CodeDom;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;



namespace ExampleSqlite
{



class Program
{
    static void Main(string[] args)
    {
        
        ServiceDesk sd = new ServiceDesk();
        ServiceDesk.InfoAccess(true);
        
    }
}





class ServiceDesk
// Service Desk as class for interface between user and database access
//
// three main requests:
// - common database information
// - write data to database
// - read data to database
{


    public static void InfoAccess(bool create=true)
    // InfoAccess
    // common information around database
    {
        const string dbPath = "mydatabase_01.db";
        
        // show database info
        if (File.Exists(dbPath))
        {
            Console.WriteLine("database exists");
            ShowDatabaseInfo(dbPath);
        }
        else
        {
            Console.WriteLine("database does not exist");
        }
        // create optional a new database
        if (create && !File.Exists(dbPath))
        {
            Console.WriteLine("will create new database");
            CreateDatabase(dbPath);
            ShowDatabaseInfo(dbPath);
        }

    }



    private static void ShowDatabaseInfo(string path)
    // ShowDatabase
    // show database infos
    // - list of existing tables
    // - size of database
    {
        Console.WriteLine("show database ...");
        using (var conn = new SQLiteConnection($"Data Source={path};Version=3;"))
        {
            conn.Open();
            // list of tables
            var cmdTables = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table';", conn);
            using (var reader = cmdTables.ExecuteReader())
            {
                Console.WriteLine("\ntables in database:");
                while (reader.Read())
                {
                    Console.WriteLine($" - {reader["name"]}");
                }
            }
            // database size
            var fileInfo = new FileInfo(path);
            Console.WriteLine($"size of database: {fileInfo.Length} Bytes");
        }
    }



    private static void CreateDatabase(string path)
    // CreateDatabase
    // create new database with a dummy table
    {
        Console.WriteLine("create database ...");
        using (var conn = new SQLiteConnection($"Data Source={path};Version=3;"))
        {
            conn.Open();
            string createTable = "CREATE TABLE welcome (Id INTEGER PRIMARY KEY, Name TEXT)";
            var cmdCreate = new SQLiteCommand(createTable,conn);
            cmdCreate.ExecuteNonQuery();
        }
    }





        // string connectionString = "Data Source=MyDatabase.db;Version=3;";
        // using (var connection = new SQLiteConnection(connectionString))
        // {
        //     connection.Open();
        //     string createTableQuery = "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY, Name TEXT)";
        //     using (var command = new SQLiteCommand(createTableQuery, connection))
        //     { command.ExecuteNonQuery(); }
        //     string insertDataQuery = "INSERT INTO Users (Name) VALUES ('Alice')";
        //     using (var command = new SQLiteCommand(insertDataQuery, connection))
        //     { command.ExecuteNonQuery(); }
        //     string selectDataQuery = "SELECT * FROM Users";
        //     using (var command = new SQLiteCommand(selectDataQuery, connection))
        //     using (var reader = command.ExecuteReader())
        //     {
        //         while (reader.Read())
        //         {
        //             Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}");



    





    public void WriteToDatabase()
    {
        Console.WriteLine("Write to Database");
    }

    public void ReadFromDatabase()
    {
        Console.WriteLine("Read from Database");
    }



    public void DebugPrintout()
    // DebugPrintout
    // printout of information for debugging issues
    {
        Console.WriteLine("debug printout of database access");
    }






}




class ReadScoreFile
// ReadScoreFile rsf = new ReadScoreFile();
// rsf.DebugPrintout();
// rsf.GetScoreFromFile("/home/lutz/c_sharp/example_sqlite/ExampleSqlite/scores/s_01.txt");
{



    public void DebugPrintout()
    {
        Console.WriteLine("debug printout of read score file");
    }

    public int GetScoreFromFile(string filename)
    {
        Console.WriteLine(filename);
        bool is_file = false;
        is_file = File.Exists(filename);
        Console.WriteLine(is_file);
        string[] zeilen = File.ReadAllLines(filename);
        foreach (string zeile in zeilen)
        {
            Console.WriteLine(zeile);
        }
        
        //using (StreamReader sr = new StreamReader("scores/s_01.txt"))
        //{
        //    string line;
        //    while ((line = sr.ReadLine()) != null)
        //    {
        //        Console.WriteLine(line);
        //    }  
        //}
        return 0;
    }
}







}


