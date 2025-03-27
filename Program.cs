using System;
using System.CodeDom;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using ExampleSqlite;

namespace ExampleSqlite
{

class Program
{
    static void Main(string[] args)
    {
        // (0) variables
        // database location
        string dbPath = "/home/lutz/test";
        string dbFile = "s.db";
        string dbLoc = string.Concat(dbPath, "/", dbFile);
        Console.WriteLine(dbLoc);
        // score file loction
        string txtPath = "/home/lutz/test";
        string txtFile = "current_score.txt";
        string txtLoc = string.Concat(txtPath, "/", txtFile);
        Console.WriteLine(txtLoc);        

        // (1) Database check
        ServiceDesk sd = new ServiceDesk(dbLoc);
        int i_db_info = sd.InfoAccess(true);

        // (2) Current Scorefile check+-
        // ReadScoreFile rsf = new ReadScoreFile(txtLoc);
        // Score score = rsf.GetCurrentScore();
        
        // sd.WriteToDatabase(score);
        // sd.ReadFromDatabase();
        
    }
}

// end of namespace
}


