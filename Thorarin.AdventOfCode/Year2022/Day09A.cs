using MoreLinq;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 9, Part = 1)]
public class Day09A : Puzzle
{
    private List<string> _lines;

    public override void ParseInput(TextReader reader)
    {
        _lines = reader.AsLines().ToList();
    }

    public override Output SampleExpectedOutput => 13;

    public override Output ProblemExpectedOutput => 6391;


    public override Output Run()
    {
        HashSet<Point> positions = new();

        Point head = new Point(0, 0);
        Point tail = new Point(0, 0);

        positions.Add(tail);

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
                Move(deltaX, deltaY);
            }
        }

        void Move(int deltaX, int deltaY)
        {
            head = new Point(head.X + deltaX, head.Y + deltaY);

            if (Math.Abs(head.X - tail.X) > 1 || Math.Abs(head.Y - tail.Y) > 1)
            {
                tail = new Point(tail.X + Step(tail.X, head.X), tail.Y + Step(tail.Y, head.Y));
                positions.Add(tail);
            }
        }

        return positions.Count;
    }

    private static int Step(int from, int to)
    {
        return to == from ? 0 : to > from ? 1 : -1;
    }
    

    public record struct Point(int X, int Y)
    {
    }
}