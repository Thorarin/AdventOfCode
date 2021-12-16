using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 16, Part = 2)]
public partial class Day16B_Binary : Puzzle
{
    private BitReader _reader;

    public override Output SampleExpectedOutput => 46;
    
    public override Output ProblemExpectedOutput => 912_901_337_844;

    public override void ParseInput(TextReader reader)
    {
        _reader = BitReader.ParseHexString(reader.ReadToEnd());
    }

    public override Output Run()
    {
        return ReadPacket();
    }

    private long ReadPacket()
    {
        int version = _reader.ReadAsInt32(3);
        int typeId = _reader.ReadAsInt32(3);

        switch (typeId)
        {
            case 4:
                // Literal value
                long value = 0;
                while (true)
                {
                    value <<= 4;
                    var literalPart = _reader.ReadAsInt32(5);
                    value += literalPart & 0xF;
                    if ((literalPart & 0x10) == 0)
                    {
                        return value;
                    }
                }
            case 0:
                // Sum
                return ReadSubPackets().Sum();
            case 1:
                // Product
                return ReadSubPackets().Aggregate(1L, (acc, next) => acc * next);
            case 2:
                // Minimum
                return ReadSubPackets().Min();
            case 3:
                // Maximum
                return ReadSubPackets().Max();
            case 5:
            {
                // Greater Than
                var v = ReadSubPackets().ToList();
                Debug.Assert(v.Count == 2);
                return v[0] > v[1] ? 1 : 0;
            }
            case 6:
            {
                // Less Than
                var v = ReadSubPackets().ToList();
                Debug.Assert(v.Count == 2);
                return v[0] < v[1] ? 1 : 0;
            }
            case 7:
            {
                // Equals
                var v = ReadSubPackets().ToList();
                Debug.Assert(v.Count == 2);
                return v[0] == v[1] ? 1 : 0;
            }            
            default:
                throw new Exception("Invalid packet " + typeId);
        }

        IEnumerable<long> ReadSubPackets()
        {
            if (_reader.ReadAsBool(1))
            {
                int packets = _reader.ReadAsInt32(11);
                for (int i = 0; i < packets; i++)
                {
                    yield return ReadPacket();
                }
            }
            else
            {
                int length = _reader.ReadAsInt32(15);
                int start = _reader.BitPosition;
                int end = start + length;

                while (_reader.BitPosition < end)
                {
                    yield return ReadPacket();
                }
            }
        }
    }
}