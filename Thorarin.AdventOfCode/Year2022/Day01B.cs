using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 1, Part = 2)]
public class Day01B : Puzzle
{
    private readonly List<int> _foodCalories = new();

    public override void ParseInput(TextReader reader)
    {
        _foodCalories.Add(0);

        foreach (string line in reader.AsLines())
        {
            if (line == "")
            {
                _foodCalories.Add(0);
                continue;
            }

            _foodCalories[^1] += int.Parse(line);
        }
    }

    public override Output SampleExpectedOutput => 45000;

    public override Output Run()
    {
        return _foodCalories.OrderByDescending(x => x).Take(3).Sum();
    }
}