using System.IO;
using FluentAssertions;
using NUnit.Framework;
using Thorarin.AdventOfCode.Year2021.Day24;

namespace Thorarin.AdventOfCode.Tests.Year2021.Day24;

public class Tests
{
    [Test]
    public void Test()
    {
        var ops = "mul z 0\r\nmod x w";
        var snippet = CodeSnippet.Parse(new StringReader(ops));

        var result = snippet.Run(new long[4], 9);
        result.Should().BeTrue();

        var result2 = snippet.Run(new long[4], 0);
        result2.Should().BeFalse();
        
        
    }
    
    
}