using System.Drawing;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Ocr;
using Color = System.Drawing.Color;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace Thorarin.AdventOfCode.Year2021;

[Puzzle(Year = 2021, Day = 13, Part = 2)]
public class Day13B_OCR : Puzzle
{
    private readonly OcrSpaceOcrService _ocrSpaceOcrService;
    private List<Dot> _dots;
    private Func<Dot, Dot> _fold;

    public override Output SampleExpectedOutput => new Answer("(OCR failed)", 16);
    public override Output ProblemExpectedOutput => new Answer("CJHAZHKU", 97);

    public Day13B_OCR(OcrSpaceOcrService ocrSpaceOcrService)
    {
        _ocrSpaceOcrService = ocrSpaceOcrService;
    }
    
    public override Output Run()
    {
        return Run2().GetAwaiter().GetResult();
    }

    public override void ParseInput(TextReader reader)
    {
        _dots = new List<Dot>();
        _fold = x => x;

        foreach (string line in reader.UntilNextEmptyLine())
        {
            var split = line.Split(',');
            _dots.Add(new Dot(int.Parse(split[0]), int.Parse(split[1])));
        }

        var regex = new Regex("fold along (?<fold>[xy])=(?<value>\\d+)");
        
        foreach (string line in reader.UntilNextEmptyLine())
        {
            var match = regex.Match(line);
            int value = int.Parse(match.Groups["value"].Value);

            var fold = _fold;
            switch (match.Groups["fold"].Value)
            {
                case "x":
                    _fold = dot => FoldX(fold(dot), value);
                    break;
                case "y":
                    _fold = dot => FoldY(fold(dot), value);
                    break;
            }
        }
    }

    private static Dot FoldX(Dot dot, int x)
    {
        if (dot.X < x) return dot;
        return dot with { X = -dot.X + x + x };
    }

    private static Dot FoldY(Dot dot, int y)
    {
        if (dot.Y < y) return dot;
        return dot with { Y = -dot.Y + y + y };
    }

    public async Task<Output> Run2()
    {
        var foldedDots = _dots.Select(_fold).Distinct().ToList();

        var width = foldedDots.Select(x => x.X).Max() + 1;
        var height = foldedDots.Select(x => x.Y).Max() + 1;

        bool[,] grid = new bool[width, height];

        foreach (var dot in foldedDots)
        {
            grid[dot.X, dot.Y] = true;
        }

        string code = await _ocrSpaceOcrService.OcrBitmap(grid);

        return new Answer(code, foldedDots.Count);
    }

    record Dot(int X, int Y);

    record Answer(string RecognizedCode, long Value) : LongOutput(Value);
}