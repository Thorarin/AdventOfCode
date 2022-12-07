using MoreLinq;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 7, Part = 2)]
public class Day07B : Puzzle
{
    private string[] _data;

    public override void ParseInput(TextReader reader)
    {
        _data = reader.ToLineArray();
    }

    public override Output SampleExpectedOutput => 24933642;

    public override Output ProblemExpectedOutput => 578710;

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

                    found.Add(tmp);
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
            {
                dir.Peek().Size += tmp.Size;
            }

            found.Add(tmp);
        }

        int spaceAvailable = 70_000_000 - found.Single(d => d.Name == "/").Size;
        int spaceToBeFreed = 30_000_000 - spaceAvailable;

        var a = found.Where(d => d.Size >= spaceToBeFreed).ToList();

        return found.Where(d => d.Size >= spaceToBeFreed).OrderBy(d => d.Size).First().Size;
    }


    public record Directory(string Name)
    {
        public int Size { get; set; }
    }
}