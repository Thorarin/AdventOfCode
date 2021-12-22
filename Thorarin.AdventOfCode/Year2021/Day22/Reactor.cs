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
        var intersects = Cuboids.Where(cuboid.Intersects).ToList();
        
        if (intersects.Any(cuboid.IsFullyInside)) return;
        
        // Remove existing cuboids fully inside this new cuboid,
        // because they will be redundant after adding this one.
        foreach (var redundant in intersects.Where(x => x.IsFullyInside(cuboid)).ToList())
        {
            Cuboids.Remove(redundant);
            intersects.Remove(redundant);
        }
        
        if (intersects.Count > 0)
        {
            AddDisjoint(cuboid, intersects);
            return;
        }
        
        Cuboids.Add(cuboid);        
    }

    private void AddDisjoint(Cuboid cuboid, List<Cuboid> oldIntersects)
    {
        var pick = oldIntersects[^1];
        oldIntersects.RemoveAt(oldIntersects.Count - 1);
        AddDisjoint(cuboid.Remove(pick), oldIntersects);
    }
    
    private void AddDisjoint(IEnumerable<Cuboid> cuboids, List<Cuboid> intersects)
    {
        foreach (var cuboid in cuboids)
        {
            var newIntersects = intersects.Where(cuboid.Intersects).ToList();
            if (newIntersects.Any(x => cuboid.IsFullyInside(x))) continue;

            if (newIntersects.Count == 0)
            {
                Cuboids.Add(cuboid);
            }
            else
            {
                AddDisjoint(cuboid, newIntersects);
            }
        }
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