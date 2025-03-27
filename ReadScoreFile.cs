

// --------------------------------------------------------------------------------------

namespace ExampleSqlite
{

public class ReadScoreFile
// ReadScoreFile rsf = new ReadScoreFile();
// rsf.DebugPrintout();
// rsf.GetScoreFromFile("/home/lutz/c_sharp/example_sqlite/ExampleSqlite/scores/s_01.txt");
{

    // private variable to be defined in this class via constructor
    private string _txtLoc;

    // constructor for class, asssin private variable
    public ReadScoreFile(string txtLoc)
    {
        _txtLoc = txtLoc;
    }


    public Score GetCurrentScore()
    // GetCurrentScore

    {
        // check file existence
        bool is_file = false;
        is_file = File.Exists(_txtLoc);
        Console.WriteLine($"get current score ... {_txtLoc}  {is_file}");
        Score score = new Score { TName = "Dummy", TDate = "Dummy", TPlayer = "Dummy"};
        // get content of file
        if (is_file == true)
        {
            // pre-definition
            string[] zeilen = File.ReadAllLines(_txtLoc);
            int counter = 0;
            // direct access to header which contains tournament name, date, player name
            score.TName   = zeilen[0];
            score.TDate   = zeilen[1];
            score.TPlayer = zeilen[2];
            // go into loop of lines for score itself
            foreach (string zeile in zeilen)
            {
                Console.WriteLine(zeile);
                counter = counter + 1;
            }
            Console.WriteLine($"{score.TName}, {score.TDate}, {score.TPlayer}");
              
        }
        // no current score file found, will reset the data in score object
        else
        {
            Console.WriteLine("no current score file found ...");
            score.TName = "";
            score.TDate = "";
            score.TPlayer = "";
        }
        // return value is object score
        return score;
    }

    
}

}

