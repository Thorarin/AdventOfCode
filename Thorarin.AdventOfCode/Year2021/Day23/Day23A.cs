﻿using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021.Day23;

[Puzzle(Year = 2021, Day = 23, Part = 1)]
public class Day23A : Puzzle
{
    private State _state;

    public override Output SampleExpectedOutput => 12521;

    public override Output? ProblemExpectedOutput => 13520;

    public override void ParseInput(TextReader reader)
    {
        reader.ReadLine();
        reader.ReadLine();

        _state = new State(roomSize: 2);

        for (int l = 0; l < 2; l++)
        {
            var line = reader.ReadLine();
            for (int room = 0; room < State.RoomCount; room++)
            {
                _state.Rooms[room][l] = line[room * 2 + 3];
            }
        }
    }

    public override Output Run()
    {
        int min = int.MaxValue;
        
        DoMoves(_state, ref min);

        return min;
    }

    private void DoMoves(State state, ref int minimumEnergy)
    {
        var nextStates = state.Moves().ToList();

        foreach (var next in nextStates)
        {
            if (next.Finished)
            {
                if (next.Energy < minimumEnergy)
                {
                    minimumEnergy = next.Energy;
                }
            }
            else if (next.Energy < minimumEnergy)
            {
                DoMoves(next, ref minimumEnergy);
            }
        }
    }
}