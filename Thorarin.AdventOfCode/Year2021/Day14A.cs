using System.Text.RegularExpressions;
using MoreLinq;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 14, Part = 1)]
public class Day14A : Puzzle
{
    private List<char> _polymer;
    private Dictionary<(char, char), char> _rules;

    public override Output SampleExpectedOutput => 1588;
    public override Output? ProblemExpectedOutput => 3247;

    public override void ParseInput(TextReader reader)
    {
        _polymer = reader.ReadLine()!.AsEnumerable().ToList();
        reader.ReadLine();

        var r = new Regex("^(?<pair>[A-Z]{2}) -> (?<insert>[A-Z])$");

        _rules = new Dictionary<(char, char), char>();
        while (reader.Peek() > 0)
        {
            var match = r.Match(reader.ReadLine()!);
            var key = match.Groups["pair"].Value;
            
            _rules.Add((key[0], key[1]), match.Groups["insert"].Value[0]);
        }
    }

    public override Output Run()
    {
        for (int i = 0; i < 10; i++)
        {
            Insert();
        }

        var grouped = _polymer.GroupBy(x => x).OrderBy(x => x.Count()).ToList();

        return grouped[^1].Count() - grouped[0].Count();
    }

    private void Insert()
    {
        for (int p = _polymer.Count - 2; p >= 0; p--)
        {
            if (_rules.TryGetValue((_polymer[p], _polymer[p + 1]), out var insert))
            {
                //Console.WriteLine($"Insert {p + 1} {_polymer.Count}");
                _polymer.Insert(p + 1, insert);
            }
        }
    }
}