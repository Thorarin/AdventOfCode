using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 25, Part = 1)]
public class Day25A : Puzzle
{
    private Floor[,] _floor = null!;

    public override Output SampleExpectedOutput => 58;

    public override Output? ProblemExpectedOutput => 435;

    private enum Floor : byte
    {
        Empty = 0,
        East = 1,
        South = 2
    }

    public override void ParseInput(TextReader reader)
    {
        var lines = reader.ToLineArray();

        _floor = new Floor[lines[0].Length, lines.Length];

        for (int y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                _floor[x, y] = ConvertToByte(line[x]);
            }
        }

        Floor ConvertToByte(char c)
        {
            return c switch
            {
                '.' => Floor.Empty,
                '>' => Floor.East,
                'v' => Floor.South
            };
        }
    }

    public override Output Run()
    {
        // Some inputs never stop completely, so put a limit on number of steps
        for (int step = 1; step < 10000; step++)
        {
            if (!(MoveEast(_floor) | MoveSouth(_floor)))
                return step;
        }

        return -1;
    }

    private bool MoveSouth(Floor[,] floor)
    {
        int width = floor.GetLength(0);
        int height = floor.GetLength(1);
        
        bool moved = false;
        for (int x = 0; x < width; x++)
        {
            bool wrap = floor[x, 0] == Floor.Empty && floor[x, height - 1] == Floor.South;
            
            for (int y = 0; y < height - 1; y++)
            {
                if (floor[x, y] != Floor.South) continue;
                int y2 = y + 1;
                if (floor[x, y2] == Floor.Empty)
                {
                    floor[x, y2] = Floor.South;
                    floor[x, y] = Floor.Empty;
                    moved = true;
                    y++;
                }
            }

            if (wrap)
            {
                floor[x, 0] = Floor.South;
                floor[x, height - 1] = Floor.Empty;
            }
        }

        return moved;
    }

    private bool MoveEast(Floor[,] floor)
    {
        int width = floor.GetLength(0);
        int height = floor.GetLength(1);
        
        bool moved = false;
        
        for (int y = 0; y < height; y++)
        {
            bool wrap = floor[0, y] == Floor.Empty && floor[width - 1, y] == Floor.East;

            for (int x = 0; x < width - 1; x++)
            {
                if (floor[x, y] != Floor.East) continue;
                int x2 = x + 1;
                if (floor[x2, y] == Floor.Empty)
                {
                    floor[x2, y] = Floor.East;
                    floor[x, y] = Floor.Empty;
                    moved = true;
                    x++;
                }
            }
            
            if (wrap)
            {
                floor[0, y] = Floor.East;
                floor[width - 1, y] = Floor.Empty;
            }
        }

        return moved;
    }
}