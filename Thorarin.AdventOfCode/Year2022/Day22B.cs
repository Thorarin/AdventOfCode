using System.Diagnostics;
using Thorarin.AdventOfCode.Extensions;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Year2022.Day22;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 22, Part = 2)]
public class Day22B : Puzzle
{
    private char[,] _grid = new char[0, 0];
    private List<string> _instructions = new();

    public override void ParseInput(TextReader reader)
    {
        var lines = reader.AsLines().ToArray();

        int columns = lines[..^1].Max(line => line.Length);
        int rows = lines.Length - 2;

        _grid = new char[columns, rows];
        _grid.Fill(' ');

        for (int y = 0; y < rows; y++)
        {
            string line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                _grid[x, y] = line[x];
            }
        }

        var sep = new[] { 'L', 'R' };
        var r = new StringReader(lines[^1]);

        while (r.Peek() != -1)
        {
            string amount = r.ReadUntilExcluding(sep)!;
            _instructions.Add(amount);

            int d = r.Read();
            if (d == -1) break;
            _instructions.Add("" + (char)d);
        }
    }

    public override Output SampleExpectedOutput => 5031;

    public override Output ProblemExpectedOutput => 144012;

    public override Output Run()
    {
        int cubeSize = Math.Min(_grid.GetLength(0), _grid.GetLength(1)) / 3;
        int x = GetStartingColumn();
        int y = 0;
        Direction direction = 0;


        Dictionary<(int X, int Y, Direction D), Rule> wrapRules = new();

        if (cubeSize == 4)
        {
            return 0;
        }
        else
        {
            wrapRules.Add((1, 0, Direction.Up),    new Rule(Direction.Right, from => (0, 150 + from.X - 50)));
            wrapRules.Add((1, 0, Direction.Left),  new Rule(Direction.Right, from => (0, 149 - from.Y)));
            wrapRules.Add((2, 0, Direction.Up),    new Rule(Direction.Up,    from => (from.X - 100, 199)));
            wrapRules.Add((2, 0, Direction.Right), new Rule(Direction.Left,  from => (99, 149 - from.Y)));
            wrapRules.Add((2, 0, Direction.Down),  new Rule(Direction.Left,  from => (99, 50 + from.X - 100)));
                                                                             
            wrapRules.Add((1, 1, Direction.Left),  new Rule(Direction.Down,  from => (0 + from.Y - 50, 100)));
            wrapRules.Add((1, 1, Direction.Right), new Rule(Direction.Up,    from => (100 + from.Y - 50, 49)));
                                                                             
            wrapRules.Add((0, 2, Direction.Left),  new Rule(Direction.Right, from => (50, 149 - from.Y)));
            wrapRules.Add((0, 2, Direction.Up),    new Rule(Direction.Right, from => (50, 50 + from.X)));
            wrapRules.Add((1, 2, Direction.Right), new Rule(Direction.Left,  from => (149, 149 - from.Y)));
            wrapRules.Add((1, 2, Direction.Down),  new Rule(Direction.Left,  from => (49, 150 + from.X - 50)));
                                                                             
            wrapRules.Add((0, 3, Direction.Left),  new Rule(Direction.Down,  from => (50 + from.Y - 150, 0)));
            wrapRules.Add((0, 3, Direction.Right), new Rule(Direction.Up,    from => (50 + from.Y - 150, 149)));
            wrapRules.Add((0, 3, Direction.Down),  new Rule(Direction.Down,  from => (100 + from.X, 0)));
        }

        foreach (var instruction in _instructions)
        {
            if (int.TryParse(instruction, out int amount))
            {
                Move(amount);
            }
            else if (instruction == "R")
            {
                direction = direction.TurnRight();
            }
            else if (instruction == "L")
            {
                direction = direction.TurnLeft();
            }
        }

        return (y + 1) * 1000 + (x + 1) * 4 + (int)direction;


        void Move(int amount)
        {
            int i = 0;
            while (i < amount && TryMove()) i++;
        }

        bool TryMove()
        {
            switch (direction)
            {
                case Direction.Right:
                    return MoveRight();
                case Direction.Down :
                    return MoveDown();
                case Direction.Left:
                    return MoveLeft();
                case Direction.Up:
                    return MoveUp();
            }

            throw new Exception();
        }

        bool MoveRight()
        {
            int max = _grid.GetLength(0);
            int tmp = x + 1;

            if (tmp >= max || _grid[tmp, y] == ' ')
            {
                return TryApplyRule();
            }

            if (_grid[tmp, y] == '#') return false;

            x = tmp;
            return true;
        }

        bool MoveDown()
        {
            int max = _grid.GetLength(1);
            int tmp = y + 1;

            if (tmp >= max || _grid[x, tmp] == ' ')
            {
                return TryApplyRule();
            }

            while (_grid[x, tmp] == ' ') tmp++;

            if (_grid[x, tmp] == '#') return false;

            y = tmp;
            return true;
        }

        bool MoveLeft()
        {
            int tmp = x - 1;

            if (tmp < 0 || _grid[tmp, y] == ' ')
            {
                return TryApplyRule();
            }

            if (_grid[tmp, y] == '#') return false;

            x = tmp;
            return true;
        }

        bool MoveUp()
        {
            int tmp = y - 1;

            if (tmp < 0 || _grid[x, tmp] == ' ')
            {
                return TryApplyRule();
            }

            if (_grid[x, tmp] == '#') return false;

            y = tmp;
            return true;
        }

        bool TryApplyRule()
        {
            int a = x / cubeSize;
            int b = y / cubeSize;

            //Console.WriteLine($"{x}, {y}: {a}, {b}, {direction}");

            var rule = wrapRules[(a, b, direction)];

            var tmp = rule.Move((x, y));

            if (_grid[tmp.X, tmp.Y] == '#') return false;

            int reverseA = tmp.X / cubeSize;
            int reverseB = tmp.Y / cubeSize;
            var reverseDirection = rule.Direction.Reverse();
            var reverseRule = wrapRules[(reverseA, reverseB, reverseDirection)];
            var reverse = reverseRule.Move((tmp.X, tmp.Y));
            if (reverse.X != x || reverse.Y != y)
            {
                Console.WriteLine($"{x} {y} {direction} -> {tmp.X} {tmp.Y} {rule.Direction} -> {reverse.X} {reverse.Y} {reverseRule.Direction} ({reverseA} {reverseB} {reverseDirection})");
                throw new Exception();
            }

            x = tmp.X;
            y = tmp.Y;
            direction = rule.Direction;

            if (_grid[x, y] != '.') throw new Exception();

            if (direction == Direction.Right && x % cubeSize != 0) throw new Exception();
            if (direction == Direction.Down  && y % cubeSize != 0) throw new Exception();
            if (direction == Direction.Left  && x % cubeSize != cubeSize - 1) throw new Exception();
            if (direction == Direction.Up    && y % cubeSize != cubeSize - 1) throw new Exception();

            // We stayed on the same plane. That's not supposed to happen
            if (x / cubeSize == a || y / cubeSize == b) throw new Exception();

            return true;
        }


        return 0;
    }

    private int GetStartingColumn()
    {
        for (int x = 0; x <= _grid.GetLength(1); x++)
        {
            if (_grid[x, 0] == '.') return x;
        }

        return -1;
    }

    private record Rule(Direction Direction, Func<(int X, int Y), (int X, int Y)> Move)
    {
    }
}