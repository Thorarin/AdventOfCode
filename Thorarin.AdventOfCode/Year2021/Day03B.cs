using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 3, Part = 2)]
public class Day03B : Puzzle
{
    private string[] _fileLines = Array.Empty<string>();

    public override Output SampleExpectedOutput => 230;
    public override Output ProblemExpectedOutput => 6_822_109;

    public override void ParseInput(TextReader reader)
    {
        _fileLines = reader.ToLineArray();
    }

    public override Output Run()
    {
        int oxygen = Convert.ToInt32(Filter(_fileLines, 0, false), 2);
        int carbonDioxide = Convert.ToInt32(Filter(_fileLines, 0, true), 2);

        return new Answer(oxygen, carbonDioxide);
    }
    
    private static string Filter(string[] input, int position, bool invert)
    {
        if (input.Length == 1) return input[0];
    
        int threshold = (int)Math.Ceiling(input.Length / 2m);
        int count = input.Count(line => line[position] == '1');

        char find = (count >= threshold) ^ invert ? '1' : '0';
        var filtered = input.Where(l => l[position] == find).ToArray();
        return Filter(filtered, position + 1, invert);
    }    

    private record Answer(long Oxygen, long CarbonDioxide) : Output(Oxygen * CarbonDioxide);
}