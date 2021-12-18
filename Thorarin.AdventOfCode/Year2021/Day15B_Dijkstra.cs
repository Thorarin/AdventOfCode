using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Pathfinding;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 15, Part = 2)]
public class Day15B_Dijkstra : Day15B_Pathfinding
{
    private int _width;
    private int _height;
    private int[,] _grid;
    private Pos[,] _posGrid;
    
    protected override int FindPath(Pos start, Pos end)
    {
        var aStar = new Dijkstra<Pos>(AdjacentNodes);
        aStar.TryPath(start, goal => goal == end, out var result);

        return result.Cost;
    }
}