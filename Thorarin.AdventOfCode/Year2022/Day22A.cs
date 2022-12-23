using System.Diagnostics;
using Thorarin.AdventOfCode.Extensions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 22, Part = 1)]
public class Day22A : Puzzle
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

    public override Output SampleExpectedOutput => 6032;
    public override Output ProblemExpectedOutput => 76332;
 
    public override Output Run()
    {
        int x = GetStartingColumn();
        int y = 0;
        int direction = 0;

        foreach (var instruction in _instructions)
        {
            if (int.TryParse(instruction, out int amount))
            {
                Move(amount);
            }
            else if (instruction == "R")
            {
                direction = (direction + 1) % 4;
            }
            else if (instruction == "L")
            {
                direction = MathEx.Modulo(direction - 1, 4);
            }
        }

        return (y + 1) * 1000 + (x + 1) * 4 + direction;


        void Move(int amount)
        {
            int i = 0;
            while (i < amount && TryMove()) i++;
        }

        bool TryMove()
        {
            switch (direction)
            {
                case 0:
                    return MoveRight();
                case 1:
                    return MoveDown();
                case 2:
                    return MoveLeft();
                case 3:
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
                tmp = 0;
            }

            while (_grid[tmp, y] == ' ') tmp++;

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
                tmp = 0;
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
                tmp = _grid.GetLength(0) - 1;
            }

            while (_grid[tmp, y] == ' ') tmp--;

            if (_grid[tmp, y] == '#') return false;

            x = tmp;
            return true;
        }

        bool MoveUp()
        {
            int tmp = y - 1;

            if (tmp < 0 || _grid[x, tmp] == ' ')
            {
                tmp = _grid.GetLength(1) - 1;
            }

            while (_grid[x, tmp] == ' ') tmp--;

            if (_grid[x, tmp] == '#') return false;

            y = tmp;
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



}