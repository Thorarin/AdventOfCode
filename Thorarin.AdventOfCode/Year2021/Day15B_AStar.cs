using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Pathfinding;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 15, Part = 2)]
public class Day15B_AStar : Day15B_Pathfinding
{
    private int _width;
    private int _height;
    private int[,] _grid;
    private Pos[,] _posGrid;
    
    protected override int FindPath(Pos start, Pos end)
    {
        var aStar = new AStar<Pos>(AdjacentNodes);
        aStar.TryPath(start, end, Heuristic, out var result);

        return result.Cost;
        
        int Heuristic(Pos pos)
        {
            return Math.Abs(end.X - pos.X) + Math.Abs(end.Y - pos.X);
        }        
    }
}