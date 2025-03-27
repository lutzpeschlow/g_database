
namespace ExampleSqlite
{

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

}
