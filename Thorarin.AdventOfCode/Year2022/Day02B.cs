using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 2, Part = 2)]
public class Day02B : Puzzle
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

    private readonly List<(Move, Outcome)> _rounds = new();

    public override void ParseInput(TextReader reader)
    {
        foreach (string line in reader.AsLines())
        {
            var move = (Move)line[0] - 'A' + 1;
            var outcome = (Outcome)line[2] - 'X';
            _rounds.Add((move, outcome));
        }
    }

    public override Output SampleExpectedOutput => 12;

    public override Output Run()
    {
        return _rounds.Select(CalculateScore).Sum();
    }

    private int CalculateScore((Move a, Outcome b) round)
    {
        return (int)CalculateMove(round) + round.b switch
        {
            Outcome.Loss => 0,
            Outcome.Draw => 3,
            Outcome.Win => 6,
            _ => throw new NotImplementedException(),
        } ;
    }

    private Move CalculateMove((Move a, Outcome b) round)
    {
        if (round.b == Outcome.Draw) return round.a;

        return round.a switch
        {
            Move.Rock => round.b == Outcome.Win ? Move.Paper : Move.Scissors,
            Move.Paper => round.b == Outcome.Win ? Move.Scissors : Move.Rock,
            Move.Scissors => round.b == Outcome.Win ? Move.Rock : Move.Paper,
            _ => throw new NotImplementedException()
        };
    }


}