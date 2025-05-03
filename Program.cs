using System;
using System.CodeDom;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Runtime.InteropServices;
using ExampleSqlite;
// DEV;PM;DES;QA

namespace ExampleSqlite
{

class Program
{
    static void Main()
    {
        // (0) set file locations, read arguments
        CommFunctions cf = new CommFunctions();
        var file_locations = cf.FileLocations();
        // 
        string dbLoc = file_locations.Item1;
        string txtLoc = file_locations.Item2;
        string path = file_locations.Item4;
        string input_arg = cf.ReadInputArguments(); 

        // depending on argument value, show, read, write, call the according function
        switch(input_arg.ToUpper())  
        {
            // (1) database check
            case "SHOW":
                Enrollment sd = new Enrollment(dbLoc);
                int i_db_info = sd.InfoAccess(true,false);
                break;
            // (2) read any data from database
            case "READ":
                Console.WriteLine("READ");
                // (2) Current Scorefile check
                // ReadScoreFile rsf = new ReadScoreFile(txtLoc);
                // Score score = rsf.GetCurrentScore();
                // score.get_debug_info();
                break; 
            // (3) write to database from textfile
            case "WRITE":
                Console.WriteLine("WRITE");
                // sd.WriteTest();
                Processing s = new Processing(dbLoc,path);
                s.FilesToDatabase();
                // sd.WriteToDatabase(score);
                // sd.ReadFromDatabase();
                // Hier write-Logik implementieren
                break;
            // (4) score to database from score-textfile
            case "SCORE":
                Console.WriteLine("SCORE");
                break;
            // default case
            default:  
                Console.WriteLine("default case");
                break;
        }
    }
}

// end of namespace
}


