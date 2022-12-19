namespace Thorarin.AdventOfCode.Framework;

public class TableRunner : RunnerBase
{
    private int _nameWidth = 0;
    private int _timeWidth = 8;

    public TableRunner(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override void BeforeRuns(IReadOnlyList<Type> types)
    {
        base.BeforeRuns(types);

        _nameWidth = types.Select(x => x.Name.Length).Append("Implementation".Length).Max();

        Console.WriteLine(new string('-', _timeWidth * 3 + _nameWidth + 25));
        Console.WriteLine($"| Puzzle    | {"Implementation".PadLeft(_nameWidth)} | {"Sample".PadLeft(_timeWidth)} | {"Problem".PadLeft(_timeWidth)} | {"Average".PadLeft(_timeWidth)} |");
        Console.WriteLine(new string('-', _timeWidth * 3 + _nameWidth + 25));

        RunContext.ConsoleOutput = false;
    }

    protected override void BeforeRun(Type type, PuzzleIdentifier identifier)
    {
        base.BeforeRun(type, identifier);

        Console.Write($"| {identifier.Year}-{identifier.Day:D2}-{identifier.Part} | {type.Name.PadRight(_nameWidth)} | ");
    }

    protected override async Task<RunResult> RunSample(Type type, PuzzleIdentifier identifier, string fileName, RunType runType, bool compareSampleOutput)
    {
        var result = await base.RunSample(type, identifier, fileName, runType, compareSampleOutput);

        if (CheckOutputCorrect(result))
        {
            Console.Write(result.TotalDuration.FormatHumanReadable().PadLeft(_timeWidth) + " | ");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("ERROR".PadLeft(_timeWidth) + " | ");
            Console.ResetColor();
        }

        return result;
    }

    protected override async Task<RunResult> RunProblem(Type type, PuzzleIdentifier identifier, string fileName, RunType runType)
    {
        var result = await base.RunProblem(type, identifier, fileName, runType);

        if (CheckOutputCorrect(result))
        {
            Console.Write(result.TotalDuration.FormatHumanReadable().PadLeft(_timeWidth) + " | ");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ERROR".PadLeft(_timeWidth) + " | ");
            Console.ResetColor();
        }

        return result;
    }

    protected override async Task<RunResult> RunAdditialIterations(Type type, PuzzleIdentifier identifier, string fileName, RunType runType, RunResult initialResult, int iterations)
    {
        var result = await base.RunAdditialIterations(type, identifier, fileName, runType, initialResult, iterations);

        if (CheckOutputCorrect(result))
        {
            Console.Write(result.TotalDuration.FormatHumanReadable().PadLeft(_timeWidth) + " |");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("ERROR".PadLeft(_timeWidth) + " |");
            Console.ResetColor();
        }

        return result;
    }

    protected override void AfterRun(Type type, PuzzleIdentifier identifier, RunResult sampleRunResult, RunResult problemRunResult, RunResult averageRunResult, int iterations)
    {
        if (iterations == 1)
        {
            Console.Write(averageRunResult.TotalDuration.FormatHumanReadable().PadLeft(_timeWidth) + " |");
        }

        Console.WriteLine();
    }
}