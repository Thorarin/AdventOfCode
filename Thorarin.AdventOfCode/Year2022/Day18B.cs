using Thorarin.AdventOfCode.Extensions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 18, Part = 2)]
public class Day18B : Puzzle
{
    private const int Margin = 2;
    private bool[,,] _lava = new bool[0, 0, 0];
    
    public override void ParseInput(TextReader reader)
    {
        int maxX = 0;
        int maxY = 0;
        int maxZ = 0;

        var voxels = new List<(int X, int Y, int Z)>();

        // 40% faster version of the parsing that avoids LINQ stuff.
        // Since it was taking longer than the actual running of the algorithm, why not.
        foreach (var line in reader.AsLines())
        {
            var split = line.AsSpan().EnumerateSplit(',');
            split.MoveNext();
            var x = int.Parse(split.Current);
            split.MoveNext();
            var y = int.Parse(split.Current);
            split.MoveNext();
            var z = int.Parse(split.Current);

            voxels.Add((x, y, z));

            maxX = Math.Max(maxX, x + 2 * Margin + 1);
            maxY = Math.Max(maxY, y + 2 * Margin + 1);
            maxZ = Math.Max(maxZ, z + 2 * Margin + 1);
        }

        _lava = new bool[maxX, maxY, maxZ];

        foreach (var voxel in voxels)
        {
            _lava[voxel.X + Margin, voxel.Y + Margin, voxel.Z + Margin] = true;
        }
    }

    public override Output SampleExpectedOutput =>58;

    public override Output ProblemExpectedOutput => 2018;

    public override Output Run()
    {
        FillPockets(_lava, Margin);

        return MeasureSurface(_lava, Margin);
    }

    private void FillPockets(bool[,,] voxels, int margin)
    {
        var water = Flood(voxels);

        for (int x = margin; x < _lava.GetLength(0) - margin; x++)
        {
            for (int y = margin; y < _lava.GetLength(1) - margin; y++)
            {
                for (int z = margin; z < _lava.GetLength(2) - margin; z++)
                {
                    if (!voxels[x, y, z] && !water[x, y, z])
                    {
                        voxels[x, y, z] = true;
                    }
                }
            }
        }
    }

    private bool[,,] Flood(bool[,,] voxels)
    {
        var water = new bool[voxels.GetLength(0), voxels.GetLength(1), voxels.GetLength(2)];

        water[1, 1, 0] = true;
        water[0, 1, 1] = true;
        water[1, 0, 1] = true;

        for (int i = 0; i < 3; i++)
        {
            for (int x = 1; x < voxels.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < voxels.GetLength(1) - 1; y++)
                {
                    for (int z = 1; z < voxels.GetLength(2) - 1; z++)
                    {
                        if (water[x, y, z] || _lava[x, y, z]) continue;

                        if (water[x - 1, y, z] || water[x + 1, y, z] || water[x, y - 1, z] ||
                            water[x, y + 1, z] || water[x, y, z - 1] || water[x, y, z + 1])
                        {
                            water[x, y, z] = true;
                        }
                    }
                }
            }
        }

        return water;
    }

    private int MeasureSurface(bool[,,] voxels, int margin)
    {
        int sides = 0;

        for (int x = margin; x < voxels.GetLength(0) - margin; x++)
        {
            for (int y = margin; y < voxels.GetLength(1) - margin; y++)
            {
                for (int z = margin; z < voxels.GetLength(2) - margin; z++)
                {
                    if (_lava[x, y, z])
                    {
                        if (!voxels[x - 1, y, z]) sides++;
                        if (!voxels[x + 1, y, z]) sides++;
                        if (!voxels[x, y - 1, z]) sides++;
                        if (!voxels[x, y + 1, z]) sides++;
                        if (!voxels[x, y, z - 1]) sides++;
                        if (!voxels[x, y, z + 1]) sides++;
                    }
                }
            }
        }

        return sides;
    }

}