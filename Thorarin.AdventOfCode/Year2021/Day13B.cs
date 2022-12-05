using System.Text;
using System.Text.RegularExpressions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 13, Part = 2)]
public class Day13B : Puzzle
{
    private List<Dot> _dots;
    private Func<Dot, Dot> _fold;

    public override Output SampleExpectedOutput => 16;
    public override Output ProblemExpectedOutput => 97;

    public override void ParseInput(TextReader reader)
    {
        _dots = new List<Dot>();
        _fold = x => x;

        foreach (string line in reader.UntilNextEmptyLine())
        {
            var split = line.Split(',');
            _dots.Add(new Dot(int.Parse(split[0]), int.Parse(split[1])));
        }

        var regex = new Regex("fold along (?<fold>[xy])=(?<value>\\d+)");
        
        foreach (string line in reader.UntilNextEmptyLine())
        {
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
        var foldedDots = _dots.Select(_fold).Distinct().ToList();
        var width = foldedDots.Select(x => x.X).Max() + 1;
        var height = foldedDots.Select(x => x.Y).Max() + 1;

        var bitmap = new bool[width, height];
        foreach (var dot in foldedDots)
        {
            bitmap[dot.X, dot.Y] = true;
        }

        return new Answer(foldedDots.Count, MakeAsciiArt(bitmap));
    }

    private static string MakeAsciiArt(bool[,] bitmap)
    {
        int width = bitmap.GetLength(0);
        int height = bitmap.GetLength(1);
        
        StringBuilder sb = new();
        sb.AppendLine();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                sb.Append(bitmap[x, y] ? '█' : ' ');
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    record Dot(int X, int Y);

    record Answer(long Value, string Image) : LongOutput(Value);
}