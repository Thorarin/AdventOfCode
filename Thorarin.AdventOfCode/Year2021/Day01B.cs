using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 1, Part = 2)]
public abstract class Day01B : Puzzle
{
    protected List<int> Input { get; private set; }
    
    public sealed override void ParseInput(TextReader reader)
    {
        Input = reader.AsLines().Select(int.Parse).ToList();        
    }
    
    public override Output SampleExpectedOutput => 5;

    public override Output ProblemExpectedOutput => 1653;
}