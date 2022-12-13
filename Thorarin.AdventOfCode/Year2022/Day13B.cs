using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Pathfinding;
using Thorarin.AdventOfCode.Year2022.Day13;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 13, Part = 2)]
public class Day13B : Puzzle
{
    private List<Packet> _packets;

    public override void ParseInput(TextReader reader)
    {
        _packets = new List<Packet>();

        foreach (var packet in reader.AsLines())
        {
            if (packet.Length > 0)
            {
                _packets.Add(Packet.Parse(packet));
            }
        }
    }

    public override Output SampleExpectedOutput => 140;

    public override Output ProblemExpectedOutput => 24477;

    public override Output Run()
    {
        var divider1 = Packet.Parse("[[2]]");
        var divider2 = Packet.Parse("[[6]]");

        _packets.Add(divider1);
        _packets.Add(divider2);

        _packets.Sort();

        var index1 = _packets.IndexOf(divider1) + 1;
        var index2 = _packets.IndexOf(divider2) + 1;

        return index1 * index2;
    }
}