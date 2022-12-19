namespace Thorarin.AdventOfCode.Framework;

public class ConsoleRunner : RunnerBase
{
    public ConsoleRunner(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override void BeforeRuns(IReadOnlyList<Type> types)
    {
        Console.WriteLine($"Found {types.Count} puzzles: {string.Join(", ", types.Select(x => x.Name))}");
        Console.WriteLine();
    }

    protected override void BeforeRun(Type type, PuzzleIdentifier identifier)
    {
        base.BeforeRun(type, identifier);
        Console.WriteLine($"Running {identifier.Year}-{identifier.Day}-{identifier.Part}, implementation: {type.Name}");
    }

    protected override async Task<RunResult> RunWarmup(Type type, PuzzleIdentifier identifier, string fileName, RunType runType)
    {
        Console.Write("Doing warmup run... ");
        var result = await base.RunWarmup(type, identifier, fileName, runType);
        Console.WriteLine("DONE");
        return result;
    }

    protected override async Task<RunResult> RunSample(Type type, PuzzleIdentifier identifier, string fileName, RunType runType, bool compareSampleOutput)
    {
        Console.Write("Running using sample data...  ");

        if (!compareSampleOutput)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARNING: sample input not found. Using problem data.");
            Console.ResetColor();
        }

        var result = await base.RunSample(type, identifier, fileName, runType, compareSampleOutput);

        Console.WriteLine($"OK ({result.TotalDuration.FormatHumanReadable()})");

        if (compareSampleOutput)
        {
            WarnOnOutputIncorrect(result.Expected, result.Output);
        }

        return result;
    }

    protected override async Task<RunResult> RunProblem(Type type, PuzzleIdentifier identifier, string fileName, RunType runType)
    {
        Console.Write("Running using problem data... ");
        var result = await base.RunProblem(type, identifier, fileName, runType);
        Console.WriteLine($"OK ({result.TotalDuration.FormatHumanReadable()})");
        Console.WriteLine($"Outcome: {result.Output}");
        WarnOnOutputIncorrect(result.Expected, result.Output);

        return result;
    }

    protected override async Task<RunResult> RunExtra(Type type, PuzzleIdentifier identifier, string fileName, string filePath, IOutput? expected)
    {
        Console.Write($"Running using extra data ({fileName})... ");

        var runResult = await Run(type, filePath, expected);

        Console.WriteLine($"OK ({runResult.TotalDuration.FormatHumanReadable()})");
        WarnOnOutputIncorrect(runResult.Expected, runResult.Output);

        if (runResult.Expected == null)
        {
            Console.WriteLine($"Extra: {runResult.Output}");
        }

        return runResult;
    }

    protected override Task<RunResult> RunAdditialIterations(Type type, PuzzleIdentifier identifier, string fileName, RunType runType, RunResult initialResult, int iterations)
    {
        int remaining = iterations - 1;
        Console.WriteLine($"Running {remaining} additional iterations (expected to take {(initialResult.TotalDuration * remaining).FormatHumanReadable()})...");
        return base.RunAdditialIterations(type, identifier, fileName, runType, initialResult, iterations);
    }

    protected override void AfterRun(Type type, PuzzleIdentifier identifier, RunResult sampleRunResult, RunResult problemRunResult, RunResult averageRunResult, int iterations)
    {
        var format = averageRunResult.TotalDuration.GetHumanReadableFormat();

        string totalString = averageRunResult.TotalDuration.FormatHumanReadable(format);
        string parseString = averageRunResult.ParseDuration.FormatHumanReadable(format).PadLeft(totalString.Length);
        string runString = averageRunResult.RunDuration.FormatHumanReadable(format).PadLeft(totalString.Length);

        Console.WriteLine(iterations > 1 ? $"Average time taken over {iterations} iterations:" : "Time taken:");
        Console.WriteLine($"  Parse:   {parseString}");
        Console.WriteLine($"  Run:     {runString}");
        Console.WriteLine($"  Total:   {totalString}");
        Console.WriteLine();
    }

    private void WarnOnOutputIncorrect(IOutput? expected, IOutput actual)
    {
        if (expected != null && !Equals(expected, actual))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARNING: output does not match the expected value!");
            Console.WriteLine($"Expected: {expected}");
            Console.WriteLine($"Actual:   {actual}");
            Console.ResetColor();
        }
    }
}