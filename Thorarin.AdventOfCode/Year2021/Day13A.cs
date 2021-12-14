using System.Text.RegularExpressions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 13, Part = 1)]
public class Day13A : Puzzle
{
    private List<Dot> _dots;
    private Func<Dot, Dot> _fold;

    public override Output SampleExpectedOutput => 17;

    public override Output ProblemExpectedOutput => 719;

    public override void ParseInput(TextReader reader)
    {
        _dots = new List<Dot>();
        _fold = x => x;

        while (true)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) break;
            
            var split = line.Split(',');
            _dots.Add(new Dot(int.Parse(split[0]), int.Parse(split[1])));
        }

        reader.ReadLine();

        var regex = new Regex("fold along (?<fold>[xy])=(?<value>\\d+)");
        while (true)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrEmpty(line)) break;

            var match = regex.Match(line);
            int value = int.Parse(match.Groups["value"].Value);

            var fold = _fold;
            switch (match.Groups["fold"].Value)
            {
                case "x":
                    _fold = dot => FoldX(fold(dot), value);
                    break;
                case "y":
                    _fold = dot => FoldY(fold(dot), value);
                    break;
            }

            // Abort reading fold instructions for part 1
            break;            
        }
    }

    private static Dot FoldX(Dot dot, int x)
    {
        if (dot.X < x) return dot;
        return dot with { X = -dot.X + x + x };
    }
    

    private static Dot FoldY(Dot dot, int y)
    {
        if (dot.Y < y) return dot;
        return dot with { Y = -dot.Y + y + y };
    }    

    public override Output Run()
    {
        var foldedDots = _dots.Select(_fold).ToList();
            
        return foldedDots.Distinct().Count();
    }

    record Dot(int X, int Y);
}