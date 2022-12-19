using System.Reflection;
using System.Runtime.InteropServices;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Ocr;

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

        var query = puzzleFinder.Query();

        if (!options.Year.HasValue && !options.Day.HasValue && string.IsNullOrEmpty(options.Implementation))
        {
            var date = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-5));
            puzzleTypes = puzzleFinder.GetPuzzlesForDate(date).ToList();
        }
        else
        {
            if (options.Year.HasValue)
            {
                if (options.Day.HasValue)
                {
                    query.ForDay(options.Year.Value, options.Day.Value);
                }
                else
                {
                    query.ForYear(options.Year.Value);
                }
            }
            else if (options.Day.HasValue)
            {
                query.ForDay(options.Day.Value);
            }

            if (!string.IsNullOrEmpty(options.Implementation))
            {
                query.WithName(options.Implementation);
            }

            puzzleTypes = query.Find().ToList();
        }

        RunnerBase runner = options.TableOutput ? new TableRunner(serviceProvider) : new ConsoleRunner(serviceProvider);

        runner.BeforeRuns(puzzleTypes);

        foreach (var puzzleType in puzzleTypes)
        {
            await runner.RunImplementation(puzzleType, options.Iterations, options.Warmup, options.RunExtraInputs);
        }
    }

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        var store = SecretStore.Load("secrets.json");
        services.AddSingleton(store);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            services.AddTransient<IOcrService, OcrSpaceOcrService>();
        }

        return services.BuildServiceProvider();
    }
    
}