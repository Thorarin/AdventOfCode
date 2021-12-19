using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 19, Part = 2)]
public class Day19B : Day19Base
{
    public override Output SampleExpectedOutput => 3621;
    public override Output ProblemExpectedOutput => 12292;

    public override Output Run()
    {
        LocateScanners(_start);

        int scannersLocated = _scannerLoc.Count(l => l.HasValue);
        if (scannersLocated < _data.Count)
        {
            Console.WriteLine($"Not all scanners located: {scannersLocated} / {_data.Count}");
        }

        #if DEBUG
        var dumpPoints = _beacons.OrderBy(x => x.X).Select(x => x.ToString().Substring(1).Replace(")", "")).ToList();
        Console.WriteLine(string.Join("\r\n", dumpPoints));
        #endif

        int maxDistance = 0;
        foreach (var a in _scannerLoc)
        {
            foreach (var b in _scannerLoc)
            {
                maxDistance = Math.Max(maxDistance, a.Value.ManhattanDistance(b.Value));
            }
        }
        
        return maxDistance;
    }
}