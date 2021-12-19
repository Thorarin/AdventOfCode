using System.Diagnostics;
using System.Text.RegularExpressions;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Year2021.Day19;

namespace Thorarin.AdventOfCode.Year2021;

public abstract class Day19Base : Puzzle
{
    protected List<Scanner> _data;
    protected List<Point?> _scannerLoc;
    protected List<Point> _beacons;
    protected int _start = 0;
    
    public override Output SampleExpectedOutput => 79;
    public override Output ProblemExpectedOutput => 359;

    public override void ParseInput(TextReader reader)
    {
        _data = new List<Scanner>();
        
        var scannerRegex = new Regex("--- scanner (?<scanner>\\d+) ---");
        var beaconRegex = new Regex("(?<x>-?\\d+),(?<y>-?\\d+),(?<z>-?\\d+)");
        
        string? line;
        int scanner = 0;
        
        while ((line = reader.ReadLine()) != null)
        {
            var sm = scannerRegex.Match(line);
            scanner = int.Parse(sm.Groups["scanner"].Value);

            var scannerData = new List<Point>();
            while ((line = reader.ReadLine()) != null)
            {
                if (line == "") break;
                var bm = beaconRegex.Match(line);
                scannerData.Add(new Point(int.Parse(bm.Groups["x"].Value), int.Parse(bm.Groups["y"].Value), int.Parse(bm.Groups["z"].Value)));
            }
            
            _data.Add(new Scanner(scannerData));
        }

        _start = 0;
        _scannerLoc = Enumerable.Range(0, scanner + 1).Select(x => (Point?)null).ToList();
        _scannerLoc[_start] = new Point(0, 0, 0);
        _beacons = new List<Point>();
        _beacons.AddRange(_data[_start].Beacons);
    }

    protected void LocateScanners(int scanner)
    {
        if (!_scannerLoc[scanner].HasValue)
            throw new Exception($"Cannot Find without location {scanner}");

        for (int s = 0; s < _data.Count; s++)
        {
            if (s == scanner) continue;
            if (_scannerLoc[s].HasValue) continue;

            foreach (var rotatedPoints in _data[s].Beacons.Rotate())
            {
                var overlap = FindOverlapKnownBeacons(rotatedPoints);
                if (overlap.HasValue)
                {
                    var scannerLoc = overlap.Value;
                    Debug.WriteLine($"Scanner {s} is at {scannerLoc} relative to scanner {_start}");
                    Console.Write($"{s} ");

                    _scannerLoc[s] = scannerLoc;
                    _beacons = _beacons.Concat(rotatedPoints.Select(p => p + scannerLoc)).Distinct().ToList();
                    
                    LocateScanners(s);
                    if (_scannerLoc[s].HasValue) break;
                }
            }
        }
    }

    private Point? FindOverlapKnownBeacons(IReadOnlyList<Point> rotatedBeaconPoints)
    {
        foreach (var pivot in rotatedBeaconPoints)
        {
            foreach (var knownBeacon in _beacons)
            {
                var offset = knownBeacon - pivot;
                var intersects = rotatedBeaconPoints.Select(p => p + offset).Intersect(_beacons).Count();
                
                if (intersects >= 12)
                {
                    Debug.WriteLine($"Found {intersects} intersections of {rotatedBeaconPoints.Count} points");
                    return offset;
                }
            }
        }
        
        return null;
    }

    protected class Scanner
    {
        public Scanner(List<Point> points)
        {
            Beacons = points;
        }
        
        public List<Point> Beacons { get; }
    }
}