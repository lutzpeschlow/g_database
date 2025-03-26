using System.Data.SQLite;

// ======================================================================================

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
            Console.WriteLine($"size of database: {fileInfo.Length} Bytes\n");
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
        // {  connection.Open();
        //     string createTableQuery = "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY, Name TEXT)";
        //     using (var command = new SQLiteCommand(createTableQuery, connection))
        //     { command.ExecuteNonQuery(); }
        //     string insertDataQuery = "INSERT INTO Users (Name) VALUES ('Alice')";
        //     using (var command = new SQLiteCommand(insertDataQuery, connection))
        //     { command.ExecuteNonQuery(); }
        //     string selectDataQuery = "SELECT * FROM Users";
        //     using (var command = new SQLiteCommand(selectDataQuery, connection))
        //     using (var reader = command.ExecuteReader())
        //     {  while (reader.Read())
        //         {  Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}");



    public void WriteToDatabase(Score score)
    {
        Console.WriteLine("Write to Database");
        const string dbPath = "mydatabase_01.db";
        using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
        {
            conn.Open();

            // input data into table
            // TABLE: TOURNAMENT
            string createT = @"CREATE TABLE IF NOT EXISTS Tournament (Name TEXT,Datum TEXT)";
            using (SQLiteCommand createTableCmd = new SQLiteCommand(createT, conn))
            {
                createTableCmd.ExecuteNonQuery();
            }

            
            
            string insertT = "INSERT INTO Tournament (Name, Datum) VALUES (@Name, @Datum)";
            using (SQLiteCommand insertCmd = new SQLiteCommand(insertT, conn))
            {
                insertCmd.Parameters.AddWithValue("@Name", score.Tournament);
                insertCmd.Parameters.AddWithValue("@Datum", score.Date);
                insertCmd.ExecuteNonQuery();
            }

            // TABLE: PLAYER
            string createP = @"CREATE TABLE IF NOT EXISTS Player (Name TEXT,Datum TEXT)";
            using (SQLiteCommand createTableCmd = new SQLiteCommand(createP, conn))
            {
                createTableCmd.ExecuteNonQuery();
            }
            string insertP = "INSERT INTO Player (Name) VALUES (@Name)";
            using (SQLiteCommand insertCmd = new SQLiteCommand(insertP, conn))
            {
                insertCmd.Parameters.AddWithValue("@Name", score.Name);
                insertCmd.ExecuteNonQuery();
            }            
                    
        }
    }



    public void ReadFromDatabase()
    {
        Console.WriteLine("Read from Database ...");
        const string dbPath = "mydatabase_01.db";
        using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
        {
            conn.Open();
            string query = "SELECT Name, Datum FROM Tournament";

            using (SQLiteCommand command = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);  // Spalte 'Name'
                        string datum = reader.GetString(1); // Spalte 'Datum'

                        Console.WriteLine($"Name: {name}, Datum: {datum}");
                    }
                }
            }

        }
    }



    public void DebugPrintout()
    // DebugPrintout
    // printout of information for debugging issues
    {
        Console.WriteLine("debug printout of database access");
    }
}

