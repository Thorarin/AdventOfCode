using System.Runtime.CompilerServices;
using Thorarin.AdventOfCode.Extensions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 14, Part = 2)]
public class Day14B : Puzzle
{
    private char[,] _grid;
    private int _maxX;
    private int _maxY;

    public override void ParseInput(TextReader reader)
    {
        var traces = ParseTracesFaster(reader);

        _maxY += 2;                                // Make some space for the floor
        _maxX = Math.Max(_maxX, 500 + _maxY) + 1;  // How much space would be needed if sand fell all the way down diagonally?
        _maxY++;                                   // Add one (also for X) to avoid bound checks

        _grid = new char[_maxX, _maxY];

        foreach (var trace in traces)
        {
            var pos = trace[0];

            foreach (var target in trace)
            {
                int stepX = Math.Sign(target.X - pos.X);
                int stepY = Math.Sign(target.Y - pos.Y);

                while (true)
                {
                    _grid[pos.X, pos.Y] = '#';
                    if (pos == target) break;
                    pos = (pos.X + stepX, pos.Y + stepY);
                }
            }
        }

        // Add floor
        for (int x = 0; x < _maxX; x++)
        {
            _grid[x, _maxY - 1] = '#';
        }
    }

    private List<List<(int X, int Y)>> ParseTraces(TextReader reader)
    {
        var traces = reader.AsLines()
            .Select(l => l.Split(" -> ")
                .Select(xy =>
                {
                    var splitCoordinates = xy.Split(',');
                    return (X: int.Parse(splitCoordinates[0]), Y: int.Parse(splitCoordinates[1]));
                }).ToList()).ToList();

        _maxX = traces.SelectMany(i => i).Select(pos => pos.X).Max();
        _maxY = traces.SelectMany(i => i).Select(pos => pos.Y).Max();

        foreach (var trace in traces)
        {
            foreach (var pos in trace)
            {
                _maxX = Math.Max(_maxX, pos.X);
                _maxY = Math.Max(_maxY, pos.Y);
            }
        }

        return traces;
    }

    private List<List<(int X, int Y)>> ParseTracesFaster(TextReader reader)
    {
        List<List<(int X, int Y)>> traces = new();
        ReadOnlySpan<char> separator = " -> ";

        foreach (var line in reader.AsLines())
        {
            var trace = new List<(int X, int Y)>();

            foreach (var coordinates in line.AsSpan().EnumerateSplit(separator))
            {
                var xyEnumerator = coordinates.EnumerateSplit(',');
                xyEnumerator.MoveNext();
                int x = int.Parse(xyEnumerator.Current);
                xyEnumerator.MoveNext();
                int y = int.Parse(xyEnumerator.Current);

                _maxX = Math.Max(_maxX, x);
                _maxY = Math.Max(_maxY, y);

                trace.Add((x, y));
            }

            traces.Add(trace);
        }

        return traces;
    }

    public override Output SampleExpectedOutput => 93;

    public override Output ProblemExpectedOutput => 23416;


    public override Output Run()
    {
        int count = 0;
        while (DropSand(500, 0))
        {
            count++;
        }

        //Console.WriteLine();
        //for (int y = 0; y < _maxY; y++)
        //{
        //    for (int x = 480; x < _maxX; x++)
        //    {
        //        Console.Write(_grid[x, y] == 0 ? ' ' : _grid[x, y]);
        //    }
        //    Console.WriteLine();
        //}
        //Console.WriteLine();

        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private bool DropSand(int x, int y)
    {
        if (_grid[x, y] == 'o') return false;

        //if (y >= _maxY)
        //{
        //    throw new Exception("Floor is supposed to be infinite?!?");
        //}

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