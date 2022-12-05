using System.Reflection;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode;

internal class Program
{
    internal static Task Main(string[] args)
    {
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly |
                                                   BindingFlags.NonPublic |
                                                   BindingFlags.Public | BindingFlags.Instance |
                                                   BindingFlags.Static))
            {
                if ((method.Attributes & MethodAttributes.Abstract) == MethodAttributes.Abstract || method.ContainsGenericParameters)
                {
                    continue;
                }
                System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(method.MethodHandle);
            }
        }

        return Parser.Default.ParseArguments<Options>(args).WithParsedAsync(RunWithOptions);
    }

    private static async Task RunWithOptions(Options options)
    {
        var serviceProvider = CreateServiceProvider();
        
        var puzzleFinder = new PuzzleFinder();
        List<Type> puzzleTypes;

        if (!string.IsNullOrEmpty(options.Implementation))
        {
            puzzleTypes = puzzleFinder.GetByImplementationName(options.Implementation).ToList();
        }
        else if (options.Year.HasValue)
        {
            if (options.Day.HasValue)
            {
                puzzleTypes = puzzleFinder.GetPuzzlesForDay(options.Year.Value, options.Day.Value).ToList();
            }
            else
            {
                puzzleTypes = puzzleFinder.GetPuzzlesForYear(options.Year.Value).ToList();    
            }
        }
        else
        {
            var date = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-5));
            puzzleTypes = puzzleFinder.GetPuzzlesForDate(date).ToList();
        }
        
        Console.WriteLine($"Found {puzzleTypes.Count} puzzles: {string.Join(", ", puzzleTypes.Select(x => x.Name))}");
        Console.WriteLine();

        var runner = new Runner(serviceProvider);
        
        foreach (var puzzleType in puzzleTypes)
        {
            await runner.RunImplementation(puzzleType, options.Iterations, options.Warmup);
            Console.WriteLine();
        }
    }

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        var store = SecretStore.Load("secrets.json");
        services.AddSingleton(store);

        return services.BuildServiceProvider();
    }
    
}