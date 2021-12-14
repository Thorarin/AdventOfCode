using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 3, Part = 1)]
public class Day03A : Puzzle
{
    private string[] _fileLines = Array.Empty<string>();

    public override Output SampleExpectedOutput => 198;
    public override Output ProblemExpectedOutput => 2_640_986;

    public override void ParseInput(TextReader reader)
    {
        _fileLines = reader.ToLineArray();
    }

    public override Output Run()
    {
        int binaryLength = _fileLines[0].Length;
        
        var counts = new int[binaryLength];

        foreach (string line in _fileLines)
        {
            for (int c = 0; c < binaryLength; c++)
            {
                if (line[c] == '1')
                {
                    counts[c]++;
                }
            }
        }

        int threshold = _fileLines.Length / 2;
        string gamma = "";
        string epsilon = "";

        for (int c = 0; c < binaryLength; c++)
        {
            gamma += counts[c] >= threshold ? "1" : "0";
            epsilon += counts[c] >= threshold ? "0" : "1";
        }

        return new Answer(Convert.ToInt64(gamma, 2), Convert.ToInt64(epsilon, 2));
    }

    private record Answer(long Gamma, long Epsilon) : Output(Gamma * Epsilon);
}