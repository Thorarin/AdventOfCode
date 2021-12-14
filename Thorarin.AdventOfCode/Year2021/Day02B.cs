using System.Diagnostics.CodeAnalysis;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 2, Part = 2)]
public class Day02B : Puzzle
{
    private List<(string command, int value)> _commands = new();

    public override Output SampleExpectedOutput => 900;
    public override Output ProblemExpectedOutput => 1_781_819_478;

    public override void ParseInput(TextReader reader)
    {
        _commands = reader
            .AsLines()
            .Select(line =>
            {
                var split = line.Split(" ");
                return (command: split[0], value: int.Parse(split[1]));
            })
            .ToList();
    }

    public override Output Run()
    {
        var position = (aim: 0, x: 0, depth: 0);

        foreach (var cmd in _commands)
        {
            switch (cmd.command)
            {
                case "forward":
                    position = position with { x = position.x + cmd.value, depth = position.depth + cmd.value * position.aim };
                    break;
                case "down":
                    position = position with { aim = position.aim + cmd.value };
                    break;
                case "up":
                    position = position with { aim = position.aim - cmd.value };
                    break;
            }
        }

        return new Result(position.depth, position.x);
    }

    [SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Local")]
    private record Result(int Depth, int X) : Output(Depth * X);
}