using System.Reflection;

namespace Thorarin.AdventOfCode.Framework;

public class PuzzleFinder
{
    private ILookup<PuzzleIdentifier, Type> Puzzles { get; }
    
    public PuzzleFinder()
    {
        Puzzles = typeof(Program).Assembly.GetTypes()
            .Where(type => typeof(IPuzzle).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
            .ToLookup(
                type => new PuzzleIdentifier(type.GetCustomAttribute<PuzzleAttribute>()!),
                type => type);        
    }

    public IEnumerable<Type> GetPuzzlesForDate(DateOnly date)
    {
        var part1 = new PuzzleIdentifier(date.Year, date.Day, 1);
        var part2 = new PuzzleIdentifier(date.Year, date.Day, 2);

        return Puzzles[part1].Concat(Puzzles[part2]);
    }
    
    public IEnumerable<Type> GetPuzzlesForYear(int year)
    {
        return Puzzles
            .Where(grouping => grouping.Key.Year == year)
            .OrderBy(grouping => grouping.Key)
            .SelectMany(grouping => grouping);
    }

    public IEnumerable<Type> GetPuzzlesForDay(int year, int day)
    {
        return Puzzles
            .Where(grouping => grouping.Key.Year == year && grouping.Key.Day == day)
            .OrderBy(grouping => grouping.Key)
            .SelectMany(grouping => grouping);
    }

    public IEnumerable<Type> GetByImplementationName(string implementationName)
    {
        return Puzzles
            .OrderBy(grouping => grouping.Key)
            .SelectMany(grouping => grouping)
            .Where(x => x.Name == implementationName);
    }
}