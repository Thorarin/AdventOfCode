using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 10, Part = 2)]
public class Day10B : Puzzle
{
    private const string ChunkStarts = "([{<";
    private const string ChunkEnds = ")]}>";
    
    private static readonly Dictionary<char, int> Scores = new()
    {
        { '(', 1 },
        { '[', 2 },
        { '{', 3 },
        { '<', 4 }
    };

    private string[] _lines = Array.Empty<string>();

    public override Output SampleExpectedOutput => 288957;
    public override Output ProblemExpectedOutput => 2116639949;

    public override void ParseInput(TextReader reader)
    {
        _lines = reader.ToLineArray();
    }

    public override Output Run()
    {
        var scorePerLine = new List<long>();
        
        foreach (string line in _lines)
        {
            var remainingOnStack = ParseLine(line);
            if (remainingOnStack.Length > 0)
            {
                scorePerLine.Add(CalculateScore(remainingOnStack));
            }
        }
        
        scorePerLine.Sort();
        
        return scorePerLine[scorePerLine.Count / 2];
    }

    private char[] ParseLine(string line)
    {
        var stack = new Stack<char>();
        foreach (var c in line)
        {
            if (ChunkStarts.Contains(c))
            {
                stack.Push(c);
            }
            else
            {
                char start = stack.Pop();
                if (!IsMatchingEnd(c, start))
                {
                    return Array.Empty<char>();
                }
            }
        }

        return stack.ToArray();
    }

    private static long CalculateScore(char[] remainingOnStack)
    {
        long score = 0;
        foreach (var c in remainingOnStack)
        {
            score = score * 5 + Scores[c];
        }

        return score;
    }
    
    bool IsMatchingEnd(char end, char start)
    {
        int f = ChunkEnds.IndexOf(end);
        if (f == -1)
        {
            throw new Exception();
        }

        return ChunkStarts[f] == start;
    }    
}