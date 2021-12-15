using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Thorarin.AdventOfCode.Framework;

public class Runner
{
    private readonly IServiceProvider _serviceProvider;

    public Runner(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public Task RunImplementation(Type type, int iterations)
    {
        try
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            return ConsoleRunner(type, iterations);
        }
        finally
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
        }
    }

    private async Task ConsoleRunner(Type type, int iterations)
    {
        var attr = type.GetCustomAttribute<PuzzleAttribute>()!;
        Console.WriteLine($"Running {attr.Year}-{attr.Day}-{attr.Part}, implementation: {type.Name}");
        
        string sampleFileName = $"Year{attr.Year}\\Inputs\\day{attr.Day:00}-sample.txt";
        string problemFileName = $"Year{attr.Year}\\Inputs\\day{attr.Day:00}-problem.txt";
        
        Console.Write("Running using sample data...  ");

        bool compareSampleOutput = true;
        
        if (!File.Exists(sampleFileName))
        {
            sampleFileName = problemFileName;
            compareSampleOutput = false;
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARNING: sample input not found. Using problem data.");
            Console.ResetColor();
        }
        
        var sampleRunResult = await Run(type, sampleFileName, RunType.Sample);
        Console.WriteLine($"OK ({sampleRunResult.TotalDuration.FormatHumanReadable()})");

        if (compareSampleOutput)
        {
            WarnOnOutputIncorrect(sampleRunResult.Expected, sampleRunResult.Output);
        }

        Console.Write("Running using problem data... ");
        
        TimeSpan sumParse = TimeSpan.Zero;
        TimeSpan sumRun = TimeSpan.Zero;
        RunResult firstRunResult = null;

        if (iterations < 1) throw new Exception();
        
        for (int iteration = 0; iteration < iterations; iteration++)
        {
            var runResult = await Run(type, problemFileName, RunType.Problem);
            sumParse += runResult.ParseDuration;
            sumRun += runResult.RunDuration;

            if (iteration == 0)
            {
                Console.WriteLine($"OK ({runResult.TotalDuration.FormatHumanReadable()})");
                Console.WriteLine($"Outcome: {runResult.Output}");
                WarnOnOutputIncorrect(runResult.Expected, runResult.Output);
                
                if (iterations > 1)
                {
                    firstRunResult = runResult;
                    int remaining = iterations - 1;
                    Console.WriteLine($"Running {remaining} additional iterations (expected to take {(runResult.TotalDuration * remaining).FormatHumanReadable()})...");
                }
            }
            else if (firstRunResult != null && !Equals(firstRunResult.Output, runResult.Output))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"WARNING: output for iteration {iteration} does not match iteration 0!");
                Console.ResetColor();
            }
        }

        var averageParse = sumParse / iterations;
        var averageRun = sumRun / iterations;
        var total = averageParse + averageRun;
        var format = total.GetHumanReadableFormat();

        string totalString = total.FormatHumanReadable(format);
        string parseString = averageParse.FormatHumanReadable(format).PadLeft(totalString.Length);
        string runString = averageRun.FormatHumanReadable(format).PadLeft(totalString.Length);

        Console.WriteLine(iterations > 1 ? "Average time taken:" : "Time taken:");
        Console.WriteLine($"  Parse:   {parseString}");
        Console.WriteLine($"  Run:     {runString}");
        Console.WriteLine($"  Total:   {totalString}");
    }    
  
    private Task<RunResult> Run(Type type, string fileName, RunType runType)
    {
        if (typeof(Puzzle).IsAssignableFrom(type))
        {
            var puzzleInstance = (Puzzle)ActivatorUtilities.CreateInstance(_serviceProvider, type);
            //var puzzleInstance = (Puzzle)Activator.CreateInstance(type)!;
            return Run(puzzleInstance, fileName, runType);
        }
        
        var instance = (IPuzzle)ActivatorUtilities.CreateInstance(_serviceProvider, type);
        //
        // var instance = (IPuzzle)Activator.CreateInstance(type)!;
        return Run(instance, fileName, runType);        
    }

    private async Task<RunResult> Run(IPuzzle puzzle, string fileName, RunType runType)
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

        var expected = puzzle.GetExpectedOutput(runType);

        return new RunResult(output, parseStopwatch.Elapsed, runStopwatch.Elapsed, expected);                
    }
   
    private async Task<RunResult> Run(Puzzle puzzle, string fileName, RunType runType)
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

        var expected = ((IPuzzle)puzzle).GetExpectedOutput(runType);

        return new RunResult(output, parseStopwatch.Elapsed, runStopwatch.Elapsed, expected);
    }
    
    private void WarnOnOutputIncorrect(Output? expected, Output actual)
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

        return expected.Value == actual.Value;
    }

    private record RunResult(Output Output, TimeSpan ParseDuration, TimeSpan RunDuration, Output? Expected)
    {
        public TimeSpan TotalDuration => ParseDuration + RunDuration;
    }
}