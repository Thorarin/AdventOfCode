using System.Text;
using JetBrains.Annotations;

namespace Thorarin.AdventOfCode.Year2021;

public partial class Day16B_Binary
{
    public class BitReader
    {
        private const int ByteSize = 8;
        private const int UintSize = sizeof(uint) * ByteSize;
        private const int BitsPerHexDigit = 4;
        private const int HexDigitsPerUint = UintSize / BitsPerHexDigit;

        private readonly uint[] _data;
        private readonly int _bitLength;
        private int _bitPosition;

        public BitReader(uint[] data, int bitBitLength)
        {
            _data = data;
            _bitLength = bitBitLength;
        }

        public static BitReader ParseHexString(string hexString)
        {
            if (BitConverter.IsLittleEndian)
            {
                var (quotient, remainder) = Math.DivRem(hexString.Length, HexDigitsPerUint);
                var bits = new uint[remainder > 0 ? quotient + 1 : quotient];

                for (int p = 0; p < hexString.Length - HexDigitsPerUint; p += HexDigitsPerUint)
                {
                    bits[p / HexDigitsPerUint] = Convert.ToUInt32(hexString.Substring(p, HexDigitsPerUint), 16);
                }

                if (remainder > 0)
                {
                    uint rawValue = Convert.ToUInt32(hexString.Substring(hexString.Length - remainder, remainder), 16);
                    bits[^1] = rawValue << (UintSize - remainder * BitsPerHexDigit);
                }

                var length = hexString.Length * BitsPerHexDigit;

                return new BitReader(bits, length);
            }

            // Endianness wouldn't be an issue if I were to just use a byte array,
            // but was a little slower when I tried it, and the code was full of casts because bit shift
            // operations on Byte still return an Int32 for some reason.
            
            throw new NotImplementedException("Not implemented for big endian platforms.");
        }

        public bool ReadAsBool([ValueRange(1, 32)] int bitsToRead)
        {
            GuardBitsToRead(bitsToRead, sizeof(uint) * ByteSize);
            return ReadBits(bitsToRead) != 0;
        }

        public int ReadAsInt32([ValueRange(1, 32)] int bitsToRead)
        {
            GuardBitsToRead(bitsToRead, sizeof(int) * ByteSize);
            return (int)ReadBits(bitsToRead);
        }

        public uint ReadAsUInt32([ValueRange(1, 32)] int bitsToRead)
        {
            GuardBitsToRead(bitsToRead, sizeof(uint) * ByteSize);
            return ReadBits(bitsToRead);
        }

        private uint ReadBits([ValueRange(1, 32)] int bitsToRead)
        {
            int toBeRead = bitsToRead;
            uint value = 0;
            while (toBeRead > 0)
            {
                var (uintPos, bitPos) = Math.DivRem(_bitPosition, UintSize);

                int canBeRead = UintSize - bitPos;
                int bitsRead = Math.Min(canBeRead, toBeRead);
                int shiftRight = UintSize - bitPos - bitsRead;

                // Mask and shift right to get only the bits we want
                uint mask = (UInt32.MaxValue << bitPos) >> bitPos;
                value |= (_data[uintPos] & mask) >> shiftRight;

                _bitPosition += bitsRead;
                toBeRead -= bitsRead;

                if (toBeRead > 0)
                {
                    value <<= toBeRead;
                }
            }

            return value;
        }

        public int BitPosition => _bitPosition;

        public override string ToString()
        {
            StringBuilder sb = new();

            foreach (var x in _data)
            {
                sb.Append(Convert.ToHexString(BitConverter.GetBytes(x).Reverse().ToArray()));
            }

            sb.Length = _bitLength / BitsPerHexDigit;

            return sb.ToString();
        }

        [AssertionMethod]
        private static void GuardBitsToRead(int bitsToRead, int maxBits)
        {
            if (bitsToRead > maxBits)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(bitsToRead), bitsToRead, $"Cannot read more than {maxBits} bits at once.");
            }
        }
    }
}