using MoreLinq;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 6, Part = 1)]
public class Day06A : Puzzle
{
    private string _data;

    public override void ParseInput(TextReader reader)
    {
        _data = reader.ReadToEnd();
    }

    public override Output SampleExpectedOutput => 7;

    public override Output? ProblemExpectedOutput => 1896;

    public override Output Run()
    {
        const int size = 4;

        var position = _data
            .Window(size)
            .Select((w, index) => (match: w.AsEnumerable().Distinct().Count() == size, index))
            .First(x => x.match);

        return position.index + size;
    }

}