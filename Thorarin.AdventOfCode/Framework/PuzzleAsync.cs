namespace Thorarin.AdventOfCode.Framework;

public abstract class PuzzleAsync : IPuzzle
{
    public abstract void ParseInput(TextReader reader);

    public virtual Output? SampleExpectedOutput => default;

    public virtual Output? ProblemExpectedOutput => default;

    public abstract Task<Output> Run();
}