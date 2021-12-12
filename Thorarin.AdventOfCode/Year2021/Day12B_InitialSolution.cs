using MoreLinq;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

/// <summary>
/// This was the first solution that came to my mind after finishing part 1.
/// The goal was simply to get the right answer with as few changes as possible.
/// </summary>
[Puzzle(Year = 2021, Day = 12, Part = 2)]
public class Day12B_InitialSolution : Puzzle
{
    private Dictionary<string, List<string>> _adjacent;
    List<string> _paths = new();

    public override Output SampleExpectedOutput => 3509;
    public override Output ProblemExpectedOutput => 149385;     

    public override void ParseInput(string[] fileLines)
    {
        _paths = new();
        _adjacent = new();
        
        foreach (var line in fileLines)
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
        if (!list.Contains(b)) list.Add(b);
        
        if (!_adjacent.TryGetValue(b, out list))
        {
            list = new List<string>();
            _adjacent.Add(b, list);
        }
        if (!list.Contains(a)) list.Add(a);
    }

    public override Output Run()
    {
        var smallCaves = _adjacent.Keys.Where(x => !IsBigCave(x) && x != "start").ToList();
        
        foreach (var smallCave in smallCaves)
        {
            DepthFirstTraversal("start", "end", smallCave);
            _paths.Sort();
            _paths = _paths.Distinct().ToList();
        }

        return _paths.Count;
    }

    private void DepthFirstTraversal(string start, string end, string smallCave)
    {
        Dictionary<string, int> visited = _adjacent.Keys.ToDictionary(x => x, x => 0);
        List<string> pathList = new() { start };
        Recurse(start, end, visited, pathList, smallCave);
    }
    
    private void Recurse(string pos, string end, Dictionary<string, int> visited, List<string> pathList, string smallCave)
    {
        if (pos == end)
        {
            _paths.Add(string.Join(',', pathList));
            return;
        }

        visited[pos]++;

        foreach (string adjacent in _adjacent[pos])
        {
            if (CanVisit(adjacent))
            {
                pathList.Add(adjacent);
                Recurse(adjacent, end, visited, pathList, smallCave);
                pathList.RemoveAt(pathList.Count - 1);
            }
        }

        visited[pos]--;

        bool CanVisit(string cave)
        {
            if (IsBigCave(cave)) return true;
            if (cave == smallCave && visited[cave] <= 1) return true;
            return visited[cave] == 0;
        }
    }
    
    private static bool IsBigCave(string cave)
    {
        return char.IsUpper(cave[0]);
    }      
}