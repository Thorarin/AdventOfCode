using Thorarin.AdventOfCode.Extensions;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Coordinates.TwoDimensional.Integer;

namespace Thorarin.AdventOfCode.Year2022;

public abstract class Day23Base : Puzzle
{
    private readonly ISet<Pos> _elves = new HashSet<Pos>();

    public override void ParseInput(TextReader reader)
    {
        int y = 0;
        foreach (var line in reader.AsLines())
        {
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == '#') _elves.Add(new Pos(x, y));
            }

            y++;
        }
    }

    public sealed override Output Run()
    {
        return Run(_elves);
    }

    public abstract Output Run(ISet<Pos> board);

    protected Rectangle GetBounds(ISet<Pos> board)
    {
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;

        foreach (var elf in board)
        {
            minX = Math.Min(minX, elf.X);
            minY = Math.Min(minY, elf.Y);
            maxX = Math.Max(maxX, elf.X);
            maxY = Math.Max(maxY, elf.Y);
        }

        return new Rectangle(new Pos(minX, minY), new Pos(maxX, maxY));
    }

    protected void Visualize(ISet<Pos> board)
    {
        var (minX, minY, maxX, maxY) = GetBounds(board);

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                Console.Write(board.Contains(new Pos(x, y)) ? '#' : '.');
            }
            Console.WriteLine();
        }
    }

    protected (ISet<Pos> board, int rounds) Simulate(ISet<Pos> initial, int maxRounds)
    {
        ISet<Pos> board = initial.ToHashSet();

        for (int round = 0; round < maxRounds; round++)
        {
            Dictionary<Pos, List<Pos>> moves = new();
            foreach (var elf in board)
            {
                DetermineMove(elf, round, moves, board);
            }

            if (!UpdateBoard(board, moves))
            {
                return (board, round + 1);
            }
        }

        return (board, maxRounds);
    }

    private void DetermineMove(Pos elf, int round, Dictionary<Pos, List<Pos>> moves, ISet<Pos> board)
    {
        var adjacent = GetAdjacent(elf, board);

        if (adjacent == 0) return;

        for (int rule = round; rule < round + 4; rule++)
        {
            switch (rule % 4)
            {
                case 0:
                    if ((adjacent & Directions.MoveNorth) == 0)
                    {
                        AddMove(elf, new Pos(elf.X, elf.Y - 1));
                        return;
                    }
                    break;
                case 1:
                    if ((adjacent & Directions.MoveSouth) == 0)
                    {
                        AddMove(elf, new Pos(elf.X, elf.Y + 1));
                        return;
                    }
                    break;
                case 2:
                    if ((adjacent & Directions.MoveWest) == 0)
                    {
                        AddMove(elf, new Pos(elf.X - 1, elf.Y));
                        return;
                    }
                    break;
                case 3:
                    if ((adjacent & Directions.MoveEast) == 0)
                    {
                        AddMove(elf, new Pos(elf.X + 1, elf.Y));
                        return;
                    }
                    break;
            }
        }

        return;
           
        void AddMove(Pos from, Pos to)
        {
            moves.AddOrUpdate(to, new List<Pos> { from }, list =>
            {
                list.Add(from);
                return list;
            });
        }
    }

    private bool UpdateBoard(ISet<Pos> board, Dictionary<Pos, List<Pos>> moves)
    {
        bool changed = false;

        foreach (var entry in moves)
        {
            if (entry.Value.Count == 1)
            {
                board.Remove(entry.Value[0]);
                board.Add(entry.Key);
                changed = true;
            }
        }

        return changed;
    }

    /// <summary>
    /// Returns a flags enum of occupied adjacent positions.
    /// </summary>
    private Directions GetAdjacent(Pos pos, ISet<Pos> board)
    {
        Directions result = 0;

        if (board.Contains((pos.X, pos.Y - 1)))     result |= Directions.North;
        if (board.Contains((pos.X + 1, pos.Y - 1))) result |= Directions.NorthEast;
        if (board.Contains((pos.X + 1, pos.Y)))     result |= Directions.East;
        if (board.Contains((pos.X + 1, pos.Y + 1))) result |= Directions.SouthEast;
        if (board.Contains((pos.X, pos.Y + 1)))     result |= Directions.South;
        if (board.Contains((pos.X - 1, pos.Y + 1))) result |= Directions.SouthWest;
        if (board.Contains((pos.X - 1, pos.Y)))     result |= Directions.West;
        if (board.Contains((pos.X - 1, pos.Y - 1))) result |= Directions.NorthWest;

        return result;
    }

    [Flags]
    protected enum Directions : byte
    {
        North = 1,
        NorthEast = 2,
        East = 4,
        SouthEast = 8,
        South = 16,
        SouthWest = 32,
        West = 64,
        NorthWest = 128,

        MoveNorth = NorthWest | North | NorthEast,
        MoveSouth = SouthWest | South | SouthEast,
        MoveWest  = NorthWest | West  | SouthWest,
        MoveEast  = NorthEast | East  | SouthEast,
    }
}