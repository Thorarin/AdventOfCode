using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 2, Part = 1)]
public class Day02A : Puzzle
{
    private List<(string command, int value)> _commands = new();

    public override Output SampleExpectedOutput => 150;
    public override Output ProblemExpectedOutput => 1635930;

    public override void ParseInput(string[] fileLines)
    {
        _commands = fileLines
            .Select(line =>
            {
                var split = line.Split(" ");
                return (command: split[0], value: int.Parse(split[1]));
            })
            .ToList();
    }

    public override Output Run()
    {
        var position = (x: 0, depth: 0);

        foreach (var cmd in _commands)
        {
            switch (cmd.command)
            {
                case "forward":
                    position = position with { x = position.x + cmd.value };
                    break;
                case "down":
                    position = position with { depth = position.depth + cmd.value };
                    break;
                case "up":
                    position = position with { depth = position.depth - cmd.value };
                    break;
            }
        }

        return new Result(position.depth, position.x);
    }

    private record Result(int Depth, int X) : Output(Depth * X);
}
