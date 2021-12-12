using MoreLinq;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

/// <summary>
/// This solution avoids duplicate paths and thus doesn't need to waste time removing
/// duplicates. Because of this, it doesn't even keep track of the paths it found :P
/// </summary>
[Puzzle(Year = 2021, Day = 12, Part = 2)]
public class Day12B_Optimized : Puzzle
{
    private Dictionary<string, List<string>> _adjacent;
    private int _pathCount;

    public override Output SampleExpectedOutput => 3509;
    public override Output ProblemExpectedOutput => 149385;     

    public override void ParseInput(string[] fileLines)
    {
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
        DepthFirstTraversal("start", "end");
        return _pathCount;
    }

    private void DepthFirstTraversal(string start, string end)
    {
        Dictionary<string, int> visited = _adjacent.Keys.ToDictionary(x => x, x => 0);
        List<string> pathList = new() { start };
        bool smallCave = false;
        Recurse(start, start, end, visited, pathList, ref smallCave);
    }
    
    private void Recurse(string pos, string start, string end, Dictionary<string, int> visited, List<string> pathList, ref bool smallCave)
    {
        if (pos == end)
        {
            _pathCount++;
            return;
        }

        int visit = ++visited[pos];
        bool isDuplicateVisit = visit == 2 && !IsBigCave(pos);
        if (isDuplicateVisit) smallCave = true;

        foreach (string adjacent in _adjacent[pos])
        {
            if (CanVisit(adjacent, smallCave))
            {
                pathList.Add(adjacent);
                Recurse(adjacent, start, end, visited, pathList, ref smallCave);
                pathList.RemoveAt(pathList.Count - 1);
            }
        }

        visited[pos]--;
        if (isDuplicateVisit) smallCave = false;

        bool CanVisit(string cave, bool visitedSmallCave)
        {
            if (IsBigCave(cave)) return true;
            if (cave == start) return false;
            if (!visitedSmallCave && visited[cave] <= 1) return true;
            return visited[cave] == 0;
        }
    }
    
    private static bool IsBigCave(string cave)
    {
        return char.IsUpper(cave[0]);
    }      
}