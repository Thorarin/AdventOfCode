namespace Thorarin.AdventOfCode.Framework;

public class PuzzleAttribute : Attribute
{
    public int Year { get; set; }
    public int Day { get; set; }
    public int Part { get; set; }
}