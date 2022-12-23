using Thorarin.AdventOfCode.Extensions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 23, Part = 1)]
public class Day23A : Puzzle
{
    private ISet<(int X, int Y)> _elves = new HashSet<(int X, int Y)>();

    public override void ParseInput(TextReader reader)
    {
        int y = 0;
        foreach (var line in reader.AsLines())
        {
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == '#') _elves.Add((x, y));
            }

            y++;
        }
    }

    public override Output SampleExpectedOutput => 110;

    public override Output ProblemExpectedOutput => 4236;

    public override Output Run()
    {
        var board = Simulate();

        int minX = Int32.MaxValue;
        int minY = Int32.MaxValue;
        int maxX = Int32.MinValue;
        int maxY = Int32.MinValue;

        foreach (var elf in board)
        {
            minX = Math.Min(minX, elf.X);
            minY = Math.Min(minY, elf.Y);
            maxX = Math.Max(maxX, elf.X);
            maxY = Math.Max(maxY, elf.Y);
        }

        int count = 0;

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (!board.Contains((x, y))) count++;
            }
        }

        return count;
    }

    public void Visualize(ISet<(int X, int Y)> board)
    {
        int minX = Int32.MaxValue;
        int minY = Int32.MaxValue;
        int maxX = Int32.MinValue;
        int maxY = Int32.MinValue;

        foreach (var elf in board)
        {
            minX = Math.Min(minX, elf.X);
            minY = Math.Min(minY, elf.Y);
            maxX = Math.Max(maxX, elf.X);
            maxY = Math.Max(maxY, elf.Y);
        }

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                Console.Write(board.Contains((x, y)) ? '#' : '.');
            }
            Console.WriteLine();
        }
    }

    public ISet<(int X, int Y)> Simulate()
    {
        ISet<(int X, int Y)> board = _elves.ToHashSet();

        for (int round = 0; round < 10; round++)
        {
           
            //Console.WriteLine();
            //Console.WriteLine($"Round {round + 1} ({board.Count})");
            //Visualize(board);

            Dictionary<(int X, int Y), List<(int X, int Y)>> moves = new();

            foreach (var elf in board)
            {
                DetermineMove(elf, round, moves);
            }

            board = MakeBoard(moves);
        }

        void DetermineMove((int X, int Y) elf, int round, Dictionary<(int X, int Y), List<(int X, int Y)>> moves)
        {
            var adjacent = GetAdjacent(elf, board);

            if (adjacent == 0)
            {
                AddMove(elf, elf);
                return;
            }

            for (int rule = round; rule < round + 4; rule++)
            {
                switch (rule % 4)
                {
                    case 0:
                        if ((adjacent & 131) == 0)
                        {
                            AddMove(elf, (elf.X, elf.Y - 1));
                            return;
                        }

                        break;
                    case 1:
                        if ((adjacent & 56) == 0)
                        {
                            AddMove(elf, (elf.X, elf.Y + 1));
                            return;
                        }

                        break;
                    case 2:
                        if ((adjacent & 224) == 0)
                        {
                            AddMove(elf, (elf.X - 1, elf.Y));
                            return;
                        }

                        break;
                    case 3:
                        if ((adjacent & 14) == 0)
                        {
                            AddMove(elf, (elf.X + 1, elf.Y));
                            return;
                        }

                        break;
                }
            }

            AddMove(elf, elf);
            return;
           
            
            void AddMove((int X, int Y) from, (int X, int Y) to)
            {
                moves.AddOrUpdate(to, new List<(int X, int Y)> { from }, list =>
                {
                    list.Add(from);
                    return list;
                });
            }

        }

        ISet<(int X, int Y)> MakeBoard(Dictionary<(int X, int Y), List<(int X, int Y)>> moves)
        {
            var b = new HashSet<(int X, int Y)>();

            foreach (var entry in moves)
            {
                if (entry.Value.Count == 1)
                {
                    b.Add(entry.Key);
                }
                else
                {
                    b.AddRange(entry.Value);
                }
            }

            return b;
        }

        return board;
    }

    private byte GetAdjacent((int X, int Y) pos, ISet<(int X, int Y)> board)
    {
        byte result = 0;

        if (board.Contains((pos.X, pos.Y - 1))) result |= 1;
        if (board.Contains((pos.X + 1, pos.Y - 1))) result |= 2;
        if (board.Contains((pos.X + 1, pos.Y))) result |= 4;
        if (board.Contains((pos.X + 1, pos.Y + 1))) result |= 8;
        if (board.Contains((pos.X, pos.Y + 1))) result |= 16;
        if (board.Contains((pos.X - 1, pos.Y + 1))) result |= 32;
        if (board.Contains((pos.X - 1, pos.Y))) result |= 64;
        if (board.Contains((pos.X - 1, pos.Y - 1))) result |= 128;

        return result;
    }

}


