using MoreLinq;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 8, Part = 1)]
public class Day08A : Puzzle
{
    private Pos[,] _grid;
    private int _width;
    private int _height;

    public override void ParseInput(TextReader reader)
    {
        var lines = reader.AsLines().ToList();

        _width = lines[0].Length;
        _height = lines.Count;

        _grid = new Pos[_width, _height];

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                _grid[x, y] = new Pos(lines[y][x] - 48);
            }
        }
    }

    public override Output SampleExpectedOutput => 21;

    public override Output ProblemExpectedOutput => 1538;

    public override Output Run()
    {
        for (int x = 0; x < _width; x++)
        {
            int height = -1;
            for (int y = 0; y < _height; y++)
            {
                if (_grid[x, y].Height > height)
                {
                    _grid[x, y].Visible = true;
                    height = _grid[x, y].Height;
                }
            }

            height = -1;
            for (int y = _height - 1; y >= 0; y--)
            {
                if (_grid[x, y].Height > height)
                {
                    _grid[x, y].Visible = true;
                    height = _grid[x, y].Height;
                }
            }
        }

        for (int y = 0; y < _height; y++)
        {
            int height = -1;
            for (int x = 0; x < _width; x++)
            {
                if (_grid[x, y].Height > height)
                {
                    _grid[x, y].Visible = true;
                    height = _grid[x, y].Height;
                }
            }

            height = -1;
            for (int x = _width - 1; x >= 0; x--)
            {
                if (_grid[x, y].Height > height)
                {
                    _grid[x, y].Visible = true;
                    height = _grid[x, y].Height;
                }
            }
        }

        return _grid.Flatten().Count(p => ((Pos)p).Visible);
    }

    public record Pos(int Height)
    {
        public bool Visible { get; set; }
    }
}