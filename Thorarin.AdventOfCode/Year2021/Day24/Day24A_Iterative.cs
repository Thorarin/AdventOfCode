using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021.Day24;

[Puzzle(Year = 2021, Day = 24, Part = 1)]
public class Day24A_Iterative : Day24Base
{
    // The sample file is actually a second problem input from another participant
    public override Output SampleExpectedOutput => 99995969919326;
    public override Output ProblemExpectedOutput => 92793949489995;

    public override Output Run()
    {
        var state = new State(-1, 0, Snippets.ToArray(), new long[4]);
        var highestNumber = FindHighestIterative(state);
        if (!highestNumber.HasValue) return 0;
        return highestNumber;
    }

    private long? FindHighestIterative(State startState)
    {
        ICollection<State> states = new List<State> { startState };
        for (int i = 0; i < 14; i++)
        {
            //Console.WriteLine($"Processing digit {i + 1:00}, {states.Count} states");
            Dictionary<long, State> newStates = new Dictionary<long, State>();

            var pruneFunc = Prune(i);
            
            foreach (var state in states)
            {
                for (int d = 9; d > 0; d--)
                {
                    if (pruneFunc(state.Z, d))
                    {
                        var nw = state.TryExecute(d);
                        if (nw != null)
                        {
                            newStates.TryAdd(nw.Z, nw);
                        }
                    }
                }
            }

            states = newStates.Values.ToList();
        }

        return states.Where(x => x.Z == 0).Select(x => x.Number).Max();
    }
}