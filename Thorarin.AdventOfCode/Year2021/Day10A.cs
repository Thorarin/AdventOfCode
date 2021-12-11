using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 10, Part = 1)]
public class Day10A : Puzzle
{
    private const string ChunkStarts = "([{<";
    private const string ChunkEnds = ")]}>";
    
    private static readonly Dictionary<char, int> Scores = new()
    {
        { ')', 3 },
        { ']', 57 },
        { '}', 1197 },
        { '>', 25137 }
    };

    private string[] _lines;

    public override Output? SampleExpectedOutput => 26397;

    public override void ParseInput(string[] fileLines)
    {
        _lines = fileLines;
    }

    public override Output Run()
    {
        int score = 0;
        
        foreach (string line in _lines)
        {
            var error = ParseLine(line);
            if (error.HasValue)
            {
                score += Scores[error.Value];
            }
        }

        return score;
    }

    private char? ParseLine(string line)
    {
        int pos = 0;
        var stack = new Stack<char>();
        foreach (var c in line)
        {
            pos++;
            if (ChunkStarts.Contains(c))
            {
                stack.Push(c);
            }
            else
            {
                char start = stack.Pop();
                if (!IsMatchingEnd(c, start))
                {
                    return c;
                }
            }
        }

        // Console.WriteLine("Incomplete line: " + line);
        
        return default;
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