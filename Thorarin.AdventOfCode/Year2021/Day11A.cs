using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 11, Part = 1)]
public class Day11A : Puzzle
{
    private int sizeX;
    private int sizeY;
    byte[,] grid;

    public override Output SampleExpectedOutput => 1656;
    public override Output ProblemExpectedOutput => 1793;

    public override void ParseInput(TextReader reader)
    {
        var fileLines = reader.ToLineArray();
        sizeY = fileLines.Length;
        sizeX = fileLines[0].Length; 
        grid = new byte[sizeY, sizeX];
        
        for (var y = 0; y < sizeY; y++)
        {
            for (var x = 0; x < sizeX; x++)
            {
                grid[y, x] = byte.Parse(fileLines[y].Substring(x, 1));
            }
        }
    }

    public override Output Run()
    {
        int flashes = 0;
        for (int i = 0; i < 100; i++)
        {
            flashes += Step();
        }

        return flashes;
    }
    
    private int Step()
    {
        for (var y = 0; y < sizeY; y++)
        {
            for (var x = 0; x < sizeX; x++)
            {
                Increment(y, x);
            }
        }

        int flashes = 0;
        
        for (var y = 0; y < sizeY; y++)
        {
            for (var x = 0; x < sizeX; x++)
            {
                if (grid[y, x] > 9)
                {
                    grid[y, x] = 0;
                    flashes++;
                }
            }
        }

        return flashes;
    }
    
    private void Increment(int y, int x)
    {
        switch (grid[y, x])
        {
            case > 9:
                return;
            case < 9:
                grid[y, x]++;
                break;
            case 9:
                grid[y, x]++;
                for (int adjacentX = Math.Max(x - 1, 0); adjacentX <= Math.Min(x + 1, 9); adjacentX++)
                {
                    for (int adjacentY = Math.Max(y - 1, 0); adjacentY <= Math.Min(y + 1, 9); adjacentY++)
                    {
                        Increment(adjacentY, adjacentX);
                    }
                }
                break;
        }
    }    
}