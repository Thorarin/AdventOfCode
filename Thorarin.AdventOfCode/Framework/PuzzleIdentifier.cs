namespace Thorarin.AdventOfCode.Framework;

public readonly struct PuzzleIdentifier : IComparable<PuzzleIdentifier>
{
    public PuzzleIdentifier(int year, int day, int part)
    {
        if (part is < 1 or > 2)
        {
            throw new ArgumentOutOfRangeException(nameof(part), part, "Part should be either 1 or 2.");
        }
        
        Year = year;
        Day = day;
        Part = part;
    }

    public PuzzleIdentifier(PuzzleAttribute metadata)
    {
        Year = metadata.Year;
        Day = metadata.Day;
        Part = metadata.Part;
    }

    public int Year { get; }
    public int Day { get; }
    public int Part { get; }

    #region Relational members
    public int CompareTo(PuzzleIdentifier other)
    {
        var yearComparison = Year.CompareTo(other.Year);
        if (yearComparison != 0) return yearComparison;
        var dayComparison = Day.CompareTo(other.Day);
        if (dayComparison != 0) return dayComparison;
        return Part.CompareTo(other.Part);
    }

    public static bool operator <(PuzzleIdentifier left, PuzzleIdentifier right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(PuzzleIdentifier left, PuzzleIdentifier right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(PuzzleIdentifier left, PuzzleIdentifier right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(PuzzleIdentifier left, PuzzleIdentifier right)
    {
        return left.CompareTo(right) >= 0;
    }
    #endregion
}