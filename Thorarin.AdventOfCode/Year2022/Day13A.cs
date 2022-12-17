using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Pathfinding;
using Thorarin.AdventOfCode.Year2022.Day13;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 13, Part = 1)]
public class Day13A : Puzzle
{
    private List<(Packet Left, Packet Right)> _packets;

    public override void ParseInput(TextReader reader)
    {
        _packets = new List<(Packet, Packet)>();


        var pairs = reader.AsLines().Chunk(3);

        foreach (var pair in pairs)
        {
            _packets.Add((Packet.Parse(pair[0]), Packet.Parse(pair[1])));
        }
    }

    public override Output SampleExpectedOutput => 13;

    public override Output ProblemExpectedOutput => 5825;


    public override Output Run()
    {
        var indices = _packets
            .Select((pair, index) => (Comparison: pair.Left.CompareTo(pair.Right), Index: index + 1))
            .Where(x => x.Comparison < 0)
            .Select(x => x.Index)
            .ToList();

        return _packets
            .Select((pair, index) => (Comparison: pair.Left.CompareTo(pair.Right), Index: index + 1))
            .Where(x => x.Comparison < 0)
            .Sum(x => x.Index);

        return indices.Sum();
    }
    
}