using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 5, Part = 2)]
public class Day05B : Puzzle
{
    private readonly List<Stack<char>> _stacks = new();
    private readonly List<Instruction> _instructions = new();

    public override void ParseInput(TextReader reader)
    {
        var lines = reader.ToLineArray();

        var height = lines.Count(l => l.Contains('['));

        var nrOfStacks = int.Parse(lines[height].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());

        for (var i = 0; i < nrOfStacks; i++)
        {
            _stacks.Add(new Stack<char>());
        }

        for (var i = height - 1; i >= 0; i--)
        {
            var crates = lines[i].Chunk(4).ToArray();
            //var crates = lines[i].Substring(1, lines[i].Length - 2).Split("] [");
            for (int s = 0; s < crates.Length; s++)
            {
                if (!char.IsWhiteSpace(crates[s][1]))
                {
                    _stacks[s].Push(crates[s][1]);
                }
            }
        }

        var regex = new Regex("move (?<Number>\\d+) from (?<From>\\d+) to (?<To>\\d+)");

        foreach (var instructionLine in lines.Skip(height + 2))
        {
            var match = regex.Match(instructionLine);
            _instructions.Add(new Instruction(
                int.Parse(match.Groups["Number"].Value),
                int.Parse(match.Groups["From"].Value),
                int.Parse(match.Groups["To"].Value)));
        }
    }

    public override Output SampleExpectedOutput => 0;

    public override Output Run()
    {
        foreach (var instruction in _instructions)
        {
            var temp = new Stack<char>();
            for (int i = 0; i < instruction.Number; i++)
            {
                temp.Push(_stacks[instruction.From - 1].Pop());
            }

            while (temp.Count > 0)
            {
                _stacks[instruction.To - 1].Push(temp.Pop());
            }
        }

        StringBuilder sb = new();

        foreach (var s in _stacks)
        {
            sb.Append(s.Pop());
        }

        return sb.ToString();
    }

    private record Instruction(int Number, int From, int To)
    {
    }
}