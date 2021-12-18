using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Year2021.Day18;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 18, Part = 1)]
public class Day18A : Puzzle
{
    private Queue<INumber> _numbers;

    public override Output SampleExpectedOutput => 4140;

    public override Output ProblemExpectedOutput => 4469;

    public override void ParseInput(TextReader reader)
    {
        _numbers = new();
        foreach (var line in reader.AsLines())
        {
            var lineReader = new StringReader(line);

            _numbers.Enqueue(NumberPair.Parse(lineReader));
        }
    }

    public override Output Run()
    {
        var zeNumber = _numbers.Dequeue();

        while (_numbers.Count > 0)
        {
            zeNumber = zeNumber.Add(_numbers.Dequeue());
            zeNumber.Reduce();
        }

        return new Answer(zeNumber.Magnitude(), zeNumber.ToString());
    }

    private record Answer(long Value, string Number) : Output(Value);
}