using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 2, Part = 1)]
public class Day02A : Puzzle
{
    private enum Move
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    private enum Outcome
    {
        Loss,
        Draw,
        Win
    }

    private readonly List<(Move, Move)> _rounds = new();

    public override void ParseInput(TextReader reader)
    {
        foreach (string line in reader.AsLines())
        {
            var a = (Move)line[0] - 'A' + 1;
            var b = (Move)line[2] - 'X' + 1;
            _rounds.Add((a, b));
        }
    }

    public override Output SampleExpectedOutput => 15;

    public override Output Run()
    {
        return _rounds.Select(CalculateScore).Sum();
    }

    private int CalculateScore((Move a, Move b) round)
    {
        return (int)round.b + CalculateOutcome(round) switch
        {
            Outcome.Loss => 0,
            Outcome.Draw => 3,
            Outcome.Win => 6,
            _ => throw new NotImplementedException(),
        };
    }

    private Outcome CalculateOutcome((Move a, Move b) round)
    {
        if (round.a == round.b) return Outcome.Draw;

        return round.a switch
        {
            Move.Rock => round.b == Move.Paper ? Outcome.Win : Outcome.Loss,
            Move.Paper => round.b == Move.Scissors ? Outcome.Win : Outcome.Loss,
            Move.Scissors => round.b == Move.Rock ? Outcome.Win : Outcome.Loss,
            _ => throw new NotImplementedException()
        };
    }


}