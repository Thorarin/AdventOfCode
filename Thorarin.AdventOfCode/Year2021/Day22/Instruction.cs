namespace Thorarin.AdventOfCode.Year2021.Day22;

public record Instruction(bool State, Cuboid Cuboid)
{
    public override string ToString()
    {
        return $"{(State ? "on" : "off")} {Cuboid}";
    }
}