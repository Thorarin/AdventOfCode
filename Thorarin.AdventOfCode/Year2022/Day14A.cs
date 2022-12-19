using MoreLinq;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Pathfinding;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 14, Part = 1)]
public class Day14A : Puzzle
{
    private char[,] _grid;
    private int _maxX;
    private int _maxY;

    public override void ParseInput(TextReader reader)
    {
        var traces = reader.AsLines()
            .Select(l => l.Split(" -> ")
                .Select(xy =>
                {
                    var splitCoordinates = xy.Split(',');
                    return (X: int.Parse(splitCoordinates[0]), Y: int.Parse(splitCoordinates[1]));
                }).ToList()).ToList();
        
        foreach (var trace in traces)
        {
            foreach (var c in trace)
            {
                _maxX = Math.Max(_maxX, c.X);
                _maxY = Math.Max(_maxY, c.Y);
            }
        }

        _maxX++;
        _maxY++;

        _grid = new char[_maxX + 1, _maxY + 1];

        foreach (var trace in traces)
        {
            foreach (var w in trace.Window(2))
            {
                int stepX = Math.Sign(w[1].X - w[0].X);
                int stepY = Math.Sign(w[1].Y - w[0].Y);

                var pos = w[0];

                do  
                {
                    _grid[pos.X, pos.Y] = '#';
                    pos = (pos.X + stepX, pos.Y + stepY);
                } while (pos != w[1]);

                _grid[w[1].X, w[1].Y] = '#';
            }
        }
    }

    public override Output SampleExpectedOutput => 24;

    public override Output ProblemExpectedOutput => 817;


    public override Output Run()
    {
        int count = 0;
        while (DropSand(500, 0))
        {
            count++;
        }

        RunContext.Out.WriteLine();
        for (int y = 0; y < _maxY; y++)
        {
            for (int x = 493; x < _maxX; x++)
            {
                RunContext.Out.Write(_grid[x, y] == 0 ? ' ' : _grid[x, y]);
            }
            RunContext.Out.WriteLine();
        }
        RunContext.Out.WriteLine();

        return count;
    }


    private bool DropSand(int x, int y)
    {
        if (y >= _maxY) return false;

        if (_grid[x, y + 1] == 0)
        {
            return DropSand(x, y + 1);
        }

        if (_grid[x - 1, y + 1] == 0)
        {
            return DropSand(x - 1, y + 1);
        }

        if (_grid[x + 1, y + 1] == 0)
        {
            return DropSand(x + 1, y + 1);
        }

        _grid[x, y] = 'o';
        return true;
    }

}