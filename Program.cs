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
        // definition of database location and score file location
        string dbPath = "/home/lutz/test";
        string dbFile = "s.db";
        string dbLoc = string.Concat(dbPath, "/", dbFile);
        Console.WriteLine(dbLoc);
        Console.WriteLine();

        // first database check
        ServiceDesk sd = new ServiceDesk(dbLoc);
        ServiceDesk.InfoAccess(true);

        // do we have a new score file: current_score.txt
        ReadScoreFile rsf = new ReadScoreFile();
        Score score = rsf.GetCurrentScore();
        // rsf.GetScoreFromFile("/home/lutz/c_sharp/example_sqlite/ExampleSqlite/scores/current_score.txt");
        sd.WriteToDatabase(score);
        sd.ReadFromDatabase();
        
    }
}

// end of namespace
}


