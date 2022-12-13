using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022.Day13
{
    public class NumberPacket : Packet
    {
        public int Number { get; set; }

        public new static NumberPacket Parse(string data)
        {
            return new NumberPacket { Number = int.Parse(data) };
        }

        public static NumberPacket Parse(StringReader reader)
        {
            var data = reader.ReadUntilExcluding(new[] { ',', ']' });
            return Parse(data!);
        }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}
