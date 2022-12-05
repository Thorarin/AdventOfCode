using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 20, Part = 2)]
public class Day20B : Day20Base
{
    public override Output SampleExpectedOutput => 3351;

    // Too high: 5854
    public override Output ProblemExpectedOutput => 18516;

    public override Output Run()
    {
        var image = EnhanceTimes(_image, 50);
        // TODO
        //Dump(image,  _image.Count < 100 ? SampleExpectedOutput.Value : ProblemExpectedOutput.Value);
        
        return image.Count;
    }
}