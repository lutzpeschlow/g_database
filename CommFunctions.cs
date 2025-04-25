using System.Runtime.InteropServices;

namespace ExampleSqlite
{

public class CommFunctions
{


    // set file locations
    public (string dbLoc, string txtLoc, string argLoc) FileLocations()
    {
        bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        string dbPath, txtPath, argPath;
        const string dbFile = "s.db";
        const string txtFile = "current_score.txt";
        const string argFile = "input_arguments.txt";
        if (isWindows)
        {
            Console.WriteLine("windows ...");
            dbPath = txtPath = argPath = @"c:\tmp";
            Directory.CreateDirectory(dbPath);
        }
        else if (isLinux)
        {
            Console.WriteLine("linux ...");
            dbPath = txtPath = argPath = "/tmp";
        }
        else
        {
            throw new NotSupportedException("non supported operating system ...");
        }
        // tuple as return value, three items in tuple
        return ( Path.Combine(dbPath, dbFile),Path.Combine(txtPath, txtFile),Path.Combine(argPath, argFile) );
    }



    // read arguments from file
    public string ReadInputArguments()
    {
        var (_, _, argLoc) = FileLocations();
        string[] validOptions = { "SHOW", "READ", "WRITE" };
        // set default value
        if (!File.Exists(argLoc)) 
            return "SHOW"; 
        // read file
        string content;
        try
        {
            content = File.ReadAllText(argLoc).Trim(); 
        }
        catch
        {
            return "SHOW";
        }
        // check valid values for arguments and return found value
        foreach (var option in validOptions)
        {
            if (content.Equals(option, StringComparison.OrdinalIgnoreCase)) 
                return option.ToUpper(); 
        }
        // if there was no valid value in file, return default value
        return "SHOW"; 
    }
}
}
