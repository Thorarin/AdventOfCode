namespace Thorarin.AdventOfCode.Year2021.Day23;

internal class State
{
    private const int HallwaySize = 11;
    internal const int RoomCount = 4;
    private const char Empty = default!;
        
    public State(int roomSize)
    {
        RoomSize = roomSize;
        Hallway = new char[HallwaySize];
        Rooms = new char[RoomCount][];
        for (int i = 0; i < RoomCount; i++)
        {
            Rooms[i] = new char[RoomSize];
        }

        Finished = IsFinished();

#if DEBUG
            MoveList = new List<string>();
#endif
    }

    private State(State previous) : this(previous.RoomSize)
    {
        Array.Copy(previous.Hallway, Hallway, Hallway.Length);

        for (int roomNr = 0; roomNr < RoomCount; roomNr++)
        {
            Array.Copy(previous.Rooms[roomNr], Rooms[roomNr], RoomSize);
        }

        Energy = previous.Energy;
        Finished = previous.Finished;

#if DEBUG
            MoveList = previous.MoveList.ToList();
#endif
    }

    public char[] Hallway { get; }
    public char[][] Rooms { get; }
    
    private int RoomSize { get; }
    
    public bool Finished { get; private set; }

    public int Energy { get; private set; }
        
#if DEBUG
        public List<string> MoveList { get; private set; }
#endif

    public IEnumerable<State> Moves()
    {
        bool movedIntoRoom = false;
            
        for (int hallwayPos = 0; hallwayPos < HallwaySize; hallwayPos++)
        {
            char pod = Hallway[hallwayPos]; 
            if (pod != Empty)
            {
                if (CanMoveIntoRoom(hallwayPos, out int targetRoomPos))
                {
                    yield return MoveIntoRoom(hallwayPos, PodBay(pod), targetRoomPos);
                    movedIntoRoom = true;
                }                                        
            }
        }

        // Moving into final spot is always best, no need to consider other movements.
        if (movedIntoRoom) yield break;

        foreach (var move in MovesIntoHallway())
        {
            yield return move;
        }
    }

    private IEnumerable<State> MovesIntoHallway()
    {
        for (int roomNr = 0; roomNr < RoomCount; roomNr++)
        {
            var room = Rooms[roomNr];
            var nativeBug = RoomNativePod(roomNr);

            if (!room.Any(x => x != Empty && x != nativeBug)) continue;
                
            var entrance = PodBayDoor(roomNr);
            int min = FindMin(entrance);
            int max = FindMax(entrance);                

            for (int roomPos = 0; roomPos < RoomSize; roomPos++)
            {
                if (room[roomPos] == Empty) continue;
                    
                for (int hallPos = min; hallPos <= max; hallPos++)
                {
                    if (!IsAllowedHallwayPosition(hallPos)) continue;
                    yield return MoveIntoHallway(roomNr, roomPos, hallPos);
                }

                break;
            }
        }

        int FindMin(int doorPos)
        {
            for (int min = doorPos; min >= 0; min--)
            {
                if (Hallway[min] != Empty) return min + 1;
            }

            return 0;
        }

        int FindMax(int doorPos)
        {
            for (int max = doorPos; max < HallwaySize; max++)
            {
                if (Hallway[max] != Empty) return max - 1;
            }

            return HallwaySize - 1;
        }
    }
        
    private State Clone()
    {
        return new State(this);
    }

    /// <summary>
    /// Check if an amphipod can move into a room. If it can, this method returns
    /// the position it should move to (as far as possible into the room) 
    /// </summary>
    /// <param name="hallwayPos">The hallway position of the amphipod being moved.</param>
    /// <param name="targetRoomPos">The position inside the room that should be moved to.</param>
    /// <returns>True if the amphipod can move into its room.</returns>
    private bool CanMoveIntoRoom(int hallwayPos, out int targetRoomPos)
    {
        var pod = Hallway[hallwayPos];
        var roomNr = PodBay(pod);
        var room = Rooms[roomNr];

        targetRoomPos = -1;
        if (room[0] != Empty) return false;
            
        for (int i = 0; i < RoomSize; i++)
        {
            if (room[i] == Empty)
            {
                targetRoomPos = i;
            }
            else if (room[i] != pod)
            {
                return false;
            }
        }

        if (targetRoomPos == -1) return false;
            
        // Check if the way to the room entrance is clear
        var entrance = PodBayDoor(roomNr);
        var sign = Math.Sign(entrance - hallwayPos);
        for (int pos = hallwayPos + sign; pos != entrance; pos += sign)
        {
            if (Hallway[pos] != Empty) return false;
        }
            
        return true;
    }

