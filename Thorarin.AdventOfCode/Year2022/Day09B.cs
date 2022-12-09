using MoreLinq;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 9, Part = 2)]
public class Day09B : Puzzle
{
    private List<string> _lines;

    public override void ParseInput(TextReader reader)
    {
        _lines = reader.AsLines().ToList();
    }

    public override Output SampleExpectedOutput => 1;

    [Input("day09-sample2.txt")]
    public Output LargerSampleExpectedOutput => 36;

    public override Output ProblemExpectedOutput => 2593;


    public override Output Run()
    {
        var knots = new Point[10];
        HashSet<Point> positions = new();

        positions.Add(knots[9]);

        foreach (var line in _lines)
        {
            int times = int.Parse(line.Substring(2));

            switch (line[0])
            {
                case 'R':
                    MoveMultiple(1, 0, times);
                    break;
                case 'U':
                    MoveMultiple(0, -1, times);
                    break;
                case 'L':
                    MoveMultiple(-1, 0, times);
                    break;
                case 'D':
                    MoveMultiple(0, 1, times);
                    break;
            }

        }

        void MoveMultiple(int deltaX, int deltaY, int times)
        {
            for (int i = 0; i < times; i++)
            {
                MoveKnot(deltaX, deltaY, 0);
            }
        }

        void MoveKnot(int deltaX, int deltaY, int knot)
        {
            var current = knots[knot] = knots[knot].Move(deltaX, deltaY);

            if (knot == knots.Length - 1)
            {
                positions.Add(current);
                return;
            }

            var next = knots[knot + 1];

            if (Math.Abs(current.X - next.X) > 1 || Math.Abs(current.Y - next.Y) > 1)
            {
                MoveKnot(
                    Step(next.X, current.X),
                    Step(next.Y, current.Y),
                    knot + 1);
            }
        }

        return positions.Count;
    }

   

    private static int Step(int from, int to)
    {
        return Math.Sign(to - from);
    }
    

    public record struct Point(int X, int Y)
    {
        public Point Move(int deltaX, int deltaY)
        {
            return new Point(X + deltaX, Y + deltaY);
        }
    }
}