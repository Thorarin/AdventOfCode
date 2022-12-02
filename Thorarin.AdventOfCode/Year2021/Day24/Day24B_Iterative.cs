using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021.Day24;

[Puzzle(Year = 2021, Day = 24, Part = 2)]
public class Day24B_Iterative : Day24Base
{
    // The sample file is actually a second problem input from another participant
    public override Output SampleExpectedOutput => 48_111_514_719_111;
    public override Output ProblemExpectedOutput => 51_131_616_112_781;

    public override Output Run()
    {
        var state = new State(-1, 0, Snippets.ToArray(), new long[4]);
        var lowestNumber = FindLowestIterative(state);
        if (!lowestNumber.HasValue) return 0;
        return lowestNumber;
    }
    
    private long? FindLowestIterative(State startState)
    {
        ICollection<State> states = new List<State> { startState };
        for (int i = 0; i < 14; i++)
        {
            Dictionary<long, State> newStates = new Dictionary<long, State>();

            var pruneFunc = Prune(i);
            
            foreach (var state in states)
            {
                for (int d = 1; d <= 9; d++)
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

        return states.Where(x => x.Z == 0).Select(x => x.Number).Min();
    }
}