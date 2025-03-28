
namespace ExampleSqlite
{

public class Score
{
    public required string  TName   { get; set; }
    public required string  TDate   { get; set; }
    public required string  TPlayer { get; set; }
    public List<(int, int)> TScore  { get; set; } = new List<(int, int)>();
    
    public int get_debug_info()
    {
        Console.WriteLine("debug info of Score ... ");
        Console.WriteLine($"  {TName}  {TDate}  {TPlayer}  {string.Join(",", TScore)}");
        return 0;
    }
    
}

}
