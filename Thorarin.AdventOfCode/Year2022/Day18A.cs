using System.Diagnostics;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 18, Part = 1)]
public class Day18A : Puzzle
{
    private bool[,,] _voxels;

    public override void ParseInput(TextReader reader)
    {
        var voxels = reader.AsLines().Select(line => line.Split(',').Select(int.Parse).ToArray()).ToList();

        var maxX = voxels.Select(v => v[0]).Max() + 3;
        var maxY = voxels.Select(v => v[1]).Max() + 3;
        var maxZ = voxels.Select(v => v[2]).Max() + 3;

        _voxels = new bool[maxX, maxY, maxZ];

        int count = 0;
        foreach (var voxel in voxels)
        {
            _voxels[voxel[0] + 1, voxel[1] + 1, voxel[2] + 1] = true;
            count++;
        }
    }

    public override Output SampleExpectedOutput => 64;

    public override Output ProblemExpectedOutput => 3412;

    public override Output Run()
    {
        int sides = 0;
        int count = 0;

        try
        {
            for (int x = 1; x < _voxels.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < _voxels.GetLength(1) - 1; y++)
                {
                    for (int z = 1; z < _voxels.GetLength(2) - 1; z++)
                    {
                        if (_voxels[x, y, z])
                        {
                            count++;
                            if (!_voxels[x - 1, y, z]) sides++;
                            if (!_voxels[x + 1, y, z]) sides++;
                            if (!_voxels[x, y - 1, z]) sides++;
                            if (!_voxels[x, y + 1, z]) sides++;
                            if (!_voxels[x, y, z - 1]) sides++;
                            if (!_voxels[x, y, z + 1]) sides++;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }


        return sides;
    }

}