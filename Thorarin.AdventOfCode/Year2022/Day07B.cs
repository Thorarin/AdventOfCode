using MoreLinq;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 7, Part = 2)]
public class Day07B : Day07Base
{
    private string[] _lines;

    public override void ParseInput(TextReader reader)
    {
        _lines = reader.ToLineArray();
    }

    public override Output SampleExpectedOutput => 24933642;

    public override Output ProblemExpectedOutput => 578710;

    public override Output Run()
    {
        var directories = TraverseDirectories(_lines);

        int spaceAvailable = 70_000_000 - directories.Single(d => d.Name == "/").Size;
        int spaceToBeFreed = 30_000_000 - spaceAvailable;

        return directories.Where(d => d.Size >= spaceToBeFreed).OrderBy(d => d.Size).First().Size;
    }
}