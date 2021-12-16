using System.Text;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 16, Part = 1)]
public class Day16A : Puzzle
{
    private BitReader _reader;
    private List<int> _versions;

    public override Output? SampleExpectedOutput => 12;

    public override void ParseInput(TextReader reader)
    {
        _reader = new BitReader(reader.ReadToEnd());        
    }

    public override Output Run()
    {
        _versions = new List<int>();
        try
        {
            ReadPacket();
        }
        catch (EndOfStreamException)
        {
        }

        return _versions.Sum();
    }

    private void ReadPacket()
    {
        int version = _reader.ReadBits(3);
        _versions.Add(version);
        //Console.WriteLine(version);
        int typeId = _reader.ReadBits(3);

        switch (typeId)
        {
            case 4:
                // Literal value
                int value = 0;
                while (true)
                {
                    value <<= 4;
                    var t = _reader.ReadBits(5);
                    value += (t & 0xF);
                    if ((t & 0b10000) == 0)
                        break;
                }
                break;
            default:
                // Operator
                if (_reader.ReadBits(1) == 0)
                {
                    int length = _reader.ReadBits(15);
                    int start = _reader.Pos;
                    int end = start + length;

                    while (_reader.Pos < end)
                    {
                        ReadPacket();
                    }
                }
                else
                {
                    int packets = _reader.ReadBits(11);
                    for (int i = 0; i < packets; i++)
                    {
                        ReadPacket();
                    }
                }
                break;
        }
    }
    



    private class BitReader
    {
        private int _pos = 0;
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
            if (_pos + nr > _bits.Length)
            {
                throw new EndOfStreamException();
            }
            
            int result = Convert.ToInt32(_bits.Substring(_pos, nr), 2);
            _pos += nr;
            return result;
        }

        public int Pos => _pos;

        public bool End => _pos > _bits.Length;
    }
}