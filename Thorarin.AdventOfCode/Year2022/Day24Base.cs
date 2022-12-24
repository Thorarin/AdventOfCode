using System.Runtime.CompilerServices;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

public abstract class Day24Base : Puzzle
{
    private int _width;
    private int _height;
    private bool[][,] _masksHor;
    private bool[][,] _masksVer;

    protected int Width => _width;
    protected int Height => _height;

    public override void ParseInput(TextReader reader)
    {
        var lines = reader.ToLineArray();

        _width = lines[0].Length;
        _height = lines.Length;

        _masksHor = BlizzardMasksHorizontal(lines);
        _masksVer = BlizzardMasksVertical(lines);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool IsBlocked(Pos pos)
    {
        return _masksHor[pos.Minute % _masksHor.Length][pos.X, pos.Y] ||
               _masksVer[pos.Minute % _masksVer.Length][pos.X, pos.Y];
    }

    protected readonly record struct Pos(int X, int Y, int Minute);

    private bool[][,] BlizzardMasksHorizontal(string[] lines)
    {
        var result = new bool[Width - 2][,];

        for (int m = 0; m < Width - 2; m++)
        {
            var mask = new bool[Width, _height];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    switch (lines[y][x])
                    {
                        case '#':
                            mask[x, y] = true;
                            break;
                        case '<':
                            mask[MathEx.Modulo(x - 1 - m, Width - 2) + 1, y] = true;
                            break;
                        case '>':
                            mask[MathEx.Modulo(x - 1 + m, Width - 2) + 1, y] = true;
                            break;
                    }
                }
            }

            result[m] = mask;
        }

        return result;
    }

    private bool[][,] BlizzardMasksVertical(string[] lines)
    {
        var result = new bool[_height - 2][,];

        for (int m = 0; m < _height - 2; m++)
        {
            var mask = new bool[Width, _height];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    switch (lines[y][x])
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