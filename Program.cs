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
    static void Main(string[] args)
    {
        // (0) variables
        bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        // hardcoded database and score file location
        string dbPath = @"";
        string dbFile = "s.db";
        string txtPath = @"";
        string txtFile = "current_score.txt";
        if (isWindows)
        {
            Console.WriteLine("win ...");
            dbPath  = @"c:\tmp";
            txtPath = @"c:\tmp";
            Directory.CreateDirectory(dbPath);
        }
        else if (isLinux)
        {
            Console.WriteLine("linux ...");
            dbPath  = "/tmp";
            txtPath = "/tmp";
        }
        string dbLoc = string.Concat(dbPath, "/", dbFile);
        Console.WriteLine(dbLoc);
        string txtLoc = string.Concat(txtPath, "/", txtFile);
        Console.WriteLine(txtLoc);        

        // (1) Database check
        ServiceDesk sd = new ServiceDesk(dbLoc);
        int i_db_info = sd.InfoAccess(true,false);

        // (2) Current Scorefile check
        // ReadScoreFile rsf = new ReadScoreFile(txtLoc);
        // Score score = rsf.GetCurrentScore();
        // score.get_debug_info();
        
        // sd.WriteTest();
        sd.TextToDatabase();
        // sd.WriteToDatabase(score);
        // sd.ReadFromDatabase();
        
    }
}

// end of namespace
}


