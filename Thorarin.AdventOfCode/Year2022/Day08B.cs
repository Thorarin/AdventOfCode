using MoreLinq;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 8, Part = 2)]
public class Day08B : Puzzle
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

    public override Output SampleExpectedOutput => 8;

    public override Output ProblemExpectedOutput => 496125;

    public override Output Run()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _grid[x, y].Score = CalculateScore(x, y);
            }
        }

        return _grid.Flatten().Max(p => ((Pos)p).Score);
    }

    private int CalculateScore(int x, int y)
    {
        int height = _grid[x, y].Height;

        int left = 0;
        for (int i = x - 1; i >= 0; i--)
        {
            left++;
            if (_grid[i, y].Height >= height) break;
        }

        int right = 0;
        for (int i = x + 1; i < _width; i++)
        {
            right++;
            if (_grid[i, y].Height >= height) break;
        }

        int up = 0;
        for (int i = y - 1; i >= 0; i--)
        {
            up++;
            if (_grid[x, i].Height >= height) break;
        }

        int down = 0;
        for (int i = y + 1; i < _height; i++)
        {
            down++;
            if (_grid[x, i].Height >= height) break;
        }

        return left * right * up * down;
    }

    public record Pos(int Height)
    {
        public int Score { get; set; }
    }
}