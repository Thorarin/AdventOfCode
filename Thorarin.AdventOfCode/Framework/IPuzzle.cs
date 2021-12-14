using JetBrains.Annotations;

namespace Thorarin.AdventOfCode.Framework;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public interface IPuzzle
{
    public void ParseInput(TextReader reader);

    public Output? SampleExpectedOutput => default;

    public Output? ProblemExpectedOutput => default;
    
    public Task<Output> Run();

    public Output? GetExpectedOutput(RunType runType)
    {
        return runType switch
        {
            RunType.Sample => SampleExpectedOutput,
            RunType.Problem => ProblemExpectedOutput,
            _ => throw new ArgumentOutOfRangeException(nameof(runType), runType, null)
        };
    }
}