using System.Runtime.CompilerServices;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Pathfinding;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 12, Part = 2)]
public class Day12B : Puzzle
{
    private int _width;
    private int _height;
    private char[,] _grid;
    private Pos[,] _posGrid;

    private Pos _start;
    private Pos _end;

    public override void ParseInput(TextReader reader)
    {
        var lines = reader.ToLineArray();

        int templateWidth = lines[0].Length;
        int templateHeight = lines.Length;
        _grid = new char[templateWidth, templateHeight];

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < templateWidth; x++)
            {
                _grid[x, y] = lines[y][x];
            }
        }
    }

    public override Output SampleExpectedOutput => 29;

    public override Output ProblemExpectedOutput => 525;

    public override Output Run()
    {
        _width = _grid.GetLength(0);
        _height = _grid.GetLength(1);

        _posGrid = new Pos[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                switch (_grid[x, y])
                {
                    case 'S':
                        _posGrid[x, y] = new Pos(x, y, 0);
                        _start = _posGrid[x, y];
                        break;
                    case 'E':
                        _posGrid[x, y] = new Pos(x, y, 25);
                        _end = _posGrid[x, y];
                        break;
                    default:
                        _posGrid[x, y] = new Pos(x, y, _grid[x, y] - 'a');
                        break;
                }
            }
        }

        var aStar = new AStar<Pos>(AdjacentNodes);

        bool success = aStar.TryPath(_end, pos => pos.Height == 0, pos => 25 - pos.Height, out var result);

        return result.Path.Length;
    }

    protected IEnumerable<(Pos, int)> AdjacentNodes(Pos pos)
    {
        if (pos.X > 0)
        {
            var p = Get(pos.X - 1, pos.Y);
            if (p.Item1.Height - pos.Height >= -1)
            {
                yield return p;
            }
        }

        if (pos.Y > 0)
        {
            var p = Get(pos.X, pos.Y - 1);
            if (p.Item1.Height - pos.Height >= -1)
            {
                yield return p;
            }
        }

        if (pos.X < _width - 1)
        {
            var p = Get(pos.X + 1, pos.Y);
            if (p.Item1.Height - pos.Height >= -1)
            {
                yield return p;
            }
        }

        if (pos.Y < _height - 1)
        {
            var p = Get(pos.X, pos.Y + 1);
            if (p.Item1.Height - pos.Height >= -1)
            {
                yield return p;
            }
        }

        (Pos, int) Get(int x, int y)
        {
            var newPos = _posGrid[x, y];
            return (newPos, -pos.Height + newPos.Height + 2);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]

    protected IEnumerable<(Pos Pos, int Cost)> AdjacentNodes2(Pos pos)
    {
        var adjacent = Get(pos.X - 1, pos.Y);
        if (adjacent.HasValue)
        {
            yield return adjacent.Value;
        }

        adjacent = Get(pos.X, pos.Y - 1);
        if (adjacent.HasValue)
        {
            yield return adjacent.Value;
        }

        adjacent = Get(pos.X + 1, pos.Y);
        if (adjacent.HasValue)
        {
            yield return adjacent.Value;
        }

        adjacent = Get(pos.X, pos.Y + 1);
        if (adjacent.HasValue)
        {
            yield return adjacent.Value;
        }

        (Pos Pos, int Cost)? Get(int x, int y)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height)
            {
                return null;
            }

            var newPos = _posGrid[x, y];
            var cost = -pos.Height + newPos.Height + 2;
            if (cost <= 0)
            {
                return null;
            }

            return (newPos, cost);
        }
    }

    public readonly record struct Pos(int X, int Y, int Height)
    {
        public override int GetHashCode()
        {
            return X << 16 | Y;
        }
    }

}