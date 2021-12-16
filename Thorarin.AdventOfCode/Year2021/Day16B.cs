using System.Diagnostics;
using System.Text;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 16, Part = 2)]
public class Day16B : Puzzle
{
    private BitReader _reader;

    public override Output? SampleExpectedOutput => 46;

    public override void ParseInput(TextReader reader)
    {
        _reader = new BitReader(reader.ReadToEnd());        
    }

    public override Output Run()
    {
        return ReadPacket();
    }

    private long ReadPacket()
    {
        int version = _reader.ReadBits(3);
        int typeId = _reader.ReadBits(3);

        switch (typeId)
        {
            case 4:
                // Literal value
                long value = 0;
                while (true)
                {
                    value <<= 4;
                    var t = _reader.ReadBits(5);
                    value += (t & 0xF);
                    if ((t & 0b10000) == 0)
                        return value;
                }
            case 0:
                // Sum
                return ReadOperator().Sum();
            case 1:
                // Product
                return ReadOperator().Aggregate(1L, (acc, next) => acc * next);
            case 2:
                // Minimum
                return ReadOperator().Min();
            case 3:
                // Maximum
                return ReadOperator().Max();
            case 5:
            {
                // GT
                var v = ReadOperator().ToList();
                Debug.Assert(v.Count == 2);
                return v[0] > v[1] ? 1 : 0;
            }
            case 6:
            {
                // LT
                var v = ReadOperator().ToList();
                Debug.Assert(v.Count == 2);
                return v[0] < v[1] ? 1 : 0;
            }
            case 7:
            {
                // EQ
                var v = ReadOperator().ToList();
                Debug.Assert(v.Count == 2);
                return v[0] == v[1] ? 1 : 0;
            }            
            default:
                throw new Exception("Invalid packet " + typeId);
        }

        IEnumerable<long> ReadOperator()
        {
            if (_reader.ReadBits(1) == 0)
            {
                int length = _reader.ReadBits(15);
                int start = _reader.Pos;
                int end = start + length;

                while (_reader.Pos < end)
                {
                    yield return ReadPacket();
                }
            }
            else
            {
                int packets = _reader.ReadBits(11);
                for (int i = 0; i < packets; i++)
                {
                    yield return ReadPacket();
                }
            }
        }
    }
    
    private class BitReader
    {
        private int _pos;
        private readonly string _bits;

        public BitReader(string hexString)
        {
            StringBuilder bits = new();
            
            for (int p = 0; p < hexString.Length; p++)
            {
                var s = Convert.ToString(Convert.ToInt32(hexString.Substring(p, 1), 16), 2);
                if (s.Length < 4) bits.Append('0', 4 - s.Length);
                bits.Append(s);
            }

            _bits = bits.ToString();
        }

        public int ReadBits(int nr)
        {
            int result = Convert.ToInt32(_bits.Substring(_pos, nr), 2);
            _pos += nr;
            return result;
        }

        public int Pos => _pos;
   }
}