using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Coordinates.TwoDimensional.Integer;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 23, Part = 2)]
public class Day23B : Day23Base
{
    public override Output SampleExpectedOutput => 20;
    public override Output ProblemExpectedOutput => 1023;

    public override Output Run(ISet<Pos> board)
    {
        var (_, rounds) = Simulate(board, 100_000);
        return rounds;
    }
}