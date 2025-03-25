using System;
using System.CodeDom;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using ExampleSqlite;





namespace ExampleSqlite
{


// === MAIN =============================================================================


class Program
{
    static void Main(string[] args)
    {
        // first database check
        ServiceDesk sd = new ServiceDesk();
        ServiceDesk.InfoAccess(true);

        // do we have a new score file: current_score.txt
        ReadScoreFile rsf = new ReadScoreFile();
        Score score = rsf.GetCurrentScore();
        // rsf.GetScoreFromFile("/home/lutz/c_sharp/example_sqlite/ExampleSqlite/scores/current_score.txt");
        sd.WriteToDatabase(score);
        sd.ReadFromDatabase();
        
    }
}


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


// --------------------------------------------------------------------------------------


public class Score
{
    public required string Tournament { get; set; }
    public required string Date { get; set; }
    // public DateTime Datum { get; set; }
    public required string Name { get; set; }
    // public List<int> ScoreTable { get; set; }
    
    public int get_debug_info()
    {
        Console.WriteLine("debug info of Score ... ");
        Console.WriteLine(Tournament);
        Console.WriteLine(Date);
        Console.WriteLine(Name);

        return 0;
    }
    
}


// --------------------------------------------------------------------------------------


public class ReadScoreFile
// ReadScoreFile rsf = new ReadScoreFile();
// rsf.DebugPrintout();
// rsf.GetScoreFromFile("/home/lutz/c_sharp/example_sqlite/ExampleSqlite/scores/s_01.txt");
{


    public Score GetCurrentScore()
    // GetCurrentScore
    // if there is a file current_score.txt it is assumed to be a new score, which is read
    // and data are intended to be stored in the database
    {
        // check file existence
        string current_file = @"/home/lutz/c_sharp/example_sqlite/ExampleSqlite/scores/current_score.txt";
        bool is_file = false;
        is_file = File.Exists(current_file);
        Console.WriteLine($"get current score ... {current_file}, {is_file}");
        Score score = new Score { Tournament = "Dummy", Date = "Dummy", Name = "Dummy"};
        // get content of file
        if (is_file == true)

        {
            // pre-definition
            string[] zeilen = File.ReadAllLines(current_file);
            int counter = 0;
            
            // direct access to header
            score.Tournament = zeilen[0];
            score.Date = zeilen[1]       ;
            // if (DateTime.TryParse(zeilen[1], out var parsedDatum))
            // {            score.Datum = parsedDatum;                        }
            // Fallback bei ungültigem Datumsformat
            // else             {    score.Datum = DateTime.MinValue;            }
            score.Name = zeilen[2];
            // go into loop of lines
            foreach (string zeile in zeilen)
            {
                Console.WriteLine(zeile);
                counter = counter + 1;
            }
            Console.WriteLine(score.Tournament);
            Console.WriteLine(score.Date);
            Console.WriteLine(score.Name);   
        }
        else
        {
            Console.WriteLine("no current score file found ...");
            score.Tournament = "";
            score.Date = "";
            score.Name = "";
        }
        // return value is object score
        return score;
    }

    
}


// --------------------------------------------------------------------------------------



// end of namespace
}


