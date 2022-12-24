using System.Runtime.CompilerServices;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Pathfinding;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 24, Part = 2)]
public class Day24B : Puzzle
{
    private int _width;
    private int _height;
    private char[,] _grid;

    private Pos _start;
    private Pos _end;
    private bool[][,] _masksHor;
    private bool[][,] _masksVer;

    public override void ParseInput(TextReader reader)
    {
        var lines = reader.ToLineArray();

        _grid = new char[lines[0].Length, lines.Length];

        for (int y = 0; y < _grid.GetLength(1); y++)
        {
            for (int x = 0; x < _grid.GetLength(0); x++)
            {
                _grid[x, y] = lines[y][x];
            }
        }

        _start = new Pos(1, 0, 0);
        _end = new Pos(_grid.GetLength(0) - 2, _grid.GetLength(1) - 1, 0);
    }

    public override Output SampleExpectedOutput => 54;

    public override Output ProblemExpectedOutput => 842;


    public override Output Run()
    {
        _width = _grid.GetLength(0);
        _height = _grid.GetLength(1);
        _masksHor = MoveBlizzardsHorizontal();
        _masksVer = MoveBlizzardsVertical();

        var aStar = new AStar<Pos>(AdjacentNodes);

        aStar.TryPath(_start, pos => pos.X == _end.X && pos.Y == _end.Y, pos => Math.Abs(pos.X - _end.X) + Math.Abs(pos.Y - _end.Y), out var result);

        var goal1 = result.Path[^1];

        aStar.TryPath(goal1, pos => pos.X == _start.X && pos.Y == _start.Y, pos => Math.Abs(pos.X - _start.X) + Math.Abs(pos.Y - _start.Y), out var result2);

        var back = result2.Path[^1];

        aStar.TryPath(back, pos => pos.X == _end.X && pos.Y == _end.Y, pos => Math.Abs(pos.X - _start.X) + Math.Abs(pos.Y - _start.Y), out var result3);

        var final = result3.Path[^1];

        return final.Minute;
    }


    private void DisplayMasks()
    {
        for (int m = 0; m <= 18; m++)
        {
            var maskHor = _masksHor[m % _masksHor.Length];
            var maskVer = _masksVer[m % _masksVer.Length];

            Console.WriteLine($"Minute {m}");

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    //Console.Write((maskHor[x, y] || maskVer[x, y]) ? '@' : '.');

                    char c = (maskHor[x, y], maskVer[x, y]) switch
                    {
                        (true, true) => '2',
                        (true, false) => '>',
                        (false, true) => 'v',
                        (false, false) => '.'
                    };
                    Console.Write(c);

                    //Console.Write(maskHor[x, y] ? '>' : maskVer[x, y] ? 'v' : '.');

                    //Console.Write(maskHor[x, y] ? '>' : maskVer[x, y] ? 'v' : '.');
                    //Console.Write(maskVer[x, y] ? 'v' : '.');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsBlocked(int x, int y, int minute)
    {
        return _masksHor[minute % _masksHor.Length][x, y] || _masksVer[minute % _masksVer.Length][x, y];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsBlocked(Pos pos)
    {
        if (pos.X >= _width || pos.Y >= _height)
        {

        }

        var a = _masksHor[pos.Minute % _masksHor.Length];
        var b = _masksVer[pos.Minute % _masksVer.Length];

        try
        {
            return _masksHor[pos.Minute % _masksHor.Length][pos.X, pos.Y] ||
                   _masksVer[pos.Minute % _masksVer.Length][pos.X, pos.Y];
        }
        catch (Exception e)
        {

        }

        return false;
    }

    //protected IEnumerable<(Pos, int)> AdjacentNodes(Pos pos)
    //{
    //    if (pos.X > 0) yield return (_posGrid[pos.X - 1, pos.Y], _grid[pos.X - 1, pos.Y]);
    //    if (pos.Y > 0) yield return (_posGrid[pos.X, pos.Y - 1], _grid[pos.X, pos.Y - 1]);
    //    if (pos.X < _width - 1) yield return (_posGrid[pos.X + 1, pos.Y], _grid[pos.X + 1, pos.Y]);
    //    if (pos.Y < _height - 1) yield return (_posGrid[pos.X, pos.Y + 1], _grid[pos.X, pos.Y + 1]);
    //}

    protected IEnumerable<(Pos, int)> AdjacentNodes(Pos pos)
    {
        var right = new Pos(pos.X + 1, pos.Y, pos.Minute + 1);
        if (!IsBlocked(right))
        {
            yield return (right, 1);
        }

        if (pos.Y < _height - 1)
        {
            var down = new Pos(pos.X, pos.Y + 1, pos.Minute + 1);
            if (!IsBlocked(down))
            {
                yield return (down, 1);
            }
        }

        var wait = new Pos(pos.X, pos.Y, pos.Minute + 1);
        if (!IsBlocked(wait))
        {
            yield return (wait, 1);
        }

        var left = new Pos(pos.X - 1, pos.Y, pos.Minute + 1);
        if (!IsBlocked(left))
        {
            yield return (left, 1);
        }

        if (pos.Y >= 1)
        {
            var up = new Pos(pos.X, pos.Y - 1, pos.Minute + 1);
            if (!IsBlocked(up))
            {
                yield return (up, 1);
            }
        }
    }


    public readonly record struct Pos(int X, int Y, int Minute)
    {
        public override int GetHashCode()
        {
            return X << 16 | Y;
        }
    }

    private bool[][,] MoveBlizzardsHorizontal()
    {
        var result = new bool[_width - 2][,];

        for (int m = 0; m < _width - 2; m++)
        {
            var mask = new bool[_width, _height];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    switch (_grid[x, y])
                    {
                        case '#':
                            mask[x, y] = true;
                            break;
                        case '<':
                            mask[MathEx.Modulo(x - 1 - m, _width - 2) + 1, y] = true;
                            break;
                        case '>':
                            mask[MathEx.Modulo(x - 1 + m, _width - 2) + 1, y] = true;
                            break;
                    }
                }
            }

            result[m] = mask;
        }

        return result;
    }

    private bool[][,] MoveBlizzardsVertical()
    {
        var result = new bool[_height - 2][,];

        for (int m = 0; m < _height - 2; m++)
        {
            var mask = new bool[_width, _height];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    switch (_grid[x, y])
                    {
                        case '#':
                            mask[x, y] = true;
                            break;
                        case '^':
                            mask[x, MathEx.Modulo(y - 1 - m, _height - 2) + 1] = true;
                            break;
                        case 'v':
                            mask[x, MathEx.Modulo(y - 1 + m, _height - 2) + 1] = true;
                            break;
                    }
                }
            }

            result[m] = mask;
        }

        return result;
    }

}