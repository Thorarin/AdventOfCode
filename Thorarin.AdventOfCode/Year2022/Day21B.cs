using System.Diagnostics;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 21, Part = 2)]
public class Day21B : Puzzle
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

    public override Output SampleExpectedOutput => 301;

    public override Output ProblemExpectedOutput => 3509819803065;

    public override Output Run()
    {
        var human = GetMonkey("humn");
        human.Number = default;

        var root = GetMonkey("root");

        var left = root.Left!.GetValue();
        var right = root.Right!.GetValue();

        if (!left.HasValue && !right.HasValue)
        {
            RunContext.Out.WriteLine($"Cannot solve this.");
            return -1;
        }
        else if (left.HasValue)
        {
            root.Right.SetValue(left.Value);
        }
        else if (right.HasValue)
        {
            root.Left.SetValue(right.Value);
        }

        // Double check result
        if (root.Left.GetValue() != root.Right.GetValue())
        {
            RunContext.Out.WriteLine($"Left hand {root.Left.GetValue()} is not equal to right hand {root.Right.GetValue()}");
            return -1;
        }

        if (!human.Number.HasValue)
        {
            RunContext.Out.WriteLine("Expected a number for human.");
            return -1;
        }

        return human.Number.Value;
    }

    private class Monkey
    {
        public long? Number { get; set; }

        public Monkey? Left { get; set; }
        public Monkey? Right { get; set; }
        public char? Operator { get; set; }

        public long? GetValue()
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
                    return default;
            }
        }

        public void SetValue(long value)
        {
            if (!Operator.HasValue)
            {
                Number = value;
                return;
            }

            var left = Left!.GetValue();
            var right = Right!.GetValue();

            if (left.HasValue && right.HasValue)
            {
                throw new Exception("Cannot set value");
            }
            else if (left.HasValue)
            {
                switch (Operator)
                {
                    case '+':
                        Right.SetValue(value - left.Value);
                        break;
                    case '-':
                        Right.SetValue(left.Value - value);
                        break;
                    case '*':
                        Right.SetValue(value / left.Value);
                        break;
                    case '/':
                        Right.SetValue(left.Value / value);
                        break;
                }
            }
            else if (right.HasValue)
            {
                switch (Operator)
                {
                    case '+':
                        Left.SetValue(value - right.Value);
                        break;
                    case '-':
                        Left.SetValue(right.Value + value);
                        break;
                    case '*':
                        Left.SetValue(value / right.Value);
                        break;
                    case '/':
                        Left.SetValue(value * right.Value);
                        break;
                }
            }

            CheckSetValue(value);
        }

        [Conditional("DEBUG")]
        private void CheckSetValue(long value)
        {
            if (GetValue() == value) return;
            var left = Left!.GetValue();
            var right = Right!.GetValue();
            Debug.WriteLine($"{left} {Operator} {right} ≠ {value}");
        }
    }
}