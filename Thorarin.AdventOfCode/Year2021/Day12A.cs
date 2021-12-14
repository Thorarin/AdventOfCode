using MoreLinq;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 12, Part = 1)]
public class Day12A : Puzzle
{
    private Dictionary<string, List<string>> _adjacent;
    List<string> _paths = new();

    public override Output SampleExpectedOutput => 226;
    public override Output ProblemExpectedOutput => 5254;

    public override void ParseInput(TextReader reader)
    {
        _adjacent = new();
        
        foreach (var line in reader.AsLines())
        {
            var split = line.Split('-');
            Add(split[0], split[1]);
        }
    }

    private void Add(string a, string b)
    {
        if (!_adjacent.TryGetValue(a, out var list))
        {
            list = new List<string>();
            _adjacent.Add(a, list);
        }
        list.Add(b);
        
        if (!_adjacent.TryGetValue(b, out list))
        {
            list = new List<string>();
            _adjacent.Add(b, list);
        }
        list.Add(a);
    }

    public override Output Run()
    {
        DepthFirstTraversal("start", "end");
        return _paths.Count;
    }

    private void DepthFirstTraversal(string start, string end)
    {
        HashSet<string> visited = new();
        List<string> pathList = new() { start };
        Recurse(start, end, visited, pathList);
    }

    private void Recurse(string pos, string end, ISet<string> visited, List<string> pathList)
    {
        if (pos == end)
        {
            _paths.Add(string.Join(',', pathList));
            return;
        }

        bool big = IsBigCave(pos);
        if (!big)
        {
            visited.Add(pos);
        }

        foreach (string adjacent in _adjacent[pos])
        {
            if (!visited.Contains(adjacent))
            {
                pathList.Add(adjacent);
                Recurse(adjacent, end, visited, pathList);
                pathList.RemoveAt(pathList.Count - 1);
            }
        }

        visited.Remove(pos);
    }

    private static bool IsBigCave(string cave)
    {
        return char.IsUpper(cave[0]);
    }
}