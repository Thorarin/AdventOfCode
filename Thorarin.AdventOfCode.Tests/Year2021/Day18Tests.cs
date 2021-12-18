using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Thorarin.AdventOfCode.Year2021.Day18;

namespace Thorarin.AdventOfCode.Tests.Year2021;

public class Day18Tests
{
    [Test]
    [TestCase("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]")]
    [TestCase("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]")]
    [TestCase("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]")]
    [TestCase("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]")]
    [TestCase("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
    [TestCase("[[[[4,0],[5,0]],[[[4,5],[2,6]],[9,5]]],[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]]", "[[[[4,0],[5,4]],[[0,[7,6]],[9,5]]],[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]]")]
    public void ExplodeTest(string before, string after)
    {
        Explode(before).Should().Be(after);
    }

    [Test]
    [TestCase("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
    public void ReduceTest(string before, string after)
    {
        Reduce(before).Should().Be(after);
    }

    [Test]
    public void SumTest()
    {
        Sum("[1,1]", "[2,2]", "[3,3]", "[4,4]", "[5,5]", "[6,6]").Should()
            .Be("[[[[5,0],[7,4]],[5,5]],[6,6]]");
    }
    
    [Test]
    public void SumTest2()
    {
        Sum("[1,1]", "[2,2]", "[3,3]", "[4,4]", "[5,5]").Should()
            .Be("[[[[3,0],[5,3]],[4,4]],[5,5]]");
    }    

    [Test]
    public void SumTest3()
    {
        Sum("[[[[4,3],4],4],[7,[[8,4],9]]]", "[1,1]")
            .Should().Be("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]");
    }          
    
    [Test]
    public void SumTest4()
    {
        Sum("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]",
            "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]")
            .Should().Be("[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]");
    }
    
    [Test]
    public void SumTest5()
    {
        var numbers = new[]
        {
            "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
            "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]"
        };
        
        var queue = new Queue<INumber>(numbers.Select(x => NumberPair.Parse(new StringReader(x))));
        var zeNumber = queue.Dequeue();

        while (queue.Count > 0)
        {
            zeNumber = zeNumber.Add(queue.Dequeue());
            zeNumber.Reduce();
        }

        zeNumber.ToString().Should().Be("[[[[7,8],[6,6]],[[6,0],[7,7]]],[[[7,8],[8,8]],[[7,9],[0,6]]]]");
    }    

    [Test]
    [TestCase("[[9,1],[1,9]]", 129)]
    public void MagnitudeTest()
    {
        
    }
    
    private static string Reduce(string number)
    {
        var reader = new StringReader(number);
        var sut = NumberPair.Parse(reader);
        sut.Reduce();
        return sut.ToString();
    }
    
    private static string Explode(string number)
    {
        var reader = new StringReader(number);
        var sut = NumberPair.Parse(reader);
        sut.Explode();
        return sut.ToString();
    }
    
    private static int Magnitude(string number)
    {
        var reader = new StringReader(number);
        var sut = NumberPair.Parse(reader);
        return sut.Magnitude();
    }

    private static string Sum(params string[] numbers)
    {
        var queue = new Queue<INumber>(numbers.Select(x => NumberPair.Parse(new StringReader(x))));
        var zeNumber = queue.Dequeue();

        while (queue.Count > 0)
        {
            zeNumber = zeNumber.Add(queue.Dequeue());
            zeNumber.Reduce();
            
            Console.WriteLine(zeNumber.ToString());
        }

        return zeNumber.ToString();
    }
}