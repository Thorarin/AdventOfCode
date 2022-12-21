using System.Diagnostics;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Year2022.Day21;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 21, Part = 2)]
public class Day21B : Day21Base
{
    public override Output SampleExpectedOutput => 301;

    public override Output ProblemExpectedOutput => 3_509_819_803_065;

    public override Output Run()
    {
        // Erase the number from the input file
        var human = GetMonkey("humn");
        human.Number = default;

        // Get to work
        var root = GetMonkey("root");
        var left = root.Left!.GetValue();
        var right = root.Right!.GetValue();

        if (!left.HasValue && !right.HasValue)
        {
            RunContext.Out.WriteLine("Cannot solve this. Only one operand is expected to be unknown.");
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
        CheckResult(root, human);

        return human.Number!.Value;
    }

    [Conditional("DEBUG")]
    private void CheckResult(Monkey root, Monkey human)
    {
        if (root.Left!.GetValue() != root.Right!.GetValue())
        {
            throw new Exception($"Left hand {root.Left.GetValue()} is not equal to right hand {root.Right.GetValue()}");
        }

        if (!human.Number.HasValue)
        {
            throw new Exception("Expected a number for human.");
        }
    }
}