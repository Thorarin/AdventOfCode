namespace Thorarin.AdventOfCode.Pathfinding;

public interface IPoint
{
    int Dimensions { get; }

    int GetValue(int dimension);

    IEnumerable<IPoint> GetNeighbors();
}