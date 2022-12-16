using System.Text.RegularExpressions;
using MoreLinq;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Pathfinding;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 15, Part = 1)]
public class Day15A : Puzzle
{
    private List<Line> _pairs = new();

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

    public override Output SampleExpectedOutput => 26;

    // 6349691 too high

    //public override Output ProblemExpectedOutput => 817;


    public override Output Run()
    {
        int row = _pairs.Count == 14 ? 10 : 2_000_000;

        foreach (var pair in _pairs)
        {
            pair.ManhattanDistance = Math.Abs(pair.SensorX - pair.BeaconX) + Math.Abs(pair.SensorY - pair.BeaconY);
        }

        //_pairs = _pairs.Where(p => Math.Abs(p.SensorY - row) > p.ManhattanDistance).ToList();

        HashSet<int> covered = new();

        foreach (var pair in _pairs)
        {
            int distanceX = pair.ManhattanDistance - Math.Abs(pair.SensorY - row);

            for (int i = 0; i <= distanceX; i++)
            {
                covered.Add(pair.SensorX - i);
                covered.Add(pair.SensorX + i);
            }
        }

        foreach (var pair in _pairs)
        {
            if (pair.BeaconY == row)
            {
                covered.Remove(pair.BeaconX);
            }

        }


        var a = covered.Order().ToList();

        return covered.Count;
    }

    private record Line(int SensorX, int SensorY, int BeaconX, int BeaconY)
    {
        public int ManhattanDistance { get; set; }
    }
}