using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Year2021.Day18;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 18, Part = 2)]
public class Day18B : Puzzle
{
    private List<string> _numbers = new();

    public override Output SampleExpectedOutput => new Answer(
        Value: 3993,
        Left: "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
        Right: "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]",
        Reduced: "[[[[7,8],[6,6]],[[6,0],[7,7]]],[[[7,8],[8,8]],[[7,9],[0,6]]]]");

    public override Output ProblemExpectedOutput => 4770;

    public override void ParseInput(TextReader reader)
    {
        _numbers = new();
        foreach (var line in reader.AsLines())
        {
            _numbers.Add(line);
        }
    }

    public override Output Run()
    {
        Answer max = new Answer(0, "", "", "");
        
        for (int a = 0; a < _numbers.Count; a++)
        {
            for (int b = 0; b < _numbers.Count; b++)
            {
                if (b == a) continue;

                var numberA = NumberPair.Parse(new StringReader(_numbers[a]));
                var numberB = NumberPair.Parse(new StringReader(_numbers[b]));
                var sum = numberA.Add(numberB);
                var reduced = sum.Reduce();
                var magnitude = reduced.Magnitude();

                if (max.Value < magnitude)
                {
                    max = new Answer(magnitude, _numbers[a], _numbers[b], reduced.ToString()!);
                }
            }
        }
        
        return max;
    }

    private record Answer(long Value, string Left, string Right, string Reduced) : Output(Value);

}