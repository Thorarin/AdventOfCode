using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 19, Part = 1)]
public class Day19A : Day19Base
{
    public override IOutput SampleExpectedOutput => new Answer(33, new[] { 9 , 12 });

    public override IOutput ProblemExpectedOutput => new Answer(1264, new[] {0, 6, 1, 10, 4, 6, 0, 0, 1, 3, 3, 2, 3, 5, 1, 8, 1, 8, 5, 1, 7, 0, 0, 0, 1, 4, 4, 2, 1, 2 });

    public override IOutput Run()
    {
        var maxGeodes = _blueprints
            .AsParallel()
            .AsOrdered()
            .Select(bp => (Geodes: Simulate(bp), Blueprint: bp))
            .ToList();

        int qualityLevel = maxGeodes.Select(x => x.Geodes * x.Blueprint.Number).Sum();

        return new Answer(qualityLevel, maxGeodes.Select(x => x.Geodes).ToList());
    }

    private int Simulate(Blueprint blueprint)
    {
        int overallBest = 0;
        Dictionary<StateKey, State> cache = new();

        var state = InitializeState(blueprint, 24);
        var best = Simulate(state, cache, ref overallBest);

        return best.Geode.Current;
    }
}