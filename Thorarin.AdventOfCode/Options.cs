using CommandLine;

namespace Thorarin.AdventOfCode;

public class Options
{
    [Option("year", HelpText = "The year to run puzzles for")]
    public int? Year { get; set; }
    
    [Option("day", HelpText = "The day to run puzzles for")]
    public int? Day { get; set; }
    
    [Option('i', "iterations", Default = 1, HelpText = "The number of times to run the puzzle (for more accurate measurements)")]
    public int Iterations { get; set; }
}