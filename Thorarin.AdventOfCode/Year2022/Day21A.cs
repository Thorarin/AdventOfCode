using System.Diagnostics;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 21, Part = 1)]
public class Day21A : Puzzle
{
    private readonly Dictionary<string, Monkey> _monkeys = new();

    public override void ParseInput(TextReader reader)
    {
        foreach (var line in reader.AsLines())
        {
            string name = line.Substring(0, 4);
            var monkey = GetMonkey(name);

            string rest = line.Substring(6);
            if (rest.Contains(' '))
            {
                var split = rest.Split(' ');
                monkey.Left = GetMonkey(split[0]);
                monkey.Right = GetMonkey(split[2]);
                monkey.Operator = split[1][0];
            }
            else
            {
                monkey.Number = int.Parse(rest);
            }
        }
    }

    private Monkey GetMonkey(string name)
    {
        if (_monkeys.TryGetValue(name, out var monkey)) return monkey;

        monkey = new Monkey();
        _monkeys.Add(name, monkey);
        return monkey;
    }

    public override Output SampleExpectedOutput => 152;

    public override Output ProblemExpectedOutput => 159591692827554;

    public override Output Run()
    {
        return GetMonkey("root").GetValue();
    }

    private class Monkey
    {
        public long? Number { get; set; }

        public Monkey? Left { get; set; }
        public Monkey? Right { get; set; }
        public char? Operator { get; set; }

        public long GetValue()
        {
            if (Number.HasValue) return Number.Value;

            switch (Operator)
            {
                case '+':
                    return Left!.GetValue() + Right!.GetValue();
                case '-':
                    return Left!.GetValue() - Right!.GetValue();
                case '*':
                    return Left!.GetValue() * Right!.GetValue();
                case '/':
                    return Left!.GetValue() / Right!.GetValue();
                default:
                    throw new Exception();
            }
        }
    }
}