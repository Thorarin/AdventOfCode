using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 25, Part = 1)]
public class Day25A : Puzzle
{
    private string[] _lines;

    public override void ParseInput(TextReader reader)
    {
        _lines = reader.AsLines().ToArray();
    }

    public override StringOutput SampleExpectedOutput => "2=-1=0";
    
    public override StringOutput ProblemExpectedOutput => "2=-0=01----22-0-1-10";
 
    public override Output Run()
    {
        long sum = _lines.Select(ParseSnafu).Sum();
        return ToSnafu(sum);
    }

    private static long ParseSnafu(string s)
    {
        long value = 0;
        long digitWeight = 1;

        for (int i = s.Length - 1; i >= 0; i--)
        {
            value += DigitValue(s[i]) * digitWeight;
            digitWeight *= 5;
        }

        return value;
    }

    private static int DigitValue(char digit)
    {
        return digit switch
        {
            '2' => 2,
            '1' => 1,
            '0' => 0,
            '-' => -1,
            '=' => -2
        };
    }

    private static char ToDigit(int value)
    {
        return value switch
        {
            2 => '2',
            1 => '1',
            0 => '0',
            -1 => '-',
            -2 => '=',
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public static string ToSnafu(long value)
    {
        long maxValue = 0;
        long digitWeight = 1;
        int digits = 1;

        for (; digits < 28; digits++)
        {
            maxValue += 2 * digitWeight;
            if (maxValue >= Math.Abs(value)) break;
            digitWeight *= 5;
        }

        return string.Create(digits, (value, digitWeight, maxValue), (span, state) =>
        {
            long weight = state.digitWeight;
            long limit = state.maxValue;
            long remaining = state.value;

            for (int i = 0; weight >= 1; i++, weight /= 5)
            {
                limit -= 2 * weight;
                var digit = (int)((Math.Abs(remaining) + limit) / weight * Math.Sign(remaining));
                span[i] = ToDigit(digit);
                remaining -= digit * weight;
            }
        });
    }
}