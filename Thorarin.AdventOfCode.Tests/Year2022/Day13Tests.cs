using FluentAssertions;
using NUnit.Framework;
using Thorarin.AdventOfCode.Year2022.Day13;

namespace Thorarin.AdventOfCode.Tests.Year2022
{
    [TestFixture]
    public class Day13Tests
    {
        [Test]
        [TestCase("[1,[2,[3,[4,[5,6,7]]]],8,9]", "[1,[2,[3,[4,[5,6,0]]]],8,9]")]
        public void CompareTo_ReturnsPositiveNumber_WhenLeftIsGreater(string left, string right)
        {
            var packet1 = Packet.Parse(left);
            var packet2 = Packet.Parse(right);

            packet1.CompareTo(packet2).Should().BeGreaterThan(0);
            packet2.CompareTo(packet1).Should().BeLessThan(0);
        }

        [Test]
        [TestCase("[1,[2,[3,[4,[5,6,7]]]],8,9]")]
        public void CompareTo_ReturnsZero_WhenComparedToItself(string str)
        {
            var packet1 = Packet.Parse(str);
            var packet2 = Packet.Parse(str);

            packet1.CompareTo(packet2).Should().Be(0);
            packet2.CompareTo(packet1).Should().Be(0);
        }

        [Test]
        public void Parse_ReturnsEmptyListPacket_WhenParsingBrackets()
        {
            var packet = Packet.Parse("[]");

            packet.Should().BeOfType<ListPacket>().Which.Packets.Should().HaveCount(0);
        }
    }
}
