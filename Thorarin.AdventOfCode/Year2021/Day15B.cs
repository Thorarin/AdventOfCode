using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 15, Part = 2)]
public class Day15B : Day15Base
{
    public override Output SampleExpectedOutput => 315;

    public override Output ProblemExpectedOutput => 2778;

    public override void ParseInput(TextReader reader)
    {
        var lines = reader.ToLineArray();

        int repeats = 5;
        int templateWidth = lines[0].Length;
        int templateHeight = lines.Length;
        var grid = new int[templateWidth * repeats, templateHeight * repeats];

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines.Length; x++)
            {
                grid[x, y] = int.Parse(lines[y].Substring(x, 1));
            }
        }

        var width = templateWidth * repeats;
        var height = templateHeight * repeats;
        
        // Use template to fill top row of 5 * 5 tiles
        for (int y = 0; y < templateHeight; y++)
        {
            for (int x = templateWidth; x < width; x++)
            {
                grid[x, y] = WrapRisk(grid[x - templateWidth, y]);
            }
        }
        
        // Then fill vertically
        for (int y = templateHeight; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = WrapRisk(grid[x, y - templateHeight]);
            }
        }

        int WrapRisk(int risk)
        {
            return (risk % 9) + 1;
        }

        Grid = grid;
    }
}