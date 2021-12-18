namespace Thorarin.AdventOfCode.Pathfinding;
public class Dijkstra<T> where T: struct
{
    private readonly AStar<T> astar;
    public Dijkstra(Func<T, IEnumerable<(T, int)>> getAdjacentFunc)
    {
        astar = new AStar<T>(getAdjacentFunc);
    }

    public Func<T, IEnumerable<(T, int)>> GetAdjacentFunc => astar.GetAdjacentFunc;

    public bool IncludeStartInPath
    {
        get => astar.IncludeStartInPath;
        set => astar.IncludeStartInPath = value;
    }

    public bool TryPath(T start, T end, out DijkstraResult<T> result) => TryPath(start, n => n.Equals(end), out result);

    public bool TryPath(T start, Func<T, bool> goalReachedPredicate, out DijkstraResult<T> result)
    {
        bool success = astar.TryPath(start, goalReachedPredicate, _ => 0, out AStarResult<T> astarResult);
        result = GetResult(astarResult);
        return success;
    }

    private DijkstraResult<T> GetResult(AStarResult<T> result)
        => new DijkstraResult<T>(result.Path, result.Cost);
}

public record DijkstraResult<T>(T[] Path, int Cost);