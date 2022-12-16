using System.Text.RegularExpressions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 16, Part = 2)]
public class Day16B : Puzzle
{
    private const int MaxMoves = 26;
    private Dictionary<string, Valve> _valves = new();
    private int _maxTotalPressure = 0;

    Dictionary<Record, (int, List<Move>)> _records = new();

    public override void ParseInput(TextReader reader)
    {
        var regex = new Regex("Valve (?<Valve>[A-Z]{2}) has flow rate=(?<FlowRate>\\d+); tunnel(s)? lead(s)? to valve(s)? (?<Tunnels>.*)");

        foreach (var line in reader.AsLines())
        {
            var match = regex.Match(line);
            if (!match.Success) Console.WriteLine(line);
            var valve = new Valve(
                match.Groups["Valve"].Value,
                int.Parse(match.Groups["FlowRate"].ValueSpan),
                match.Groups["Tunnels"].Value.Split(", "));

            _valves[valve.Name] = valve;
        }

    }

    // Intermediate 1327
    public override Output SampleExpectedOutput => 1707;

    // Intermediate 1561
    // Too low 2665
    // Correct 2790, but why? :P
    public override Output ProblemExpectedOutput => 2790;


    public override Output Run()
    {
        var state = new State
        {
            Valves = _valves,
            Position = "AA"
        };


        int maxPressure = 0;
        List<Move> moves = null;

        foreach (var m in GetMoves(state))
        {
            var (pressure, bla) = DoMoves(state, m);
            if (pressure > maxPressure)
            {
                maxPressure = pressure;
                moves = bla;

                if (pressure > _maxTotalPressure)
                {
                    _maxTotalPressure = pressure;
                    moves = bla;
                    Console.WriteLine(_maxTotalPressure);
                }
            }
        }

        state = new State
        {
            Valves = _valves,
            Position = "AA"
        };

        foreach (var a in moves)
        {
            //Console.WriteLine($"Minute {state.ElapsedTime}");
            //Console.WriteLine($"Released: {state.PressurePerMinute}");
            //Console.WriteLine(a.ToString());
            a.Do(state);
        }

        Console.WriteLine($"Intermediate {state.PressureTotal}");

        state.Moves.Clear();
        state.ElapsedTime = 0;
        state.Position = "AA";
        maxPressure = 0;
        moves = null;
        _records.Clear();

        //return state.PressureTotal;

        foreach (var m in GetMoves(state))
        {
            var (pressure, bla) = DoMoves(state, m);
            if (pressure > maxPressure)
            {
                maxPressure = pressure;
                moves = bla;

                if (pressure > _maxTotalPressure)
                {
                    _maxTotalPressure = pressure;
                    moves = bla;
                    Console.WriteLine(_maxTotalPressure);
                }
            }
        }

        foreach (var a in moves)
        {
            Console.WriteLine($"Minute {state.ElapsedTime}");
            //Console.WriteLine($"Released: {state.PressurePerMinute}");
            Console.WriteLine(a.ToString());
            a.Do(state);
        }

        //foreach (var v in _valves.Values.Where(v => v.FlowRate > 0).OrderBy(v => v.Name))
        //{
        //    Console.WriteLine($"Valve {v.Name} ({v.FlowRate}: {v.Opened}");
        //}

        //Console.WriteLine(_records.Values.Max());

        return maxPressure;
    }


    private (int, List<Move>) DoMoves(State state, Move move)
    {
        var expectedTotal = state.CalcExpectedTotal();
        var r = new Record(state.ElapsedTime, state.Position);
        if (_records.TryGetValue(r, out var record) && record.Item1 > expectedTotal + 100)
        {
            return (-1, new List<Move>());
        }

        if (record.Item1 < expectedTotal)
        {
            _records[r] = (expectedTotal, state.Moves.ToList());
        }

        if (state.ElapsedTime >= MaxMoves)
        {
            return (state.PressureTotal, state.Moves.ToList()); 
        }
        
        move.Do(state);

        int maxPressure = 0;
        List<Move> moves = null;

        foreach (var m in GetMoves(state))
        {
            var (pressure, bla) = DoMoves(state, m);

            if (pressure > maxPressure)
            {
                maxPressure = pressure;
                moves = bla;

                if (pressure > _maxTotalPressure)
                {
                    _maxTotalPressure = pressure;
                    Console.WriteLine(_maxTotalPressure);
                }
                
            }
        }

        move.Undo(state);

        return (maxPressure, moves);
    }

    private IEnumerable<Move> GetMoves(State state)
    {
        var valve = state.Valve;

        if (!valve.Opened && valve.FlowRate > 0)
        {
            yield return new OpenMove { Valve = valve.Name };
        }

        foreach (string tunnel in valve.Tunnels)
        {
            if (state.Moves.Count > 0 && state.Moves[^1] is TunnelMove p && p.From == tunnel)
                continue;

            yield return new TunnelMove { From = valve.Name, To = tunnel };
        }

        //yield return new WaitMove();
    }


    private record Valve(string Name, int FlowRate, IList<string> Tunnels)
    {
        public bool Opened { get; set; }
    }

    private abstract class Move
    {
        public abstract void Do(State state);
        public abstract void Undo(State state);
    }

    private class TunnelMove : Move
    {
        public string From { get; set; }
        public string To { get; set; }

        public override void Do(State state)
        {
            state.Position = To;
            state.DoTick(this);
        }

        public override void Undo(State state)
        {
            state.Position = From;
            state.UndoTick();
        }

        public override string ToString()
        {
            return $"Move from {From} to {To}";
        }
    }

    private class OpenMove : Move
    {
        public string Valve { get; set; }

        public override void Do(State state)
        {
            state.DoTick(this);

            var valve = state.Valves[Valve];
            valve.Opened = true;

            state.PressureTotal += (MaxMoves - state.Moves.Count) * valve.FlowRate;
        }

        public override void Undo(State state)
        {
            var valve = state.Valves[Valve];
            state.PressureTotal -= (MaxMoves - state.Moves.Count) * valve.FlowRate;
            valve.Opened = false;

            state.UndoTick();
        }

        public override string ToString()
        {
            return $"Open {Valve}";
        }
    }

    private class WaitMove : Move
    {
        public override void Do(State state)
        {
            state.DoTick(this);
        }

        public override void Undo(State state)
        {
            state.UndoTick();
        }
    }

    private class State
    {
        public State()
        {
            Moves = new List<Move>();
        }

        public Dictionary<string, Valve> Valves { get; set; }
        public string Position { get; set; }
        public int ElapsedTime { get; set; }
        public List<Move> Moves { get; set; }
        public int PressureTotal { get; set; }

        public void DoTick(Move move)
        {
            ElapsedTime++;

            Moves.Add(move);
        }

        public void UndoTick()
        {
            ElapsedTime--;

            Moves.RemoveAt(Moves.Count - 1);
        }

        public Valve Valve => Valves[Position];

        public int CalcExpectedTotal()
        {
            return PressureTotal;
        }
    }

    private record struct Record(int Time, string Position) {}
}