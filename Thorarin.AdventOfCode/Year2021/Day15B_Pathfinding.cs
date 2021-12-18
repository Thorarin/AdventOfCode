using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Pathfinding;

namespace Thorarin.AdventOfCode.Year2021;

public abstract class Day15B_Pathfinding : Day15Base
{
    private int _width;
    private int _height;
    private int[,] _grid;
    private Pos[,] _posGrid;
    
    public override void ParseInput(TextReader reader)
    {
        var lines = reader.ToLineArray();

        int repeats = 5;
        int templateWidth = lines[0].Length;
        int templateHeight = lines.Length;
        var grid = new int[templateWidth * repeats, templateHeight * repeats];

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines.Length; x++)
            {
                grid[x, y] = int.Parse(lines[y].Substring(x, 1));
            }
        }

        var width = templateWidth * repeats;
        var height = templateHeight * repeats;
        
        // Use template to fill top row of 5 * 5 tiles
        for (int y = 0; y < templateHeight; y++)
        {
            for (int x = templateWidth; x < width; x++)
            {
                grid[x, y] = WrapRisk(grid[x - templateWidth, y]);
            }
        }
        
        // Then fill vertically
        for (int y = templateHeight; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = WrapRisk(grid[x, y - templateHeight]);
            }
        }

        int WrapRisk(int risk)
        {
            return (risk % 9) + 1;
        }

        Grid = grid;
    }    
    
    public override Output Run()
    {
        _width = Grid.GetLength(0);
        _height = Grid.GetLength(1);
        _grid = Grid;

        _posGrid = new Pos[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _posGrid[x, y] = new Pos(x, y);
            }
        }

        return FindPath(_posGrid[0, 0], _posGrid[_width - 1, _height - 1]);
    }

    protected abstract int FindPath(Pos start, Pos end);
    
    protected IEnumerable<(Pos, int)> AdjacentNodes(Pos pos)
    {
        if (pos.X > 0) yield return (_posGrid[pos.X - 1, pos.Y], _grid[pos.X - 1, pos.Y]);
        if (pos.Y > 0) yield return (_posGrid[pos.X, pos.Y - 1], _grid[pos.X, pos.Y - 1]);
        if (pos.X < _width - 1) yield return (_posGrid[pos.X + 1, pos.Y], _grid[pos.X + 1, pos.Y]);
        if (pos.Y < _height - 1) yield return (_posGrid[pos.X, pos.Y + 1], _grid[pos.X, pos.Y + 1]);
    }

    protected readonly record struct Pos(int X, int Y)
    {
        public override int GetHashCode()
        {
            return X << 16 | Y;
        }
    }
}