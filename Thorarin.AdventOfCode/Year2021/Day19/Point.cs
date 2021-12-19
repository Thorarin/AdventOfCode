namespace Thorarin.AdventOfCode.Year2021.Day19;

public readonly struct Point
{
    public Point(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Point(int[] coordinates)
    {
        if (coordinates.Length != 3) throw new Exception();

        X = coordinates[0];
        Y = coordinates[1];
        Z = coordinates[2];
    }
    
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    public static implicit operator Point((int x, int y, int z) point)
    {
        return new Point(point.x, point.y, point.z);
    }

    public int this[int dimension]
    {
        get
        {
            return dimension switch {
                0 => X,
                1 => Y,
                2 => Z,
                _ => throw new ArgumentOutOfRangeException(nameof(dimension), dimension, null)
            };
        }
    }

    public static Point operator -(Point a, Point b)
    {
        return new Point(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }
    
    public static Point operator +(Point a, Point b)
    {
        return new Point(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public int ManhattanDistance(Point other)
    {
        return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
    }

    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }

    public Point Variation(int variation)
    {
        var (up, rot) = Math.DivRem(variation, 4);
        return Up(up).Rot(rot);
    }
    
    private Point Up(int up)
    {
        return up switch
        {
            0 => this,
            1 => new Point(X, -Y, -Z),
            2 => new Point(X, -Z, Y),
            3 => new Point(-Y, -Z, X),
            4 => new Point(-X, -Z, -Y),
            5 => new Point(Y, -Z, -X),
        };
    }

    private Point Rot(int rot)
    {
        return rot switch
        {
            0 => this,
            1 => new Point(-Y, X, Z),
            2 => new Point(-X, -Y, Z),
            3 => new Point(Y, -X, Z)
        };
    }
}