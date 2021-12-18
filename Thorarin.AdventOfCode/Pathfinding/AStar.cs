namespace Thorarin.AdventOfCode.Pathfinding;

public static class AStar
{
    public static Func<T, int> ManhattanDistance<T>(T target) where T : IPoint => new Func<T, int>(a =>
    {
        int value = 0;
        for (int d = 0; d < a.Dimensions; d++)
        {
            value += Math.Abs(a.GetValue(d) - target.GetValue(d));
        }

        return value;
    });
}


public class AStar<T> where T: struct
{
    public AStar(Func<T, IEnumerable<(T, int)>> getAdjacentFunc)
    {
        GetAdjacentFunc = getAdjacentFunc;
    }

    public Func<T, IEnumerable<(T, int)>> GetAdjacentFunc { get; }

    public bool IncludeStartInPath { get; set; }

    public bool TryPath(T start, T end, Func<T, int> heuristicFunc, out AStarResult<T> result) => TryPath(start, n => n.Equals(end), heuristicFunc, out result);
    
    public bool TryPath(T start, Func<T, bool> goalReachedPredicate, Func<T, int> heuristicFunc, out AStarResult<T> result)
    {
        var queue = new PriorityQueue<T, int>();
        var childToParent = new Dictionary<T, T>();
        var itemToCost = new Dictionary<T, int>();
        
        itemToCost.Add(start, 0);
        queue.Enqueue(start, 0);

        int visited = 0;
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            visited++;

            if (goalReachedPredicate(current))
            {
                result = GetResult(current, childToParent, itemToCost[current], visited);
                return true;
            }

            foreach (var (adjacent, cost) in GetAdjacentFunc(current))
            {
                int newCost = itemToCost[current] + cost;
                if (!itemToCost.TryGetValue(adjacent, out var lastCost))
                {
                    lastCost = int.MaxValue;
                }

                if (newCost < lastCost)
                {
                    itemToCost[adjacent] = newCost;
                    childToParent[adjacent] = current;
                    queue.Enqueue(adjacent, newCost + heuristicFunc(adjacent));
                }
            }
        }

        result = new AStarResult<T>(Array.Empty<T>(), 0, 0);
        return false;
    }

    private AStarResult<T> GetResult(T end, IDictionary<T, T> childToParent, int cost, int visited)
    {
        Stack<T> stack = new Stack<T>();
        T current = end;
        do
        {
            stack.Push(current);
        } while (childToParent.TryGetValue(current, out current));

        T[] path = IncludeStartInPath
            ? stack.ToArray()
            : stack.Skip(1).ToArray();

        return new AStarResult<T>(path, cost, visited);
    }
}