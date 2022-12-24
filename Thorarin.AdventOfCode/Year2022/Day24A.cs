using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Pathfinding;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 24, Part = 1)]
public class Day24A : Day24Base
{
    public override Output SampleExpectedOutput => 18;

    public override Output ProblemExpectedOutput => 290;


    public override Output Run()
    {
        var start = new Pos(1, 0, 0);
        var goal = new Pos(Width - 2, Height - 1, 0);

        var aStar = new AStar<Pos>(AdjacentNodes);
        aStar.TryPath(start, pos => pos.X == goal.X && pos.Y == goal.Y, pos => Math.Abs(pos.X - goal.X) + Math.Abs(pos.Y - goal.Y), out var result);

        return result.Path.Length;
    }
    
    protected IEnumerable<(Pos, int)> AdjacentNodes(Pos pos)
    {
        var right = new Pos(pos.X + 1, pos.Y, pos.Minute + 1);
        if (!IsBlocked(right))
        {
            yield return (right, 1);
        }

        var down = new Pos(pos.X, pos.Y + 1, pos.Minute + 1);
        if (!IsBlocked(down))
        {
            yield return (down, 1);
        }

        var wait = new Pos(pos.X, pos.Y, pos.Minute + 1);
        if (!IsBlocked(wait))
        {
            yield return (wait, 1);
        }

        var left = new Pos(pos.X - 1, pos.Y, pos.Minute + 1);
        if (!IsBlocked(left))
        {
            yield return (left, 1);
        }

        if (pos.Y > 1)
        {
            var up = new Pos(pos.X, pos.Y - 1, pos.Minute + 1);
            if (!IsBlocked(up))
            {
                yield return (up, 1);
            }
        }
    }
}