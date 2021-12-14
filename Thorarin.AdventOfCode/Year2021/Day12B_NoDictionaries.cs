using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

/// <summary>
/// This solution is based on <see cref="Day12B_Optimized"/>, but also avoids the use of dictionaries
/// in the computationally expensive part of the algorithm, saving quite a bit of time.
/// </summary>
[Puzzle(Year = 2021, Day = 12, Part = 2)]
public class Day12B_NoDictionaries : Puzzle
{
    private Dictionary<string, List<string>> _adjacentDictionary;
    private int[][] _adjacent;
    private int _firstSmallCave = -1;
    private int _pathCount;

    public override Output SampleExpectedOutput => 3509;
    public override Output ProblemExpectedOutput => 149385;     

    public override void ParseInput(TextReader reader)
    {
        _adjacentDictionary = new();
        
        foreach (var line in reader.AsLines())
        {
            var split = line.Split('-');
            Add(split[0], split[1]);
        }
    }

    private void Add(string a, string b)
    {
        if (!_adjacentDictionary.TryGetValue(a, out var list))
        {
            list = new List<string>();
            _adjacentDictionary.Add(a, list);
        }
        if (!list.Contains(b)) list.Add(b);
        
        if (!_adjacentDictionary.TryGetValue(b, out list))
        {
            list = new List<string>();
            _adjacentDictionary.Add(b, list);
        }
        if (!list.Contains(a)) list.Add(a);
    }

    public override Output Run()
    {
        var keys = _adjacentDictionary.Keys.ToList();
        keys.Sort((a, b) => a[0].CompareTo(b[0]));

        _adjacent = new int[keys.Count][];
        for (int k = 0; k < keys.Count; k++)
        {
            _adjacent[k] = _adjacentDictionary[keys[k]].Select(x => keys.IndexOf(x)).ToArray();
            if (_firstSmallCave == -1 && !IsBigCave(keys[k]))
                _firstSmallCave = k;
        }

        int start = keys.IndexOf("start");
        int end = keys.IndexOf("end");
        
        DepthFirstTraversal(start, end);
        return _pathCount;
    }

    private void DepthFirstTraversal(int start, int end)
    {
        var visited = new int[_adjacent.Length]; 
        List<int> pathList = new() { start };
        bool smallCave = false;
        Recurse(start, start, end, visited, pathList, ref smallCave);
    }
    
    private void Recurse(int pos, int start, int end, int[] visited, List<int> pathList, ref bool smallCave)
    {
        if (pos == end)
        {
            _pathCount++;
            return;
        }

        int visit = ++visited[pos];
        bool isDuplicateVisit = visit == 2 && !IsBigCave(pos);
        if (isDuplicateVisit) smallCave = true;

        foreach (int adjacent in _adjacent[pos])
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

        bool CanVisit(int cave, bool visitedSmallCave)
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

    private bool IsBigCave(int cave)
    {
        return cave < _firstSmallCave;
    }
}