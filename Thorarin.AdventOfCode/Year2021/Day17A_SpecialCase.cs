using System.Text.RegularExpressions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 17, Part = 1)]
public class Day17A_SpecialCase : Puzzle
{
    private int _y1;
    private int _y2;

    public override Output SampleExpectedOutput => new Answer(45, 9);
    public override Output ProblemExpectedOutput => new Answer(23005, 214);

    public override void ParseInput(TextReader reader)
    {
        var line = reader.ReadLine()!;
        var match = Regex.Match(line, "^target area: x=(?<x1>-?\\d+)\\.\\.(?<x2>-?\\d+), y=(?<y1>-?\\d+)\\.\\.(?<y2>-?\\d+)");

        _y1 = int.Parse(match.Groups["y1"].Value);
        _y2 = int.Parse(match.Groups["y2"].Value);

        if (_y2 > _y1)
        {
            (_y2, _y1) = (_y1, _y2);
        }
    }

    public override Output Run()
    {
        var deltaY = -_y2 - 1;
        var maxY = MathEx.Termial(deltaY);
        
        return new Answer(maxY, deltaY);        
    }

    private record Answer(long Value, int DeltaY) : LongOutput(Value);
}