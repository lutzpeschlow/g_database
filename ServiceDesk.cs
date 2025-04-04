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
        if (remove && File.Exists(_dbLoc))
        {
            File.Delete(_dbLoc);
            Console.WriteLine("!!! WARNING: existing database was deleted !!!");
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
            // Table: Players
            string sql = "CREATE TABLE IF NOT EXISTS Players ( player_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                          "first_name VARCHAR(100),last_name VARCHAR(100),birth_date DATE,gender CHAR(1),email VARCHAR(255), " + 
                          "phone VARCHAR(50),registration_date DATE,home_club_id INTEGER,current_hcp_mf DECIMAL(5, 2),current_hcp_dgv DECIMAL(5, 2), " +
                          "hcp_index DECIMAL(5, 2),status VARCHAR(20) CHECK(status IN ('aktiv', 'inaktiv', 'gesperrt')), " +
                          "FOREIGN KEY(home_club_id) REFERENCES GolfClubs(club_id))";
            var command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();            
            // Table: GolfCourses
            sql = "CREATE TABLE IF NOT EXISTS GolfCourses (course_id INTEGER PRIMARY KEY AUTOINCREMENT,course_name VARCHAR(100)," +
                    "loc VARCHAR(255), tot_par INTEGER,tot_length INTEGER,num_holes INTEGER,contact_info VARCHAR(255))";
            command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();
            // Table: Holes
            sql = "CREATE TABLE IF NOT EXISTS Holes (hole_id INTEGER PRIMARY KEY AUTOINCREMENT, course_id INTEGER, hole_number INTEGER," +
                    "par INTEGER,handicap INTEGER,length INTEGER,description TEXT,FOREIGN KEY (course_id) REFERENCES GolfCourses(course_id))";
            command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();                
            // Table: Tournaments
            sql = "CREATE TABLE IF NOT EXISTS Tournaments (tournament_id INTEGER PRIMARY KEY AUTOINCREMENT,tournament_name VARCHAR," +
                    "course_id INTEGER,start_date DATE, end_date DATE,format VARCHAR,max_participants INTEGER,entry_fee DECIMAL," +
                    "status VARCHAR,organizer_id INTEGER,handicap_cutoff DECIMAL," +
                    " FOREIGN KEY (course_id) REFERENCES GolfCourses(course_id),FOREIGN KEY (organizer_id) REFERENCES GolfClubs(club_id))";
            command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();                
            // Table: Participants
            sql = "CREATE TABLE IF NOT EXISTS Participants (participation_id INTEGER PRIMARY KEY AUTOINCREMENT,tournament_id INTEGER," +
                    "player_id INTEGER,registration_date DATE,handicap_at_time DECIMAL,payment_status VARCHAR," +
                    "FOREIGN KEY (tournament_id) REFERENCES Tournaments(tournament_id),FOREIGN KEY (player_id) REFERENCES Players(player_id))";
            command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();                
            // Table: Results
            sql = "CREATE TABLE IF NOT EXISTS Results (result_id INTEGER PRIMARY KEY AUTOINCREMENT,participation_id INTEGER," +
                    "round_number INTEGER,total_score INTEGER,net_score INTEGER,stableford_points INTEGER,birdies INTEGER," +
                    "pars INTEGER,bogeys INTEGER,double_bogeys_or_worse INTEGER,status VARCHAR," +
                    "FOREIGN KEY (participation_id) REFERENCES Participants(participation_id))";
            command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();                
            // Table: HoleResults             
            sql = "CREATE TABLE IF NOT EXISTS HoleResults (holeresult_id INTEGER PRIMARY KEY AUTOINCREMENT,result_id INTEGER," +
                    "hole_id INTEGER,strokes INTEGER,putts INTEGER,fairway_hit BOOLEAN,greens_in_regulation BOOLEAN,penalties INTEGER," +
                    "FOREIGN KEY (result_id) REFERENCES Results(result_id),FOREIGN KEY (hole_id) REFERENCES Holes(hole_id))";
            command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();                
            // Table: HandicapHistory           
            sql = "CREATE TABLE IF NOT EXISTS HandicapHistory (history_id INTEGER PRIMARY KEY AUTOINCREMENT,player_id INTEGER," +
                    "effective_date DATE,handicap_value_mf DECIMAL,handicap_value_dgv DECIMAL,calculation_basis VARCHAR,related_tournament_id INTEGER NULL," +
                    "FOREIGN KEY (player_id) REFERENCES Players(player_id),FOREIGN KEY (related_tournament_id) REFERENCES Tournaments(tournament_id))";
            command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();                
            // Table: GolfClubs              
            sql = "CREATE TABLE GolfClubs (club_id INTEGER PRIMARY KEY AUTOINCREMENT,club_name VARCHAR,address VARCHAR,contact_person VARCHAR," +
                    "phone VARCHAR,email VARCHAR,website VARCHAR)";
            command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();                
            // Table: Flights           
            sql = "CREATE TABLE Flights (flight_id INTEGER PRIMARY KEY AUTOINCREMENT,tournament_id INTEGER,round_number INTEGER,flight_name VARCHAR," +
                    "start_time DATETIME, tee_id INTEGER,FOREIGN KEY(tournament_id) REFERENCES Tournaments(tournament_id),FOREIGN KEY(tee_id) REFERENCES Tees(tee_id))";
            command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();                
            // Table: FlightAssignments              
            sql = "CREATE TABLE FlightAssignments (assignment_id INTEGER PRIMARY KEY AUTOINCREMENT,flight_id INTEGER,participation_id INTEGER,starting_order INTEGER," +
                    "FOREIGN KEY(flight_id) REFERENCES Flights(flight_id),FOREIGN KEY(participation_id) REFERENCES Participants(participation_id))";
            command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();                
            // Table: Tees          
            sql = "CREATE TABLE Tees (tee_id INTEGER PRIMARY KEY AUTOINCREMENT,course_id INTEGER,tee_name VARCHAR CHECK(tee_name IN ('Herren', 'Damen', 'Championship'))," +
                    "total_length INTEGER,course_rating DECIMAL,slope_rating INTEGER,FOREIGN KEY(course_id) REFERENCES GolfCourses(course_id))";
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