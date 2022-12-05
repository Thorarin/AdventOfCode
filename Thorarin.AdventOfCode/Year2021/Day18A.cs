using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Year2021.Day18;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 18, Part = 1)]
public class Day18A : Puzzle
{
    private Queue<Number> _numbers;

    public override Output SampleExpectedOutput => 4140;

    public override Output ProblemExpectedOutput => 4469;

    public override void ParseInput(TextReader reader)
    {
        _numbers = new();
        foreach (var line in reader.AsLines())
        {
            _numbers.Enqueue(Number.Parse(line));
        }
    }

    public override Output Run()
    {
        var zeNumber = _numbers.Dequeue();

        while (_numbers.Count > 0)
        {
            zeNumber += _numbers.Dequeue();
        }

        return new Answer(zeNumber.GetMagnitude(), zeNumber.ToString());
    }

    private record Answer(long Value, string Number) : LongOutput(Value);
}