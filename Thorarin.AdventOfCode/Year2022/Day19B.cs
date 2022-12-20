using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 19, Part = 2)]
public class Day19B : Day19Base
{
    private const int Minutes = 32;

    public override IOutput SampleExpectedOutput => new Answer(3472, new[] { 56, 62 });

    public override IOutput ProblemExpectedOutput => new Answer(13475, new[] { 11, 49, 25 });

    [Input("day19-problem-mark.txt")]
    public IOutput ProblemMarkExpectedOutput => new Answer(25056, new[] { 16, 54, 29 });

    public override IOutput Run()
    {
        var maxGeodes = _blueprints
            .Take(3)
            .AsParallel()
            .AsOrdered()
            .Select(Simulate)
            .ToList();

        var multiplied = maxGeodes.Aggregate((a, b) => a * b);

        return new Answer(multiplied, maxGeodes);
    }

    private int Simulate(Blueprint blueprint)
    {
        int overallBest = 0;
        Dictionary<StateKey, State> cache = new();

        var state = InitializeState(blueprint, Minutes);
        var best = Simulate(state, cache, ref overallBest);

        return best.Geode.Current;
    }
}