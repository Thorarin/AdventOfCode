using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Year2021.Day18;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 18, Part = 2)]
public class Day18B : Puzzle
{
    private List<Number> _numbers = new();

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
            _numbers.Add(Number.Parse(line));
        }
    }

    public override Output Run()
    {
        Answer max = Answer.Empty;

        foreach (var a in _numbers)
        {
            foreach (var b in _numbers)
            {
                if (b == a) continue;
                var sum = a + b;
                var magnitude = sum.GetMagnitude();

                if (max.Value < magnitude)
                {
                    max = new Answer(magnitude, a, b, sum);
                }
            }
        }
       
        return max;
    }

    private record Answer(long Value, string Left, string Right, string Reduced) : Output(Value)
    {
        public Answer(long value, Number left, Number right, Number reduced) 
            : this(value, left.ToString(), right.ToString(), reduced.ToString())
        {
        }

        public static Answer Empty => new(0, "", "", "");
    }

}