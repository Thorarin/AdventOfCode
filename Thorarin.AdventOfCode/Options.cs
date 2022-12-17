using CommandLine;

namespace Thorarin.AdventOfCode;

public class Options
{
    [Option("year", HelpText = "The year to run puzzles for")]
    public int? Year { get; set; }
    
    [Option("day", HelpText = "The day to run puzzles for")]
    public int? Day { get; set; }
    
    [Option("implementation")]
    public string? Implementation { get; set; }
    
    [Option('i', "iterations", Default = 1, HelpText = "The number of times to run the puzzle (for more accurate measurements)")]
    public int Iterations { get; set; }

    [Option("warmup", HelpText = "Make sure code is pre-JITted by running with sample data once")]
    public bool Warmup { get; set; }

    [Option("extra", HelpText = "Run puzzles for additional input files that may be defined")]
    public bool RunExtraInputs { get; set; }
}