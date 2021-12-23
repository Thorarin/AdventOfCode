using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021.Day23;

[Puzzle(Year = 2021, Day = 23, Part = 2)]
public class Day23B : Puzzle
{
    private State _state = null!;

    public override Output SampleExpectedOutput => 44169;

    public override Output ProblemExpectedOutput => 48708;

    public override void ParseInput(TextReader reader)
    {
        reader.ReadLine();
        reader.ReadLine();

        _state = new State(roomSize: 4);

        for (int l = 0; l < 4; l += 3)
        {
            var line = reader.ReadLine()!;
            for (int room = 0; room < State.RoomCount; room++)
            {
                _state.Rooms[room][l] = line[room * 2 + 3];
            }
        }

        // Extra data from the "folded diagram"
        _state.Rooms[0][1] = 'D';
        _state.Rooms[0][2] = 'D';
        _state.Rooms[1][1] = 'C';
        _state.Rooms[1][2] = 'B';
        _state.Rooms[2][1] = 'B';
        _state.Rooms[2][2] = 'A';
        _state.Rooms[3][1] = 'A';
        _state.Rooms[3][2] = 'C';
    }

    public override Output Run()
    {
        int min = int.MaxValue;
        List<string> moveList = new();
        
        DoMoves(_state, ref min, ref moveList);

        #if DEBUG
        foreach (var line in moveList)
        {
            Console.WriteLine("Moves for shortest solution: ");
            Console.WriteLine(line);
        }
        #endif
        
        return min;
    }    

    private void DoMoves(State state, ref int minimumEnergy, ref List<string> moveList)
    {
        #if DEBUG
        // Makes for less confusing debugging
        var moves = state.Moves().ToList();
        #else
        var moves = state.Moves();
        #endif
        
        foreach (var move in moves)
        {
            if (move.Finished)
            {
                if (move.Energy < minimumEnergy)
                {
                    minimumEnergy = move.Energy;
                    #if DEBUG
                    moveList = move.MoveList;
                    #endif
                }
            }
            else if (move.Energy < minimumEnergy)
            {
                DoMoves(move, ref minimumEnergy, ref moveList);
            }
        }
    }
}