using System.Reflection;

namespace Thorarin.AdventOfCode.Framework;

public class PuzzleFinder
{
    private ILookup<PuzzleIdentifier, Type> Puzzles { get; }
    
    public PuzzleFinder()
    {
        Puzzles = typeof(Program).Assembly.GetTypes()
            .Where(type => typeof(IPuzzle).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract && type.GetCustomAttribute<PuzzleAttribute>() != null)
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

    public PuzzleFinderQueryBuilder Query()
    {
        return new PuzzleFinderQueryBuilder(Puzzles);
    }

    public class PuzzleFinderQueryBuilder
    {
        private int? _year;
        private int? _day;
        private string? _name;
        private ILookup<PuzzleIdentifier, Type> Puzzles { get; }

        protected internal PuzzleFinderQueryBuilder(ILookup<PuzzleIdentifier, Type> puzzles)
        {
            Puzzles = puzzles;
        }

        public PuzzleFinderQueryBuilder ForYear(int year)
        {
            _year = year;

            return this;
        }

        public PuzzleFinderQueryBuilder ForDay(int day)
        {
            _day = day;

            return this;
        }

        public PuzzleFinderQueryBuilder ForDay(int year, int day)
        {
            _year = year;
            _day = day;

            return this;
        }

        public PuzzleFinderQueryBuilder WithName(string name)
        {
            _name = name;

            return this;
        }

        public IEnumerable<Type> Find()
        {
            IEnumerable<IGrouping<PuzzleIdentifier, Type>> query;

            if (_year.HasValue && _day.HasValue)
            {
                query = Puzzles
                    .Where(grouping => grouping.Key.Year == _year && grouping.Key.Day == _day);
            }
            else if (_year.HasValue)
            {
                query = Puzzles
                    .Where(grouping => grouping.Key.Year == _year);
            }
            else if (_day.HasValue)
            {
                query = Puzzles
                    .Where(grouping => grouping.Key.Day == _day);
            }
            else
            {
                query = Puzzles;
            }

            var query2 = query
                .OrderBy(grouping => grouping.Key)
                .SelectMany(grouping => grouping);

            if (_name != null)
            {
                query2 = query2
                    .Where(x => x.Name == _name);
            }

            return query2;
        }
    }
}