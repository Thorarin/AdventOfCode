using System.Text.RegularExpressions;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Year2021.Day22;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 22, Part = 1)]
public class Day22A_WithCuboids : Day22Base
{
    // The sample file used is from part 2 (so I don't have to switch files)
    public override Output SampleExpectedOutput => 474140;
    
    public override Output ProblemExpectedOutput => 576028;

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

        var regionOfInterest = new Cuboid(-50, 50, -50, 50, -50, 50);
        
        return reactor.GetCubesOnInCuboid(regionOfInterest);        
    }
}