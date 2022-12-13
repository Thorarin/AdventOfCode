using System.Text;

namespace Thorarin.AdventOfCode.Year2022.Day13
{
    public class ListPacket : Packet
    {
        public List<Packet> Packets { get; } = new();

        public new static Packet Parse(StringReader reader)
        {
            var result = new ListPacket();

            reader.Read();
            while (true)
            {
                if (reader.Peek() != ']')
                {
                    result.Packets.Add(Packet.Parse(reader));
                }

                var ch = reader.Read();
                if (ch == ',')
                {
                    // Expected separator
                }
                else if (ch == ']')
                {
                    break;
                }
                else
                {
                    throw new Exception("Parse error");
                }
            }

            return result;
        }


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[");

            foreach (var packet in Packets)
            {
                sb.Append(packet);
                sb.Append(',');
            }

            if (Packets.Count > 0)
            {
                sb.Length--;
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}
