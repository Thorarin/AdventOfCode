using System.Diagnostics;
using System.Reflection;

namespace Thorarin.AdventOfCode.Framework;

public class Runner
{
    public void RunImplementation(Type type)
    {
        var attr = type.GetCustomAttribute<PuzzleAttribute>()!;
        Console.WriteLine($"Running {attr.Year}-{attr.Day}-{attr.Part}, implementation: {type.Name}");
        
        string sampleFileName = $"Year{attr.Year}\\Inputs\\day{attr.Day:00}-sample.txt";
        string problemFileName = $"Year{attr.Year}\\Inputs\\day{attr.Day:00}-problem.txt";
        
        Console.WriteLine("Running using sample data...");

        bool compareSampleOutput = true;
        
        if (!File.Exists(sampleFileName))
        {
            sampleFileName = problemFileName;
            compareSampleOutput = false;
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARNING: sample input not found. Using problem data.");
            Console.ResetColor();
        }
        
        var instance = (Puzzle)Activator.CreateInstance(type)!;
        var (sampleOutput, _, _) = Run(instance, sampleFileName);

        if (compareSampleOutput)
        {
            WarnOnOutputIncorrect(instance.SampleExpectedOutput, sampleOutput);
        }

        Console.WriteLine("Running using problem data...");
        
        instance = (Puzzle)Activator.CreateInstance(type)!;    
        var (output, parse, run) = Run(instance, problemFileName);

        var total = parse + run;
        var format = total.GetHumanReadableFormat();

        string totalString = total.FormatHumanReadable(format);
        string parseString = parse.FormatHumanReadable(format).PadLeft(totalString.Length);
        string runString = run.FormatHumanReadable(format).PadLeft(totalString.Length);
        
        Console.WriteLine($"Outcome: {output}");
        
        WarnOnOutputIncorrect(instance.ProblemExpectedOutput, output);
        
        Console.WriteLine("Time taken:");
        Console.WriteLine($"  Parse:   {parseString}");
        Console.WriteLine($"  Run:     {runString}");
        Console.WriteLine($"  Total:   {totalString}");
    }

    private (Output output, TimeSpan parse, TimeSpan run) Run(Puzzle day, string fileName)
    {
        var fileLines = File.ReadAllLines(fileName);

        var parseStopwatch = Stopwatch.StartNew();
        day.ParseInput(fileLines);
        parseStopwatch.Stop();

        var runStopwatch = Stopwatch.StartNew();
        var output = day.Run();
        runStopwatch.Stop();

        return (output, parseStopwatch.Elapsed, runStopwatch.Elapsed);
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
}