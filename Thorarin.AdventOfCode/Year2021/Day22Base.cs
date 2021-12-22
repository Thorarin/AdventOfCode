using System.Text.RegularExpressions;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Year2021.Day22;

namespace Thorarin.AdventOfCode.Year2021;

public abstract class Day22Base : Puzzle
{
    protected IReadOnlyList<Instruction> Instructions { get; private set; }

    public override void ParseInput(TextReader reader)
    {
        var regex = new Regex("^(?<state>on|off) x=(?<x1>-?\\d+)\\.\\.(?<x2>-?\\d+),y=(?<y1>-?\\d+)\\.\\.(?<y2>-?\\d+),z=(?<z1>-?\\d+)\\.\\.(?<z2>-?\\d+)");

        var instructions = new List<Instruction>();

        foreach (var line in reader.AsLines())
        {
            var match = regex.Match(line);
            
            instructions.Add(new Instruction(
                match.Groups["state"].Value == "on",
                new Cuboid(
                    int.Parse(match.Groups["x1"].Value),
                    int.Parse(match.Groups["x2"].Value),
                    int.Parse(match.Groups["y1"].Value),
                    int.Parse(match.Groups["y2"].Value),
                    int.Parse(match.Groups["z1"].Value),
                    int.Parse(match.Groups["z2"].Value))
            ));
        }

        Instructions = instructions;
    }
}