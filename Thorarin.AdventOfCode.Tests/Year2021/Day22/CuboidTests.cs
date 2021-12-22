using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Thorarin.AdventOfCode.Year2021.Day22;

namespace Thorarin.AdventOfCode.Tests.Year2021.Day22;

public class CuboidTests
{
    [Test]
    public void SplitY_SplitIntoThree_Test()
    {
        var a = new Cuboid(0, 10, 0, 10, 0, 10);
        var b = new Cuboid(0, 10, 3, 5, 0, 10);

        var split = a.SplitY(b).ToList();
        split.Should()
            .BeValidSplitOf(a)
            .And.HaveCount(3, "top of b below the top of a, and the bottom of b is above the bottom of a");
    }

    [Test]
    [TestCase(-2, 5)]
    [TestCase(8, 15)]
    public void SplitY_SplitIntoTwo_Test(int y1, int y2)
    {
        var sut = new Cuboid(0, 10, 0, 10, 0, 10);
        var b = new Cuboid(0, 10, y1, y2, 0, 10);

        var split = sut.SplitY(b).ToList();
        split.Should()
            .BeValidSplitOf(sut)
            .And.HaveCount(2);
    }
    
    /// <summary>
    /// Test that verifies we're not splitting into more cuboids than necessary.
    /// A very naive algorithm might split this into 26 adjacent cuboids.
    /// </summary>
    [Test]
    public void Remove_Test()
    {
        var outerCuboid = new Cuboid(0, 99, 0, 99, 0, 99);
        var innerCuboid = new Cuboid(20, 79, 20, 79, 20, 79);

        var hollowCuboid = outerCuboid.Remove(innerCuboid).ToList();

        hollowCuboid.Should().HaveCount(6);
    }       
}