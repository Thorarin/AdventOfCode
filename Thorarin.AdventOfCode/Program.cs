using CommandLine;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode;

internal class Program
{
    internal static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args).WithParsed(RunWithOptions);
    }

    private static void RunWithOptions(Options options)
    {
        var puzzleFinder = new PuzzleFinder();
        List<Type> puzzleTypes;
        
        if (options.Year.HasValue)
        {
            if (options.Day.HasValue)
            {
                throw new NotImplementedException();
            }

            puzzleTypes = puzzleFinder.GetPuzzlesForYear(options.Year.Value).ToList();
        }
        else
        {
            var date = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-5));
            puzzleTypes = puzzleFinder.GetPuzzlesForDate(date).ToList();
        }
        
        Console.WriteLine($"Found {puzzleTypes.Count} puzzles: {string.Join(", ", puzzleTypes.Select(x => x.Name))}");
        Console.WriteLine();

        var runner = new Runner();
        
        foreach (var puzzleType in puzzleTypes)
        {
            runner.RunImplementation(puzzleType);
            Console.WriteLine();
        }
    }
    
}