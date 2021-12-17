using System.Text.RegularExpressions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 17, Part = 1)]
public class Day17A : Puzzle
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
        //int maxDownwardSpeed =

        int? found = int.MinValue;
        int maxY = 0;
        
        for (int dY = 0; dY < 10000; dY++)
        {
            var (success, y) = Simulate(0, dY);
            if (success)
            {
                found = dY;
                maxY = y;
            }
        }

        if (!found.HasValue)
        {
            throw new Exception();
        }
        
        return new Answer(maxY, found.Value);
    }

    private (bool, int) Simulate(int dX, int dY)
    {
        int x = 0;
        int y = 0;

        int maxY = 0;
        
        while (true)
        {
            x += dX;
            y += dY;
            maxY = Math.Max(y, maxY);

            if (y <= _y1 && y >= _y2)
            {
                return (true, maxY);
            }
            else if (y < _y2)
            {
                return (false, maxY);
            }
            
            dX = Math.Max(0, Math.Sign(dX) * (Math.Abs(dX) - 1));
            dY--;
        }
    }

    private record Answer(long Value, int DeltaY) : Output(Value);
}