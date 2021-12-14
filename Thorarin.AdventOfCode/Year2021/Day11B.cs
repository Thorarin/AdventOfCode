using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 11, Part = 2)]
public class Day11B : Puzzle
{
    private int _sizeX;
    private int _sizeY;
    byte[,] _grid = new byte[0, 0];

    public override Output SampleExpectedOutput => 195;
    public override Output ProblemExpectedOutput => 247;

    public override void ParseInput(TextReader reader)
    {
        var fileLines = reader.ToLineArray();
        _sizeY = fileLines.Length;
        _sizeX = fileLines[0].Length; 
        _grid = new byte[_sizeY, _sizeX];
        
        for (var y = 0; y < _sizeY; y++)
        {
            for (var x = 0; x < _sizeX; x++)
            {
                _grid[y, x] = byte.Parse(fileLines[y].Substring(x, 1));
            }
        }
    }

    public override Output Run()
    {
        for (int step = 1;; step++)
        {
            int flashes = Step();

            if (flashes == 100)
            {
                return step;
            }
        }
    }
    
    private int Step()
    {
        for (var y = 0; y < _sizeY; y++)
        {
            for (var x = 0; x < _sizeX; x++)
            {
                Increment(y, x);
            }
        }

        int flashes = 0;
        
        for (var y = 0; y < _sizeY; y++)
        {
            for (var x = 0; x < _sizeX; x++)
            {
                if (_grid[y, x] > 9)
                {
                    _grid[y, x] = 0;
                    flashes++;
                }
            }
        }

        return flashes;
    }
    
    private void Increment(int y, int x)
    {
        switch (_grid[y, x])
        {
            case > 9:
                return;
            case < 9:
                _grid[y, x]++;
                break;
            case 9:
                _grid[y, x]++;
                int maxX = Math.Min(x + 1, _sizeX - 1);
                int maxY = Math.Min(y + 1, _sizeY - 1);
                for (int adjacentX = Math.Max(x - 1, 0); adjacentX <= maxX; adjacentX++)
                {
                    for (int adjacentY = Math.Max(y - 1, 0); adjacentY <= maxY; adjacentY++)
                    {
                        Increment(adjacentY, adjacentX);
                    }
                }
                break;
        }
    }    
}