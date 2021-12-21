using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 21, Part = 2)]
public class Day21B : Puzzle
{
    private GameState _game;
    
    public override Output SampleExpectedOutput => 444356092776315;

    public override Output ProblemExpectedOutput => 265_845_890_886_828;

    public override void ParseInput(TextReader reader)
    {
        _game = new GameState();
        
        _game.Players[0] = new PlayerState(0, int.Parse(reader.ReadLine()!.Split(':')[1]));
        _game.Players[1] = new PlayerState(0, int.Parse(reader.ReadLine()!.Split(':')[1]));
    }

    public override Output Run()
    {
      var result = _game.TakeTurn();
  
      return Math.Max(result.Item1, result.Item2);
    }

    public record GameState()
    {
        private static readonly int[] _chances = { 0, 0, 0, 1, 3, 6, 7, 6, 3, 1 };
        
        public PlayerState[] Players { get; init; } = new PlayerState[2];

        public int PlayerTurn { get; init; }

        public long Multiplier { get; init; } = 1;
        
        public (long, long) TakeTurn()
        {
            var w = GetWins();
            if (w.HasValue)
            {
                return w.Value;
            }
            
            long player1 = 0;
            long player2 = 0;
            
            for (int roll = 3; roll <= 9; roll++)
            {
                var newState = Play(roll, _chances[roll]);
                var wins = newState.TakeTurn();

                player1 += wins.Item1;
                player2 += wins.Item2;
            }

            return (player1, player2);
        }

        private GameState Play(int roll, long multiplier)
        {
            var state = new[] { Players[0], Players[1] };

            state[PlayerTurn] = state[PlayerTurn].Move(roll);

            return this with
            {
                Players = state,
                PlayerTurn = (PlayerTurn + 1) % 2,
                Multiplier = Multiplier * multiplier
            };
        }

        public (long, long)? GetWins()
        {
            if (Players[0].Score >= 21)
            {
                return (Multiplier, 0);
            }

            if (Players[1].Score >= 21)
            {
                return (0, Multiplier);
            }

            return null;
        }
    }
        
    public record PlayerState(int Score, int Position)
    {
        public PlayerState Move(int rolled)
        {
            int newPosition = (Position - 1 + rolled) % 10 + 1;
            return this with
            {
                Position = newPosition,
                Score = Score + newPosition
            };
        }
    }
}