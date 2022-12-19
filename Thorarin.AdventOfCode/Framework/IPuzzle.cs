using JetBrains.Annotations;

namespace Thorarin.AdventOfCode.Framework;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public interface IPuzzle
{
    public void ParseInput(TextReader reader);

    public IOutput? SampleExpectedOutput => default;

    public IOutput? ProblemExpectedOutput => default;
    
    public Task<IOutput> Run();

    public IOutput? GetExpectedOutput(RunType runType)
    {
        return runType switch
        {
            RunType.Sample => SampleExpectedOutput,
            RunType.Problem => ProblemExpectedOutput,
            _ => throw new ArgumentOutOfRangeException(nameof(runType), runType, null)
        };
    }
}