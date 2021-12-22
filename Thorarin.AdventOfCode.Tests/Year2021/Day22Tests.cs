using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Thorarin.AdventOfCode.Year2021.Day22;

namespace Thorarin.AdventOfCode.Tests.Year2021;

public class Day22Tests
{
    [Test]
    public void SplitYTest()
    {
        var sut = new Cuboid(0, 10, 0, 10, 0, 10);
        var b = new Cuboid(0, 10, 3, 5, 0, 10);

        var split = sut.SplitY(b).ToList();

        split.Should().HaveCount(3);
        AssertValidSplit(sut, split);
    }
    
    [Test]
    public void SplitYTest2()
    {
        var sut = new Cuboid(0, 10, 0, 10, 0, 10);
        var b = new Cuboid(0, 10, -2, 5, 0, 10);

        var split = sut.SplitY(b).ToList();

        split.Should().HaveCount(2);
        AssertValidSplit(sut, split);
    }
    
    [Test]
    public void SplitYTest3()
    {
        var sut = new Cuboid(0, 10, 0, 10, 0, 10);
        var b = new Cuboid(0, 10, 8, 15, 0, 10);

        var split = sut.SplitY(b).ToList();

        split.Should().HaveCount(2);
        AssertValidSplit(sut, split);
    }

    [Test]
    public void ReactorTest()
    {
        var sut = new Reactor();
        
        sut.TurnOn(new Cuboid(0, 10, 0, 10, 0, 10));
        sut.TurnOn(new Cuboid(100, 110, 0, 10, 0, 10));

        long expectedCubesOn = (11 * 11 * 11) * 2;
        sut.CubesOn.Should().Be(expectedCubesOn);
        
        sut.TurnOff(new Cuboid(1, 9, 1, 9, 1, 9));

        expectedCubesOn -= (9 * 9 * 9);
        sut.CubesOn.Should().Be(expectedCubesOn);

        sut.TurnOn(new Cuboid(2, 8, 2, 8, 2, 8));
        AssertNoOverlaps(sut.Cuboids);
        expectedCubesOn += (7 * 7 * 7);
        sut.CubesOn.Should().Be(expectedCubesOn);
        
        sut.TurnOff(new Cuboid(105, 300, -10, 20, -10, 20));
        AssertNoOverlaps(sut.Cuboids);
        expectedCubesOn -= (6 * 11 * 11);
        sut.CubesOn.Should().Be(expectedCubesOn);
    }

    [Test]
    public void StupidFailingTestCaseINeedToDebug()
    {
        // These are some of the instructions from the sample file, but in code.
        // The assertions I had added were failing on the second TurnOff call.
        // (that is, some cuboids were still intersecting with the removed cuboid)
        var reactor = new Reactor();
        reactor.TurnOn(new Cuboid(-5, 47, -31, 22, -19, 33));
        reactor.TurnOn(new Cuboid(-44, 5, -27, 21, -14, 35));
        reactor.TurnOn(new Cuboid(-49, -1, -11, 42, -10, 38));
        reactor.TurnOn(new Cuboid(-20, 34, 40, 6, -44, 1));
        reactor.TurnOff(new Cuboid(26, 39, 40, 50, 2, 11));
        reactor.TurnOn(new Cuboid(-41, 5, -41, 6, -36, 8));
        reactor.TurnOff(new Cuboid(-43, -33, -45, -28, 7, 25));

        reactor.CubesOn.Should().Be(437_781L);
    }
    

    private static void AssertValidSplit(Cuboid original, IReadOnlyList<Cuboid> splitList)
    {
        AssertNoOverlaps(splitList);
        splitList.Sum(s => s.Volume).Should().Be(original.Volume);
    }

    private static void AssertNoOverlaps(IReadOnlyList<Cuboid> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if (j == i) continue;

                list[i].Intersects(list[j]).Should().BeFalse();
            }
        }
    }
    
}