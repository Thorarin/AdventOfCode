using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022.Day21;

[Puzzle(Year = 2022, Day = 21, Part = 1)]
public abstract class Day21Base : Puzzle
{
    private readonly Dictionary<string, Monkey> _monkeys = new();

    public override void ParseInput(TextReader reader)
    {
        foreach (var line in reader.AsLines())
        {
            string name = line.Substring(0, 4);
            var monkey = GetMonkey(name);

            var rest = line.AsSpan().Slice(6);
            if (rest.Contains(' '))
            {
                monkey.Left = GetMonkey(rest.Slice(0, 4).ToString());
                monkey.Right = GetMonkey(rest.Slice(7).ToString());
                monkey.Operator = rest[5];
            }
            else
            {
                monkey.Number = int.Parse(rest);
            }
        }
    }
    protected Monkey GetMonkey(string name)
    {
        if (_monkeys.TryGetValue(name, out var monkey)) return monkey;

        monkey = new Monkey();
        _monkeys.Add(name, monkey);
        return monkey;
    }
}