using MoreLinq;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 7, Part = 1)]
public class Day07A : Puzzle
{
    private string[] _data;

    public override void ParseInput(TextReader reader)
    {
        _data = reader.ToLineArray();
    }

    public override Output SampleExpectedOutput => 95437;

    public override Output ProblemExpectedOutput => 1447046;

    public override Output Run()
    {
        Stack<Directory> dir = new();

        List<Directory> found = new();

        foreach (string line in _data)
        {
            if (line.StartsWith("$ cd"))
            {
                string dirName = line.Substring(5);
                if (dirName == "/")
                {
                    dir.Clear();
                    dir.Push(new Directory(dirName));
                }
                else if (dirName == "..")
                {
                    var tmp = dir.Pop();
                    dir.Peek().Size += tmp.Size;

                    if (tmp.Size <= 100_000)
                    {
                        found.Add(tmp);
                    }
                }
                else
                {
                    dir.Push(new Directory(dirName));
                }
            }
            else if (line.StartsWith("$ ls"))
            {
                // nothing
            }
            else if (line.StartsWith("dir "))
            {
                // nothing
            }
            else
            {
                var tmp = line.Split(' ');
                dir.Peek().Size += int.Parse(tmp[0]);
            }
        }

        while (dir.Count > 0)
        {
            var tmp = dir.Pop();

            if (dir.Count > 0)
                dir.Peek().Size += tmp.Size;

            if (tmp.Size <= 100_000)
            {
                found.Add(tmp);
            }
        }

        return found.Sum(d => d.Size);

    }

    public record Directory(string Name)
    {
        public long Size { get; set; }
    }
}