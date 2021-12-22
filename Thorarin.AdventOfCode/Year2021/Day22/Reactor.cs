namespace Thorarin.AdventOfCode.Year2021.Day22;

internal class Reactor
{
    public Reactor()
    {
        Cuboids = new List<Cuboid>();
    }
    
    public List<Cuboid> Cuboids { get; }

    public long CubesOn => Cuboids.Sum(c => c.Volume);

    public void TurnOn(Cuboid cuboid)
    {
        if (Cuboids.Any(cuboid.IsFullyInside)) return;

        // Remove existing cuboids fully inside this new cuboid,
        // because they will be redundant after adding this one.
        for (var i = Cuboids.Count - 1; i >= 0; i--)
        {
            if (Cuboids[i].IsFullyInside(cuboid))
            {
                Cuboids.RemoveAt(i);
            }
        }

        foreach (var existing in Cuboids)
        {
            if (existing.Intersects(cuboid))
            {
                foreach (var splitCube in cuboid.Remove(existing))
                {
                    TurnOn(splitCube);
                }
                return;
            }
        }
        
        Cuboids.Add(cuboid);        
    }

    public void TurnOff(Cuboid cuboid)
    {
        // Exit if no cubes are turned on in the cuboid area
        if (!Cuboids.Any(cuboid.Intersects)) return;

        // Temporarily remove all cuboids that intersect with
        // the cuboid we're trying to turn off.
        List<Cuboid> overlappingCuboids = new List<Cuboid>();
        for (var i = Cuboids.Count - 1; i >= 0; i--)
        {
            var existing = Cuboids[i];
            if (existing.Intersects(cuboid))
            {
                Cuboids.RemoveAt(i);
                overlappingCuboids.Add(existing);
            }
        }

        foreach (var overlappingCuboid in overlappingCuboids)
        {
            // Split the overlapping cuboid into smaller ones,
            // omitting the intersection with the cuboid we're turning off.
            // Re-add the remaining cuboids.
            foreach (var splitCubes in overlappingCuboid.Remove(cuboid))
            {
                TurnOn(splitCubes);
            }
        }
    }

    public long GetCubesOnInCuboid(Cuboid cuboid)
    {
        var tempReactor = new Reactor();
        tempReactor.TurnOn(cuboid);

        long total = tempReactor.CubesOn;

        foreach (var remove in Cuboids)
        {
            tempReactor.TurnOff(remove);
        }

        return total - tempReactor.CubesOn;
    }
}