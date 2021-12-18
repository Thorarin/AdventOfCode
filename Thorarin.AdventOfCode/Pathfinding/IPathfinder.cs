namespace Thorarin.AdventOfCode.Pathfinding;

public interface IPathFinder<T>
{
    public bool TryPath(T start, T end, out T[] path);

    public bool IncludeStart { get; set; }
}