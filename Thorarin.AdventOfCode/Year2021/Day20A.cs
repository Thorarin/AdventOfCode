using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 20, Part = 1)]
public class Day20A : Day20Base
{
    public override Output SampleExpectedOutput => 35;

    // Too high: 5854
    public override Output ProblemExpectedOutput => 5819;

    public override Output Run()
    {
        var image = EnhanceTimes(_image, 2);
        // TODO
        //Dump(image, _image.Count < 100 ? SampleExpectedOutput.Value : ProblemExpectedOutput.Value);
        
        return image.Count;
    }
}