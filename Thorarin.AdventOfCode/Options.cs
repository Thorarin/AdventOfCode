using CommandLine;

namespace Thorarin.AdventOfCode;

public class Options
{
    [Option("year", HelpText = "The year to run puzzles for")]
    public int? Year { get; set; }
    
    [Option("day", HelpText = "The day to run puzzles for")]
    public int? Day { get; set; }
}