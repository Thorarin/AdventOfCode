using MoreLinq;

namespace Thorarin.AdventOfCode.Year2022.Day13
{
    public class Packet : IComparable<Packet>
    {
        public static Packet Parse(string data)
        {
            using var reader = new StringReader(data);
            return Parse(reader);
        }

        public static Packet Parse(StringReader reader)
        {
            if (reader.Peek() != '[')
            {
                return NumberPacket.Parse(reader);
            }

            return ListPacket.Parse(reader);
        }


        public int CompareTo(Packet? other)
        {
            //Console.WriteLine($"Compare {this} vs {other}");
            {
                if (this is ListPacket left && other is ListPacket right)
                {
                    // Both sides are ListPackets
                    foreach (var (innerLeft, innerRight) in left.Packets.ZipShortest(right.Packets, (a, b) => (a, b)))
                    {
                        var comparison = innerLeft.CompareTo(innerRight);
                        if (comparison != 0)
                        {
                            //Console.WriteLine(comparison);
                            return comparison;
                        }
                    }

                    return left.Packets.Count - right.Packets.Count;
                }
            }

            {
                if (this is NumberPacket left && other is NumberPacket right)
                {
                    // Both sides are NumberPackets
                    return left.Number - right.Number;
                }
            }

            {
                // Convert one side to a ListPacket if both are not the same type
                var left = this switch
                {
                    NumberPacket nr => new ListPacket { Packets = { nr } },
                    ListPacket l => l,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var right = other switch
                {
                    NumberPacket nr => new ListPacket { Packets = { nr } },
                    ListPacket l => l,
                    _ => throw new ArgumentOutOfRangeException()
                };

                return left.CompareTo(right);
            }
        }
    }
}
