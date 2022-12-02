using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021.Day24;

[Puzzle(Year = 2021, Day = 24, Part = 1)]
public class Day24A_Recursive : Day24Base
{
    // The sample file is actually a second problem input from another participant
    public override Output SampleExpectedOutput => 99995969919326;
    public override Output ProblemExpectedOutput => 92793949489995;

    public override Output Run()
    {
        var state = new State(-1, 0, Snippets.ToArray(), new long[4]);
        var highestNumber = FindHighestRecursive(state, GetAllPruneFunctions());
        if (!highestNumber.HasValue) return 0;
        return highestNumber;
    }
    
    private long? FindHighestRecursive(State state, Func<long, int, bool>[] pruneFunctions)
    {
        for (int d = 9; d > 0; d--)
        {
            if (!pruneFunctions[state.Pos + 1](state.Z, d)) continue;
                
            var newState = state.TryExecute(d);
            if (newState != null)
            {
                if (newState.Pos == 13)
                {
                    if (newState.Registers[3] == 0)
                    {
                        return newState.Number;
                    }
                    return null;
                }

                var digits = FindHighestRecursive(newState, pruneFunctions);
                if (digits != null) return digits;
            }
        }

        return null;
    }
}