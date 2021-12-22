using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Year2021.Day22;

namespace Thorarin.AdventOfCode.Tests.Year2021.Day22;

public static class AssertionExtensions
{
    public static CuboidAssertions Should(this IEnumerable<Cuboid> instance)
    {
        return new CuboidAssertions(instance);
    }

    public class CuboidAssertions : GenericCollectionAssertions<Cuboid>
    {
        public CuboidAssertions(IEnumerable<Cuboid> actualValue) : base(actualValue)
        {
        }
        
        public AndConstraint<CuboidAssertions> BeValidSplitOf(Cuboid cuboid, string because = "", params object[] becauseArgs)
        {
            HaveNoIntersectingCuboids();

            var totalVolume = Subject.Sum(x => x.Volume);

            Execute.Assertion
                .ForCondition(totalVolume == cuboid.Volume)
                .FailWith(
                    "Expected {context:collection} the total volume of {0} to be equal to {1} (the volume of the original cuboid), but found {2}",
                    Subject, cuboid.Volume, totalVolume);

            return new AndConstraint<CuboidAssertions>(this);
        }
        
        public AndConstraint<CuboidAssertions> HaveNoIntersectingCuboids(string because = "", params object[] becauseArgs)
        {
            IEnumerable<Cuboid> unexpectedItems = Subject.Where(c => Subject.Any(c2 => !c2.Equals(c) && c2.Intersects(c)));

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(!unexpectedItems.Any())
                .FailWith(
                    "Expected {context:collection} {0} to not have any cuboids intersecting with each other {reason}, but found {1}.",
                    Subject, unexpectedItems);

            return new AndConstraint<CuboidAssertions>(this);
        }              
        
    }
    
  
}