using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Thorarin.AdventOfCode.Year2021.Day19;

namespace Thorarin.AdventOfCode.Tests.Year2021;

public class Day19Tests
{
    [Test]
    public void RotateTest()
    {
        var rotatedSets = new[]
        {
            new Point(-2, -3, 1),
            new Point(5, 4, 1)
        }.Rotate();

        var allPoints = rotatedSets.SelectMany(x => x).Distinct().ToList();

        allPoints.Should().Contain(new Point(-2, -3, 1));
        allPoints.Should().Contain(new Point(2, -1, 3));
        allPoints.Should().Contain(new Point(-1, -3, -2));
        allPoints.Should().Contain(new Point(1, 3, -2));
        allPoints.Should().Contain(new Point(3, 1, 2));
    }
}