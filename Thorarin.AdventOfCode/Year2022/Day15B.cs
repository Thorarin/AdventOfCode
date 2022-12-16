using System.Text.RegularExpressions;
using MoreLinq;
using Thorarin.AdventOfCode.Extensions;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Pathfinding;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 15, Part = 2)]
public class Day15B : Puzzle
{
    private List<Line> _pairs = new();
    private int _space;

    public override void ParseInput(TextReader reader)
    {
        var regex = new Regex("Sensor at x=(?<SensorX>-?\\d+), y=(?<SensorY>-?\\d+): closest beacon is at x=(?<BeaconX>-?\\d+), y=(?<BeaconY>-?\\d+)");

        foreach (var line in reader.AsLines())
        {
            var match = regex.Match(line);

            _pairs.Add(new Line(
                int.Parse(match.Groups["SensorX"].ValueSpan),
                int.Parse(match.Groups["SensorY"].ValueSpan),
                int.Parse(match.Groups["BeaconX"].ValueSpan),
                int.Parse(match.Groups["BeaconY"].ValueSpan)));
        }

    }

    public override Output SampleExpectedOutput => 56000011;

    public override Output ProblemExpectedOutput => 12_051_287_042_458;

    public override Output Run()
    {
        if (_pairs.Count == 14)
        {
            _space = 20;
        }
        else
        {
            _space = 4_000_000;
        }

        foreach (var pair in _pairs)
        {
            pair.ManhattanDistance = Math.Abs(pair.SensorX - pair.BeaconX) + Math.Abs(pair.SensorY - pair.BeaconY);
        }

        //_pairs = _pairs.Where(p => Math.Abs(p.SensorY - row) > p.ManhattanDistance).ToList();

        var buffer = new bool[_space + 1];


        //Enumerable.Range(0, _space + 1)
        //    .AsParallel()


        for (int row = 0; row <= _space; row++)
        {
            var result = InspectRow2(row);

            if (result != -1)
            {
                return result * 4_000_000 + row;
            }

            if (row % 1000 == 0)
            {
                //Console.WriteLine(row);
            }
        }

        return -1;
    }

    private long InspectRow(int row, bool[] buffer)
    {
        buffer.Fill(false);

        foreach (var pair in _pairs)
        {
            int distanceX = pair.ManhattanDistance - Math.Abs(pair.SensorY - row);

            for (int i = 0; i <= distanceX; i++)
            {
                if (pair.SensorX - i >= 0 && pair.SensorX - i <= _space)
                    buffer[pair.SensorX - i] = true;

                if (pair.SensorX + i >= 0 && pair.SensorX + i <= _space)
                    buffer[pair.SensorX + i] = true;
            }
        }

        return Array.IndexOf(buffer, false);
    }

    private long InspectRow2(int row)
    {
        for (int x = 0; x < _space; x++)
        {
            bool covered = false;

            foreach (var pair in _pairs)
            {
                int distanceX = pair.ManhattanDistance - Math.Abs(pair.SensorY - row);

                if (distanceX >= 0 && x >= pair.SensorX - distanceX && x <= pair.SensorX + distanceX)
                {
                    covered = true;

                    if (x < pair.SensorX + distanceX - 1)
                        x = pair.SensorX + distanceX - 1;
                    
                    break;
                }
            }

            if (!covered) return x;
        }

        return -1;
    }


    private record Line(int SensorX, int SensorY, int BeaconX, int BeaconY)
    {
        public int ManhattanDistance { get; set; }
    }
}