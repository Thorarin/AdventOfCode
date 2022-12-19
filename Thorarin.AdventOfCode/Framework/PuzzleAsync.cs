namespace Thorarin.AdventOfCode.Framework;

public abstract class PuzzleAsync : IPuzzle
{
    public abstract void ParseInput(TextReader reader);

    public virtual IOutput? SampleExpectedOutput => default;

    public virtual IOutput? ProblemExpectedOutput => default;

    public abstract Task<IOutput> Run();
}