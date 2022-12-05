using System.Text.RegularExpressions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 17, Part = 1)]
public class Day17A : Puzzle
{
    private int _x1;
    private int _x2;
    private int _y1;
    private int _y2;

    public override Output SampleExpectedOutput => new Answer(45, 9);
    public override Output ProblemExpectedOutput => new Answer(23005, 214);

    [Input("day17-extra.txt")]
    public Output ExtraExpectedOutput => 66; 

    public override void ParseInput(TextReader reader)
    {
        var line = reader.ReadLine()!;
        var match = Regex.Match(line, "^target area: x=(?<x1>-?\\d+)\\.\\.(?<x2>-?\\d+), y=(?<y1>-?\\d+)\\.\\.(?<y2>-?\\d+)");

        _x1 = int.Parse(match.Groups["x1"].Value);
        _x2 = int.Parse(match.Groups["x2"].Value);
        _y1 = int.Parse(match.Groups["y1"].Value);
        _y2 = int.Parse(match.Groups["y2"].Value);

        if (_y2 > _y1)
        {
            (_y2, _y1) = (_y1, _y2);
        }
        
        if (_x2 < _x1)
        {
            (_x2, _x1) = (_x1, _x2);
        }
    }

    public override Output Run()
    {
        // The probe needs to eventually reach _x1, so the lower bound for horizontal velocity
        // is n where n + ... + 2 + 1 = _x2. Upper bound is easy; reaching _x2 in a single step.
        // Some values in between are not valid, because the horizontal position will be
        // less than _x1 one step, and greater than _x2 the next. We filter those first.
        var possibleDeltaX = Enumerable.Range(MathEx.InverseTermialUnsafe(_x1), _x2).Where(SimulateX).ToList();
        foreach (var dX in possibleDeltaX)
        {
            // Lower bound for vertical velocity is also easy; reaching _y2 in a single step.
            // The upper bound I don't understand well enough to explain here ;)
            for (int dY = -_y2 - _y2 - 1; dY >= _y2; dY--)
            {
                var (success, maxY) = Simulate(dX, dY);
                if (success)
                {
                    return new Answer(maxY, MathEx.InverseTermial(maxY));    
                }
            }
        }

        throw new Exception("No answer found");
    }
    
    private bool SimulateX(int dX)
    {
        int x = 0;

        while (true)
        {
            x += dX;
            if (x < _x1 && dX == 0) return false;
            if (x > _x2) return false;
            if (x >= _x1 && x <= _x2) return true;
            dX -= Math.Sign(dX);
        }
    }    

    private (bool success, int maxY) Simulate(int dX, int dY)
    {
        int x = 0;
        int y = 0;

        int maxY = 0;
        
        while (true)
        {
            x += dX;
            y += dY;
            maxY = Math.Max(y, maxY);

            if (x < _x1 && dX == 0) return (false, maxY);
            if (x > _x2) return (false, maxY);
            if (y < _y2) return (false, maxY);            
            if (y <= _y1 && y >= _y2 && x >= _x1 && x <= _x2)
            {
                return (true, maxY);
            }            
            
            dX -= Math.Sign(dX);
            dY--;
        }
    }

    private record Answer(long Value, int DeltaY) : LongOutput(Value);
}