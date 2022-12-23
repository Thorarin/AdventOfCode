using Thorarin.AdventOfCode.Coordinates.TwoDimensional.Integer;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 23, Part = 1)]
public class Day23A : Day23Base
{
    public override Output SampleExpectedOutput => 110;

    public override Output ProblemExpectedOutput => 4236;

    public override Output Run(ISet<Pos> board)
    {
        (board, _) = Simulate(board, 10);

        return CountEmptyPositions(board);
    }

    private int CountEmptyPositions(ISet<Pos> board)
    {
        var (minX, minY, maxX, maxY) = GetBounds(board);

        int count = 0;
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (!board.Contains((x, y))) count++;
            }
        }

        return count;
    }
}


