using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 10, Part = 1)]
public class Day10A : Puzzle
{
    private List<string> _lines;
    private bool[] _screen = new bool[240];

    public override void ParseInput(TextReader reader)
    {
        _lines = reader.AsLines().ToList();
    }

    public override Output SampleExpectedOutput => 13140;

    public override Output ProblemExpectedOutput => 13920;


    public override Output Run()
    {
        int x = 1;
        int cycle = 0;

        List<int> strengths = new();

        foreach (var line in _lines)
        {
            switch (line.Substring(0, 4))
            {
                case "addx":
                {
                    int increment = int.Parse(line.Substring(5));

                    cycle++;
                    RegisterSignalStrength();
                    // Nothing happens first cycle

                    cycle++;
                    RegisterSignalStrength();
                    x += increment;
                    break;
                }
                case "noop":
                    // Do nothing
                    cycle++;
                    RegisterSignalStrength();
                    break;
            }
        }

        return strengths.Sum();

        void RegisterSignalStrength()
        {
            if ((cycle - 20) % 40 == 0)
            {
                strengths.Add(cycle * x);
            }
        }
    }
}