using System.Text.RegularExpressions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 17, Part = 2)]
public class Day17B : Puzzle
{
    private int _x1;
    private int _x2;
    private int _y1;
    private int _y2;

    public override Output SampleExpectedOutput => 112;

    public override Output ProblemExpectedOutput => 2040;
    
    [Input("day17-extra.txt")]
    public Output ExtraExpectedOutput => new Answer(820, MaxY: 66, DeltaY: 11); 
    
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
        int found = 0;
        var possibleX = Enumerable.Range(MathEx.InverseTermialUnsafe(_x1), _x2).Where(SimulateX).ToList();
        var possibleY = Enumerable.Range(_y2, -_y2 - _y2).Where(SimulateY).ToList();

        int maxDeltaY = int.MinValue;
        foreach (int dX in possibleX)
        {
            foreach (int dY in possibleY)
            {
                if (Simulate(dX, dY))
                {
                    found++;
                    maxDeltaY = Math.Max(maxDeltaY, dY);
                }
            }
        }

        int maxY = MathEx.Termial(maxDeltaY);

        return  new Answer(found, maxY, maxDeltaY);
    }

    private bool Simulate(int dX, int dY)
    {
        int x = 0;
        int y = 0;
        
        while (true)
        {
            x += dX;
            y += dY;

            if (x < _x1 && dX == 0) return false;
            if (x > _x2) return false;
            if (y < _y2) return false;

            if (y <= _y1 && y >= _y2 && x >= _x1 && x <= _x2)
            {
                return true;
            }

            dX -= Math.Sign(dX);
            dY--;
        }
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
    
    private bool SimulateY(int dY)
    {
        int y = 0;
        while (y >= _y2)
        {
            y += dY;
            if (y <= _y1 && y >= _y2) return true;
            dY--;
        }

        return false;
    }

    private record Answer(long Value, int MaxY, int DeltaY) : LongOutput(Value);
}