namespace Thorarin.AdventOfCode.Year2021.Day22;

public readonly struct Cuboid
{
    public Cuboid(int x1, int x2, int y1, int y2, int z1, int z2)
    {
        if (x1 > x2) (x1, x2) = (x2, x1);
        if (y1 > y2) (y1, y2) = (y2, y1);
        if (z1 > z2) (z1, z2) = (z2, z1);

        X1 = x1;
        X2 = x2;
        Y1 = y1;
        Y2 = y2;
        Z1 = z1;
        Z2 = z2;
    }

    public int X1 { get; }
    public int X2 { get; }
    public int Y1 { get; }
    public int Y2 { get; }
    public int Z1 { get; }
    public int Z2 { get; }

    public long Volume => (long)(X2 - X1 + 1) * (Y2 - Y1 + 1) * (Z2 - Z1 + 1);
    
    public bool IsFullyInside(Cuboid other)
    {
        return
            X1 >= other.X1 && X2 <= other.X2 &&
            Y1 >= other.Y1 && Y2 <= other.Y2 &&
            Z1 >= other.Z1 && Z2 <= other.Z2;
    }

    public bool Intersects(Cuboid other)
    {
        if (X2 < other.X1) return false;
        if (other.X2 < X1) return false;
        if (Z2 < other.Z1) return false;
        if (other.Z2 < Z1) return false;
        if (Y2 < other.Y1) return false;
        if (other.Y2 < Y1) return false;

        return true;
    }

    /// <summary>
    /// Take a cuboid bite out of this cuboid.
    /// Returns a bunch of new ones that together cover the needed volume.
    /// </summary>
    public IEnumerable<Cuboid> Remove(Cuboid other)
    {
        return Split(other).Where(c => !c.Intersects(other));
    }

    /// <summary>
    /// Split a cuboid in smaller ones, that together cover the same volume.
    /// </summary>    
    internal IEnumerable<Cuboid> Split(Cuboid other)
    {
        // Significantly less elegant than my original implementation,
        // but results in quite a few less cuboids, which helps performance.
        
        foreach (var splitX in SplitX(other))
        {
            if (!splitX.Intersects(other))
            {
                yield return splitX;
            }
            else
            {
                foreach (var splitY in splitX.SplitY(other))
                {
                    if (!splitY.Intersects(other))
                    {
                        yield return splitY;
                    }
                    else
                    {
                        foreach (var splitZ in splitY.SplitZ(other))
                        {
                            yield return splitZ;
                        }
                    }
                }
            }
        }
    }

    internal IEnumerable<Cuboid> SplitX(Cuboid other)
    {
        if (other.X1 > X1)
        {
            yield return new Cuboid(X1, other.X1 - 1, Y1, Y2, Z1, Z2);
        }

        yield return new Cuboid(Math.Max(X1, other.X1), Math.Min(X2, other.X2), Y1, Y2, Z1, Z2);

        if (other.X2 < X2)
        {
            yield return new Cuboid(other.X2 + 1, X2, Y1, Y2, Z1, Z2);
        }
    }
    
    internal IEnumerable<Cuboid> SplitY(Cuboid other)
    {
        if (other.Y1 > Y1)
        {
            yield return new Cuboid(X1, X2, Y1, other.Y1 - 1, Z1, Z2);
        }

        yield return new Cuboid(X1, X2, Math.Max(Y1, other.Y1), Math.Min(Y2, other.Y2), Z1, Z2);

        if (other.Y2 < Y2)
        {
            yield return new Cuboid(X1, X2, other.Y2 + 1, Y2, Z1, Z2);
        }
    }    
    
    internal IEnumerable<Cuboid> SplitZ(Cuboid other)
    {
        if (other.Z1 > Z1)
        {
            yield return new Cuboid(X1, X2, Y1, Y2, Z1, other.Z1 - 1);
        }

        yield return new Cuboid(X1, X2, Y1, Y2, Math.Max(Z1, other.Z1), Math.Min(Z2, other.Z2));

        if (other.Z2 < Z2)
        {
            yield return new Cuboid(X1, X2, Y1, Y2, other.Z2 + 1, Z2);
        }
    }
    
    public override string ToString()
    {
        return $"({X1}-{X2}, {Y1}-{Y2}, {Z1}-{Z2})";
    }
}