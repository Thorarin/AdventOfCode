namespace Thorarin.AdventOfCode.Framework;

public abstract class Puzzle : IPuzzle
{
    public abstract void ParseInput(TextReader reader);

    public virtual IOutput? SampleExpectedOutput => default;

    public virtual IOutput? ProblemExpectedOutput => default;

    public abstract IOutput Run();
    
    Task<IOutput> IPuzzle.Run()
    {
        return Task.FromResult(Run());
    }
}