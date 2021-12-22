using System.Text.RegularExpressions;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Year2021.Day22;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 22, Part = 2)]
public class Day22B : Day22Base
{
    public override Output SampleExpectedOutput => 2_758_514_936_282_235;

    public override Output? ProblemExpectedOutput => 1_387_966_280_636_636;

    public override Output Run()
    {
        var reactor = new Reactor();
        
        foreach (var instruction in Instructions)
        {
            switch (instruction.State)
            {
                case true:
                    reactor.TurnOn(instruction.Cuboid);
                    break;
                case false:
                    reactor.TurnOff(instruction.Cuboid);
                    break;
            }
        }
        
        return reactor.CubesOn;
    }
}