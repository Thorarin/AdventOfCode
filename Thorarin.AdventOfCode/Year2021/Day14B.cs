using System.Text.RegularExpressions;
using MoreLinq;
using MoreLinq.Extensions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 14, Part = 2)]
public class Day14B : Puzzle
{
    private Dictionary<(char, char), long> _pairCount;
    private Dictionary<(char, char), char> _rules;
    private char _first;
    private char _last;

    public override Output SampleExpectedOutput => 2_188_189_693_529;
    public override Output? ProblemExpectedOutput => 4_110_568_157_153;

    public override void ParseInput(TextReader reader)
    {
        string polymer = reader.ReadLine()!;
        _first = polymer[0];
        _last = polymer[^1];

        _pairCount = new Dictionary<(char, char), long>();

        for (int i = 0; i < polymer.Length - 1; i++)
        {
            IncrementPair(_pairCount, (polymer[i], polymer[i + 1]), 1);
        }
        
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
        for (int i = 0; i < 40; i++)
        {
            Insert();
        }

        Dictionary<char, long> doubleCounts = _pairCount.Aggregate(
            new Dictionary<char, long>(),
            (acc, next) =>
            {
                acc.TryGetValue(next.Key.Item1, out long count1);
                acc[next.Key.Item1] = count1 + next.Value;

                acc.TryGetValue(next.Key.Item2, out long count2);
                acc[next.Key.Item2] = count2 + next.Value;
                
                return acc;
            });

        // Everything was just counted twice, except for the start and end of the polymer.
        // We could just use Math.Ceiling when dividing by 2 later, given our input, but I think
        // it would fail if the first and last element are the same.
        doubleCounts[_first]++;
        doubleCounts[_last]++;
        var orderedCounts = doubleCounts.OrderBy(x => x.Value).Select(x => x.Value / 2).ToList();

        return orderedCounts[^1] - orderedCounts[0];
    }

    private void Insert()
    {
        // Technically, the clone is not necessary for the problem input, but if there
        // were any element (i.e. letter) that doesn't occur in any insertion rule,
        // the algorithm would fail without it.
        var clone = _pairCount.ToDictionary(x => x.Key, x => x.Value);

        foreach (var pair in _pairCount)
        {
            if (_rules.TryGetValue(pair.Key, out char insert))
            {
                // The insertion of C into (A, B) creates new pairs (A, C) and (C, B).
                var pairA = (pair.Key.Item1, insert);
                var pairB = (insert, pair.Key.Item2);
                
                // Update the counts accordingly.
                IncrementPair(clone, pair.Key, -pair.Value);
                IncrementPair(clone, pairA, pair.Value);
                IncrementPair(clone, pairB, pair.Value);
            }
        }

        _pairCount = clone;
    }

    private void IncrementPair(Dictionary<(char, char), long> dic, (char, char) pair, long amount)
    {
        dic.TryGetValue(pair, out long current);
        dic[pair] = current + amount;
    } 
}