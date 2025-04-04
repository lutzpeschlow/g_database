using System.Data.SQLite;



namespace ExampleSqlite
{



class ServiceDesk
// Service Desk as class for interface between user and database access
//
// three main requests:
// - common database information (InfoAccess)
// - write data to database
// - read data to database
{

    // variable from main to be defined in this class via constructor
    private string _dbLoc;



    // constructor for class
    public ServiceDesk(string dbLoc)
    {
        _dbLoc = dbLoc;
    }



    public int InfoAccess(bool create=true, bool remove=false)
    // InfoAccess
    // common information around database
    {
        // show database info if database already exists
        if (File.Exists(_dbLoc))
        {
            Console.WriteLine("database exists");
            ShowDatabaseInfo(_dbLoc);
        }
        else
        {
            Console.WriteLine("database does not exist");
        }
        // create optional a new database, remove old one optional
        if (remove && File.EXists(_dbLoc))
        {
            File.Delete(_dbLoc);
        }
        if (create && !File.Exists(_dbLoc))
        {
            Console.WriteLine("will create new database");
            CreateDatabase(_dbLoc);
            ShowDatabaseInfo(_dbLoc);
        }
        return 0;
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
    // create new database with a pre-defined schema for later usage
    

    {
        Console.WriteLine("create database ...");
        using (var conn = new SQLiteConnection($"Data Source={path};Version=3;"))
        {
            // open connection
            conn.Open();


            string sql = "CREATE TABLE IF NOT EXISTS Players ( player_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                          "first_name VARCHAR(100),last_name VARCHAR(100),birth_date DATE,gender CHAR(1),email VARCHAR(255), " + 
                          "phone VARCHAR(50),registration_date DATE,home_club_id INTEGER,current_hcp_mf DECIMAL(5, 2),current_hcp_dgv DECIMAL(5, 2), " +
                          "hcp_index DECIMAL(5, 2),status VARCHAR(20) CHECK(status IN ('aktiv', 'inaktiv', 'gesperrt')), " +
                          "FOREIGN KEY(home_club_id) REFERENCES GolfClubs(club_id))";
            var command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();            

            sql = "CREATE TABLE IF NOT EXISTS GolfCourses (course_id INTEGER PRIMARY KEY AUTOINCREMENT,course_name VARCHAR(100)," +
                    "loc VARCHAR(255), tot_par INTEGER,tot_length INTEGER,num_holes INTEGER,contact_info VARCHAR(255))";
            command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();

            // // Table: Tournaments
            // sql = "CREATE TABLE IF NOT EXISTS Tournaments (ID INTEGER PRIMARY KEY AUTOINCREMENT, " + 
            //                "TName TEXT, TDate TEXT, NumHoles INTEGER, UNIQUE(TName, TDate))";
            // command = new SQLiteCommand(sql, conn);
            // command.ExecuteNonQuery();
            // // Table: Players
            // sql = "CREATE TABLE IF NOT EXISTS Players (ID INTEGER PRIMARY KEY AUTOINCREMENT, " + 
            //       "Username TEXT, Lastname TEXT, Firstname TEXT, Hcp1 REAL, Hcp2 REAL, UNIQUE(Username))";
            // command = new SQLiteCommand(sql, conn);
            // command.ExecuteNonQuery();
            // // Table: Scores
            // sql = "CREATE TABLE IF NOT EXISTS Scores (ID INTEGER PRIMARY KEY AUTOINCREMENT, " + 
            //       "PlayerId INTEGER, TournamentId INTEGER, HoleId INTEGER, Score Integer)";
            // command = new SQLiteCommand(sql, conn);
            // command.ExecuteNonQuery();

            


            // close connection
            conn.Close();
        }
    }
     













    public void WriteToDatabase(Score score)
    {
        Console.WriteLine("Write to Database");
        
        using (var conn = new SQLiteConnection($"Data Source={_dbLoc};Version=3;"))
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
                insertCmd.Parameters.AddWithValue("@Name", score.TName);
                insertCmd.Parameters.AddWithValue("@Datum", score.TDate);
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
                insertCmd.Parameters.AddWithValue("@Name", score.TPlayer);
                insertCmd.ExecuteNonQuery();
            }            
                    
        }
    }



    public void ReadFromDatabase()
    {
        Console.WriteLine("Read from Database ...");
        
        using (var conn = new SQLiteConnection($"Data Source={_dbLoc};Version=3;"))
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

}