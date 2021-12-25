using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 25, Part = 1)]
public class Day25A : Puzzle
{
    private char[,] _floor = null!;

    public override Output SampleExpectedOutput => 58;

    public override Output? ProblemExpectedOutput => 435;

    public override void ParseInput(TextReader reader)
    {
        var lines = reader.ToLineArray();

        _floor = new char[lines[0].Length, lines.Length];

        for (int y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                _floor[x, y] = line[x];
            }
        }
    }

    public override Output Run()
    {
        int width = _floor.GetLength(0);
        int height = _floor.GetLength(1);

        int step = 0;
        
        while (true)
        {
            bool moved = false;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (_floor[x, y] != '>') continue;
                    int x2 = (x + 1) % width;
                    if (_floor[x2, y] == '.')
                    {
                        _floor[x2, y] = '>';
                        _floor[x, y] = ' ';
                        x++;
                        moved = true;
                    }
                }
            }
            
            Refill();
        
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (_floor[x, y] != 'v') continue;
                    int y2 = (y + 1) % height;
                    if (_floor[x, y2] == '.')
                    {
                        _floor[x, y2] = 'v';
                        _floor[x, y] = ' ';
                        moved = true;
                        y++;
                    }
                }
            }
            
            Refill();            

            step++;

            // Some inputs never stop completely, so put a maximum on number of steps
            if (!moved || step > 10000) break;
        }

        return step;
    }

    private void Refill()
    {
        int width = _floor.GetLength(0);
        int height = _floor.GetLength(1);        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_floor[x, y] == ' ')
                {
                    _floor[x, y] = '.';
                }
            }
        }        
    }

    private void Dump()
    {
        int width = _floor.GetLength(0);
        int height = _floor.GetLength(1);

        Console.WriteLine();
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write(_floor[x, y]);
            }
            Console.WriteLine();
        }
        
    }
}