    private State MoveIntoRoom(int hallwayPos, int roomNr, int roomPos)
    {
        var entrance = PodBayDoor(roomNr);
        int hallwaySteps = Math.Abs(entrance - hallwayPos);
        int roomSteps = roomPos + 1;
        int steps = hallwaySteps + roomSteps;

        var clone = Clone();
        var pod = clone.Hallway[hallwayPos];
        clone.Hallway[hallwayPos] = Empty;
        clone.Rooms[roomNr][roomPos] = pod;
        clone.Finished = clone.IsFinished();
        
        int cost = clone.AddCost(pod, steps);
            
#if DEBUG
        clone.MoveList.Add($"Move {pod} from hallway {hallwayPos} to room {roomNr}.{roomSteps} for {cost}");
#endif

        return clone;
    }

    private State MoveIntoHallway(int roomNr, int roomPos, int hallwayPos)
    {
        var pod = Rooms[roomNr][roomPos];
        if (pod == Empty) throw new Exception();

#if DEBUG
        if (roomPos > 0 && Rooms[roomNr].Take(roomPos).Any(x => x != Empty))
        {
            throw new Exception("Trying to move through some other crawlies!");
        }
#endif
            
        var entrance = PodBayDoor(roomNr);
        int steps = roomPos + 1 + Math.Abs(entrance - hallwayPos);

        var clone = Clone();
        clone.Hallway[hallwayPos] = pod;
        clone.Rooms[roomNr][roomPos] = Empty;
        int cost = clone.AddCost(pod, steps);
            
#if DEBUG
        clone.MoveList.Add($"Move {pod} from room {roomNr} to hallway {hallwayPos} for {cost}");
#endif
            
        return clone;
    }

    private int AddCost(char amphipod, int steps)
    {
        int cost = CostPerMove(amphipod) * steps;
        Energy += cost;
        return cost;
    }

    private static int CostPerMove(char amphipod)
    {
        return amphipod switch
        {
            'A' => 1,
            'B' => 10,
            'C' => 100,
            'D' => 1000,
            _ => throw new ArgumentOutOfRangeException(nameof(amphipod), amphipod, null)
        };
    }

    private static int PodBayDoor(int roomNr)
    {
        return roomNr * 2 + 2;
    }

    private static char RoomNativePod(int roomNr)
    {
        return (char)('A' + roomNr);
    }
        
    private static int PodBay(char amphipod)
    {
        return amphipod - 'A';
    }

    private static bool IsAllowedHallwayPosition(int pos)
    {
        return pos % 2 == 1 || pos == 0 || pos == HallwaySize - 1;
    }

    private bool IsFinished()
    {
        for (int roomNr = 0; roomNr < RoomCount; roomNr++)
        {
            var nativePod = RoomNativePod(roomNr);
            for (int i = 0; i < RoomSize; i++)
            {
                var room = Rooms[roomNr];
                if (room[i] != nativePod) return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Unused, but I tried using A* with this heuristic. It wasn't any faster.
    /// </summary>
    public int CalculateHeuristic()
    {
        int heuristic = 0;
        for (int hallPos = 0; hallPos < HallwaySize; hallPos++)
        {
            if (Hallway[hallPos] == Empty) continue;
            int stepsToGetToRoom = PodBayDoor(PodBay(Hallway[hallPos]));

            heuristic += (Math.Abs(hallPos - stepsToGetToRoom) + 1) * CostPerMove(Hallway[hallPos]);
        }

        return heuristic;
    }
        
    public override string ToString()
    {
        return new string(Hallway.Select(x => x == Empty ? '_' : x).ToArray());
    }        
}