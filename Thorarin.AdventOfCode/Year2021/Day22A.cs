using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 22, Part = 1)]
public class Day22A : Day22Base
{
    // The sample file used is from part 2 (so I don't have to switch files)
    public override Output SampleExpectedOutput => 474140;

    public override Output ProblemExpectedOutput => 576028;

    public override Output Run()
    {
        bool[,,] reactor = new bool[101, 101, 101];

        foreach (var instruction in Instructions)
        {
            for (int x = Math.Max(instruction.Cuboid.X1, -50); x <= Math.Min(instruction.Cuboid.X2, 50); x++)
            {
                for (int y = Math.Max(instruction.Cuboid.Y1, -50); y <= Math.Min(instruction.Cuboid.Y2, 50); y++)
                {
                    for (int z = Math.Max(instruction.Cuboid.Z1, -50); z <= Math.Min(instruction.Cuboid.Z2, 50); z++)
                    {
                        reactor[x + 50, y + 50, z + 50] = instruction.State;
                    }
                }
            }
        }

        int count = 0;
        for (int x = 0; x <= 100; x++)
        {
            for (int y = 0; y <= 100; y++)
            {
                for (int z = 0; z <= 100; z++)
                {
                    if (reactor[x, y, z]) count++;
                }
            }
        }
        
        return count;
    }
}