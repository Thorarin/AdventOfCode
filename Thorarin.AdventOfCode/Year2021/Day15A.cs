using Thorarin.AdventOfCode.Extensions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 15, Part = 1)]
public class Day15A : Day15Base
{
    public override Output SampleExpectedOutput => 40;

    public override Output ProblemExpectedOutput => 423;

    public override void ParseInput(TextReader reader)
    {
        var lines = reader.ToLineArray();

        int width = lines[0].Length;
        int height = lines.Length;
        var grid = new int[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = int.Parse(lines[y].Substring(x, 1));
            }
        }

        Grid = grid;
    }
}