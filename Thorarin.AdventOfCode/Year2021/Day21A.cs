using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 21, Part = 1)]
public class Day21A : Puzzle
{
    private PlayerState[] _players;
    
    public override Output SampleExpectedOutput => 739785;

    public override Output ProblemExpectedOutput => 711480;

    public override void ParseInput(TextReader reader)
    {
        _players = new PlayerState[2];
        
        _players[0] = new PlayerState(int.Parse(reader.ReadLine()!.Split(':')[1]));
        _players[1] = new PlayerState(int.Parse(reader.ReadLine()!.Split(':')[1]));
    }

    public override Output Run()
    {
        int playerTurn = 0;
        int roll = 6;
        int rolls = 0;

        while (_players.All(p => p.Score < 1000))
        {
            _players[playerTurn].Move(roll);
            roll += 9;
            rolls += 3;

            //Console.WriteLine(" " + _players[playerTurn].Position + " " + _players[playerTurn].Position);
            
            playerTurn = (playerTurn + 1) % 2;
        }

        var loser = _players.First(p => p.Score < 1000);

        return loser.Score * rolls;
    }


    public class PlayerState
    {
        public PlayerState(int Position)
        {
            this.Position = Position;
        }

        public int Score { get; private set; }
        public int Position { get; private set; }

        public void Move(int rolled)
        {
            Position = (Position - 1 + rolled) % 10 + 1;
            Score += Position;
        }

        public void Deconstruct(out int Position)
        {
            Position = this.Position;
        }
    }
}