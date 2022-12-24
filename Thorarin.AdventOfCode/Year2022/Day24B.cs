using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Pathfinding;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 24, Part = 2)]
public class Day24B : Day24Base
{
    public override Output SampleExpectedOutput => 54;

    public override Output ProblemExpectedOutput => 842;

    public override Output Run()
    {
        var start = new Pos(1, 0, 0);
        var goal = new Pos(Width - 2, Height - 1, 0);

        var aStar = new AStar<Pos>(AdjacentNodes);
        aStar.TryPath(start, pos => pos.X == goal.X && pos.Y == goal.Y, pos => Math.Abs(pos.X - goal.X) + Math.Abs(pos.Y - goal.Y), out var firstTrip);
        var goal1 = firstTrip.Path[^1];

        aStar.TryPath(goal1, pos => pos.X == start.X && pos.Y == start.Y, pos => Math.Abs(pos.X - start.X) + Math.Abs(pos.Y - start.Y), out var returnTrip);
        var back = returnTrip.Path[^1];

        aStar.TryPath(back, pos => pos.X == goal.X && pos.Y == goal.Y, pos => Math.Abs(pos.X - goal.X) + Math.Abs(pos.Y - goal.Y), out var finalTrip);
        var final = finalTrip.Path[^1];

        return final.Minute;
    }

    protected IEnumerable<(Pos, int)> AdjacentNodes(Pos pos)
    {
        var right = new Pos(pos.X + 1, pos.Y, pos.Minute + 1);
        if (!IsBlocked(right))
        {
            yield return (right, 1);
        }

        if (pos.Y < Height - 1)
        {
            var down = new Pos(pos.X, pos.Y + 1, pos.Minute + 1);
            if (!IsBlocked(down))
            {
                yield return (down, 1);
            }
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

        if (pos.Y >= 1)
        {
            var up = new Pos(pos.X, pos.Y - 1, pos.Minute + 1);
            if (!IsBlocked(up))
            {
                yield return (up, 1);
            }
        }
    }
}