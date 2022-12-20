using FluentAssertions;
using NUnit.Framework;
using Thorarin.AdventOfCode.Extensions;

namespace Thorarin.AdventOfCode.Tests.Extensions
{
    [TestFixture]
    public class ArrayExtensionsTests
    {
        [Test]
        public void MoveTest()
        {
            var array = new[] { 0, 1, 2, 3, 4, 5 };

            array.Move(4, 0);
            array.Move(1, 4);

            array.Should().BeEquivalentTo(new[] { 4, 1, 2, 3, 0, 5 });
        }
    }
}
