using FluentAssertions;
using NUnit.Framework;
using Thorarin.AdventOfCode.Year2021.Day22;

namespace Thorarin.AdventOfCode.Tests.Year2021.Day22;

public class ReactorTests
{
    /// <summary>
    /// Not exactly Arrange, Act, Assert style, but tests a lot of the methods of
    /// the Reactor class in conjunction.
    /// </summary>
    [Test]
    public void Reactor_Test()
    {
        var sut = new Reactor();

        // Start with two non-intersecting cuboids
        sut.TurnOn(new Cuboid(0, 10, 0, 10, 0, 10));
        sut.TurnOn(new Cuboid(100, 110, 0, 10, 0, 10));

        long expectedCubesOn = (11 * 11 * 11) * 2;
        sut.CubesOn.Should().Be(expectedCubesOn, "we have two 11 by 11 by 11 cubes");
        sut.Cuboids.Should().HaveNoIntersectingCuboids().And.HaveCount(2);

        // Hollow out the first cuboid.
        // We need at least 6 cuboids to represent a hollow one. 
        sut.TurnOff(new Cuboid(1, 9, 1, 9, 1, 9));
        expectedCubesOn -= (9 * 9 * 9);
        sut.CubesOn.Should().Be(expectedCubesOn, "a 9 by 9 by 9 volume was removed from one of the cubes");
        sut.Cuboids.Should().HaveNoIntersectingCuboids().And.HaveCount(7);

        // Place a new cuboid inside the hollow area
        sut.TurnOn(new Cuboid(2, 8, 2, 8, 2, 8));
        expectedCubesOn += (7 * 7 * 7);
        sut.CubesOn.Should().Be(expectedCubesOn, "a 7 by 7 by 7 cube was turned on inside the hollow cube");
        sut.Cuboids.Should().HaveNoIntersectingCuboids().And.HaveCount(8);

        // Slice the top of the second cuboid (with a larger cuboid than necessary)
        sut.TurnOff(new Cuboid(105, 300, -10, 20, -10, 20));
        sut.Cuboids.Should().HaveNoIntersectingCuboids();
        expectedCubesOn -= (6 * 11 * 11);
        sut.CubesOn.Should().Be(expectedCubesOn, "a 6 by 11 by 11 cuboid was sliced off the second cuboid");
        sut.Cuboids.Should().HaveNoIntersectingCuboids().And.HaveCount(8);
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
}