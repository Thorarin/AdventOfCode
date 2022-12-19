using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Thorarin.AdventOfCode.Framework;

public abstract class RunnerBase
{
    private readonly IServiceProvider _serviceProvider;

    protected RunnerBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task RunImplementation(Type type, int iterations, bool warmup, bool runExtraInputs)
    {
        try
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            return BaseRunner(type, iterations, warmup, runExtraInputs);
        }
        finally
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
        }
    }

    public virtual void BeforeRuns(IReadOnlyList<Type> types)
    {
    }

    protected virtual void BeforeRun(Type type, PuzzleIdentifier identifier)
    {
    }

    protected virtual async Task<RunResult> RunWarmup(Type type, PuzzleIdentifier identifier, string fileName, RunType runType)
    {
        return await Run(type, fileName, runType);
    }

    protected virtual async Task<RunResult> RunSample(Type type, PuzzleIdentifier identifier, string fileName, RunType runType, bool compareSampleOutput)
    {
        return await Run(type, fileName, RunType.Sample);
    }

    protected virtual async Task<RunResult> RunProblem(Type type, PuzzleIdentifier identifier, string fileName, RunType runType)
    {
        return await Run(type, fileName, RunType.Problem);
    }

    protected virtual async Task<RunResult> RunAdditialIterations(Type type, PuzzleIdentifier identifier, string fileName, RunType runType, RunResult initialResult, int iterations)
    {
        TimeSpan sumParse = initialResult.ParseDuration;
        TimeSpan sumRun = initialResult.RunDuration;

        for (int iteration = 1; iteration < iterations; iteration++)
        {
            var runResult = await Run(type, fileName, RunType.Problem);
            sumParse += runResult.ParseDuration;
            sumRun += runResult.RunDuration;

            //if (firstRunResult != null && !Equals(firstRunResult.Output, runResult.Output))
            //{
            //    Console.ForegroundColor = ConsoleColor.Yellow;
            //    Console.WriteLine($"WARNING: output for iteration {iteration} does not match iteration 0!");
            //    Console.ResetColor();
            //}
        }

        return initialResult with { ParseDuration = sumParse / iterations, RunDuration = sumRun / iterations };
    }

    protected virtual async Task<RunResult> RunExtra(Type type, PuzzleIdentifier identifier, string fileName, string filePath, IOutput? expected)
    {
        return await Run(type, filePath, expected);
    }

    protected virtual void AfterRun(Type type, PuzzleIdentifier identifier, RunResult sampleRunResult, RunResult problemRunResult, RunResult averageRunResult, int iterations)
    {
    }

    private async Task BaseRunner(Type type, int iterations, bool warmup, bool runExtraInputs)
    {
        var attr = type.GetCustomAttribute<PuzzleAttribute>()!;
        var puzzleId = new PuzzleIdentifier(attr);
        BeforeRun(type, puzzleId);

        string path = $"Year{attr.Year}\\Inputs";
        string sampleFileName = Path.Combine(path, $"day{attr.Day:00}-sample.txt");
        string problemFileName = Path.Combine(path, $"day{attr.Day:00}-problem.txt");

        if (warmup)
        {
            await Run(type, sampleFileName, RunType.Sample);
        }

        bool compareSampleOutput = true;

        if (!File.Exists(sampleFileName))
        {
            sampleFileName = problemFileName;
            compareSampleOutput = false;
        }

        var sampleRunResult = await RunSample(type, puzzleId, sampleFileName, RunType.Sample, compareSampleOutput);
        var problemRunResult = await RunProblem(type, puzzleId, problemFileName, RunType.Problem);
        var averageRunResult = problemRunResult;

        if (iterations > 1)
        {
            averageRunResult = await RunAdditialIterations(type, puzzleId, problemFileName, RunType.Problem, problemRunResult, iterations);
        }

        if (runExtraInputs)
        {
            foreach (var extras in DiscoverExtraInputs(type))
            {
                string filePath = Path.Combine(path, extras.FileName);

                if (!File.Exists(filePath))
                {
                    RunExtraMissingFile(extras.FileName);
                    continue;
                }

                await RunExtra(type, puzzleId, extras.FileName, filePath, extras.Expected);
            }
        }

        AfterRun(type, puzzleId, sampleRunResult, problemRunResult, averageRunResult, iterations);
    }


    protected virtual void RunExtraMissingFile(string extrasFileName)
    {
    }

    protected List<(string FileName, IOutput? Expected)> DiscoverExtraInputs(Type type)
    {
        var instance = ActivatorUtilities.CreateInstance(_serviceProvider, type);
            
        var extraInputProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(pi => typeof(IOutput).IsAssignableFrom(pi.PropertyType))
            .Select(pi => (pi, input: pi.GetCustomAttribute<InputAttribute>()))
            .Where(x => x.input != null)
            .ToList();

        return extraInputProperties.Select(x => (x.input!.FileName, (IOutput?)x.pi.GetValue(instance))).ToList();
    }

    protected async Task<RunResult> Run(Type type, string fileName, RunType runType)
    {
        GC.Collect(2);
        RunContext.Reset();

        RunResult result;

        if (typeof(Puzzle).IsAssignableFrom(type))
        {
            var puzzleInstance = (Puzzle)ActivatorUtilities.CreateInstance(_serviceProvider, type);
            result = await Run(puzzleInstance, fileName, GetExpectedOutput(puzzleInstance));
        }
        else
        {
            var puzzleInstance = (IPuzzle)ActivatorUtilities.CreateInstance(_serviceProvider, type);
            result = await Run(puzzleInstance, fileName, GetExpectedOutput(puzzleInstance));
        }

        RunContext.Reset();

        return result;

        IOutput? GetExpectedOutput(IPuzzle puzzle)
        {
            return puzzle.GetExpectedOutput(runType);
        }
    }

    protected async Task<RunResult> Run(Type type, string fileName, IOutput? expected)
    {
        GC.Collect(2);
        RunContext.Reset();

        RunResult runResult;

        if (typeof(Puzzle).IsAssignableFrom(type))
        {
            var puzzleInstance = (Puzzle)ActivatorUtilities.CreateInstance(_serviceProvider, type);
            runResult = await Run(puzzleInstance, fileName, expected);
        }
        else
        {
            var puzzleInstance = (IPuzzle)ActivatorUtilities.CreateInstance(_serviceProvider, type);
            runResult = await Run(puzzleInstance, fileName, expected);
        }
        
        RunContext.Reset();

        return runResult;
    }

    private async Task<RunResult> Run(IPuzzle puzzle, string fileName, IOutput? expected)
    {
        Stopwatch parseStopwatch;
        string fileContent = await File.ReadAllTextAsync(fileName);
        using (var reader = new StringReader(fileContent))
        {
            parseStopwatch = Stopwatch.StartNew();
            puzzle.ParseInput(reader);
            parseStopwatch.Stop();
        }
        
        var runStopwatch = Stopwatch.StartNew();
        var output = await puzzle.Run();
        runStopwatch.Stop();

        return new RunResult(output, parseStopwatch.Elapsed, runStopwatch.Elapsed, expected);                
    }

    private async Task<RunResult> Run(Puzzle puzzle, string fileName, Output? expected)
    {
        Stopwatch parseStopwatch;
        string fileContent = await File.ReadAllTextAsync(fileName);
        using (var reader = new StringReader(fileContent))
        {
            parseStopwatch = Stopwatch.StartNew();
            puzzle.ParseInput(reader);
            parseStopwatch.Stop();
        }

        var runStopwatch = Stopwatch.StartNew();
        var output = puzzle.Run();
        runStopwatch.Stop();

        return new RunResult(output, parseStopwatch.Elapsed, runStopwatch.Elapsed, expected);
    }

    /// <summary>
    /// Smart comparison to handle the case of custom Output records.
    /// If the type of <paramref name="expected"/> and <paramref name="actual"/> match,
    /// use the default equality operator for records, which compares all properties.
    /// If the types are not equal, compare only the final <see cref="Output.Value"/>
    /// that contains the puzzle answer.
    /// </summary>
    private bool Equals(Output expected, Output actual)
    {
        if (expected.GetType() == actual.GetType())
        {
            return expected == actual;
        }
        // TODO
        return false;
        //return expected.Value == actual.Value;
    }

    protected bool CheckOutputCorrect(RunResult runResult)
    {
        return runResult.Expected == null || Equals(runResult.Expected, runResult.Output);
    }
}