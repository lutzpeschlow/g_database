
namespace ExampleSqlite
{

public class Score
{
    public required string TName   { get; set; }
    public required string TDate   { get; set; }
    public required string TPlayer { get; set; }
    
    public int get_debug_info()
    {
        Console.WriteLine("debug info of Score ... ");
        Console.WriteLine(TName);
        Console.WriteLine(TDate);
        Console.WriteLine(TPlayer);
        return 0;
    }
    
}

}
