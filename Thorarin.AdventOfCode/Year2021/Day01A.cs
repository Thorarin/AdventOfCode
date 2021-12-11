using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 1, Part = 1 )]
public class Day01A : Puzzle
{
    private int[] _depths;
    
    public override void ParseInput(string[] fileLines)
    {
        _depths = fileLines.Select(Int32.Parse).ToArray();
    }

    public override Output SampleExpectedOutput => 7;

    public override Output Run()
    {
        int count = 0;
        int previousDepth = Int32.MaxValue;
        
        foreach (int depth in _depths)
        {
            if (depth > previousDepth) count++;
            previousDepth = depth;
        }

        return count;
    }
}