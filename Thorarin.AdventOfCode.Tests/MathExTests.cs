using FluentAssertions;
using NUnit.Framework;

namespace Thorarin.AdventOfCode.Tests;

public class MathExTests
{
    [Test]
    [TestCase(10, 0, 1)]
    [TestCase(10, 2, 100)]
    [TestCase(10, 5, 100000)]
    public void IntPowTest(int x, int pow, int expected)
    {
        MathEx.IntPow(x, pow).Should().Be(expected);
    }
}