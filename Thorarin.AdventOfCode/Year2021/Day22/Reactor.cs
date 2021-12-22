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
        List<Cuboid> intersects = new();
        for (var i = Cuboids.Count - 1; i >= 0; i--)
        {
            var existing = Cuboids[i];
            if (!existing.Intersects(cuboid)) continue;
            if (cuboid.IsFullyInside(existing)) return;
            if (existing.IsFullyInside(cuboid))
            {
                // Remove existing cuboids fully inside this new cuboid,
                // because they will be redundant after adding this one.                
                Cuboids.RemoveAt(i);
            }
            else
            {
                intersects.Add(existing);
            }
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
        for (var i = Cuboids.Count - 1; i >= 0; i--)
        {
            var existing = Cuboids[i];
            if (existing.Intersects(cuboid))
            {
                Cuboids.RemoveAt(i);
                Cuboids.AddRange(existing.Remove(cuboid));
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