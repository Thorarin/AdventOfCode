using Thorarin.AdventOfCode.Extensions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

public abstract class Day15Base : Puzzle
{
    private int _width;
    private int _height;
    private int[,] _grid;
    private int[,] _minCost;

    protected int[,] Grid
    {
        get => _grid;
        set
        {
            _grid = value;
            _width = _grid.GetLength(0);
            _height = _grid.GetLength(1);
        }
    }
    
    public override Output Run()
    {
        _minCost = new int[_width, _height];
        _minCost.Fill(int.MaxValue);

        // Initialize top row with more sensible values than int.MaxValue
        _minCost[0, 0] = 0;
        for (int x = 1; x < _width; x++)
        {
            _minCost[x, 0] = _minCost[x - 1, 0] + _grid[x, 0];
        }
        
        bool change = SweepDown();

        SweepDiagonal();

        while (change)
        {
            change = SweepRight();
            change |= SweepDown();
            change |= SweepDiagonal();
            change |= SweepUp();
            change |= SweepLeft();
            
            //Console.WriteLine("*");
            
            // if (!change)
            // {
            //     //change = SweepDiagonal();
            //     change |= SweepRight();
            //     change |= SweepDown();
            // }

        }
        
        return _minCost[_width - 1, _height - 1];
    }

    private bool SweepDown()
    {
        return Sweep(0, 1, 1, 1, 0, -1);
    }

    private bool SweepRight()
    {
        return Sweep(1, 0, 1, 1, -1, 0);
    }

    private bool SweepUp()
    {
        return Sweep(0, _height - 2, 1, -1, 0, 1);
    }
    
    private bool SweepLeft()
    {
        return Sweep(_width - 2, 0, -1, 1, 1, 0);
    }
    
    private bool Sweep(int startX, int startY, int incrementX, int incrementY, int deltaX, int deltaY)
    {
        bool change = false;
        for (int x = startX; x >= 0 && x < _width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < _height; y += incrementY)
            {
                int candidateCost = _minCost[x + deltaX, y + deltaY] + _grid[x, y];
                if (candidateCost < _minCost[x, y])
                {
                    _minCost[x, y] = candidateCost;
                    change = true;
                }
            }
        }

        return change;
    }

    private bool SweepDiagonal()
    {
        bool changes = false;
        for (int d = 1; d < _width; d++)
        {
            for (int x = 0; x <= d; x++)
            {
                int y = d - x;

                int? c = null;
                if (x > 1)
                {
                    c = _minCost[x - 1, y] + _grid[x, y];
                }

                // if (x < _width - 1)
                // {
                //     c = Math.Min(c ?? int.MaxValue, _minCost[x + 1, y] + _grid[x, y]);
                // }
                
                if (y > 0)
                {
                    c = Math.Min(c ?? int.MaxValue, _minCost[x, y - 1] + _grid[x, y]);
                }

                // if (y < _height - 1)
                // {
                //     c = Math.Min(c ?? int.MaxValue, _minCost[x, y + 1] + _grid[x, y]);
                // }
                
                if (c.HasValue && c.Value < _minCost[x, y])
                {
                    _minCost[x, y] = c.Value;
                    changes = true;
                }
            }
        }
        
        return changes;
    }
    
    
}