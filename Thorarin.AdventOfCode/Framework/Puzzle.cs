using JetBrains.Annotations;

namespace Thorarin.AdventOfCode.Framework;

public abstract class Puzzle : IPuzzle
{
    public abstract void ParseInput(string[] fileLines);

    public virtual Output? SampleExpectedOutput => default;

    public virtual Output? ProblemExpectedOutput => default;

    public abstract Output Run();
    
    Task<Output> IPuzzle.Run()
    {
        return Task.FromResult(Run());
    }
}