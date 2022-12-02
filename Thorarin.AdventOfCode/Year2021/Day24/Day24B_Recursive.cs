using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021.Day24;

[Puzzle(Year = 2021, Day = 24, Part = 2)]
public class Day24B_Recursive : Day24Base
{
    // The sample file is actually a second problem input from another participant
    public override Output SampleExpectedOutput => 48_111_514_719_111;
    public override Output ProblemExpectedOutput => 51_131_616_112_781;

    public override Output Run()
    {
        var state = new State(-1, 0, Snippets.ToArray(), new long[4]);
        var lowestNumber = FindLowestRecursive(state, GetAllPruneFunctions());
        if (!lowestNumber.HasValue) return 0;
        return lowestNumber;
    }
    
    private long? FindLowestRecursive(State state, Func<long, int, bool>[] pruneFunctions)
    {
        for (int d = 1; d <= 9; d++)
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

                var digits = FindLowestRecursive(newState, pruneFunctions);
                if (digits != null) return digits;
            }
        }

        return null;
    }
}