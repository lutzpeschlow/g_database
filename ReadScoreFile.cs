

// --------------------------------------------------------------------------------------

namespace ExampleSqlite
{

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

}

