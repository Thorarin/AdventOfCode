using JetBrains.Annotations;

namespace Thorarin.AdventOfCode.Framework;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public interface IPuzzle
{
    public void ParseInput(string[] fileLines);

    public Output? SampleExpectedOutput => default;

    public Output? ProblemExpectedOutput => default;
    
    public Task<Output> Run();
}