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
    private readonly string _apiKey;
    private List<Dot> _dots;
    private Func<Dot, Dot> _fold;

    public override Output SampleExpectedOutput => new Answer("(OCR failed)", 16);
    public override Output ProblemExpectedOutput => new Answer("CJHAZHKU", 97);

    public Day13B_OCR(SecretStore secrets)
    {
        _apiKey = secrets.GetSecret("OcrApiKey");
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
        int mag = 2;
        Bitmap bitmap = new Bitmap(width * mag + 20, height * mag + 20);
        Graphics graphics = Graphics.FromImage(bitmap);
        graphics.FillRectangle(Brushes.White, 0, 0, (width * mag) + 20, (height * mag) + 20);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y])
                {
                    graphics.FillRectangle(Brushes.Black, x * mag + 10, y * mag + 10, mag, mag);
                    
                    if (y < height - 1 && x < width - 1)
                    {
                        if (grid[x + 1, y + 1] && !grid[x + 1, y] && !grid[x, y +1])
                        {
                            bitmap.SetPixel(x * mag + 12, y * mag + 11, Color.Black);
                            bitmap.SetPixel(x * mag + 11, y * mag + 12, Color.Black);
                        }
                    }
                    
                    if (y > 0 && x < width - 1)
                    {
                        if (grid[x + 1, y - 1] && !grid[x + 1, y] && !grid[x, y - 1])
                        {
                            bitmap.SetPixel(x * mag + 12, y * mag + 10, Color.Black);
                            bitmap.SetPixel(x * mag + 11, y * mag + 9, Color.Black);
                        }
                    }                    
                    
                }

            }
        }

        string code;
        
        using (var memoryStream = new MemoryStream())
        {
            bitmap.Save(memoryStream, ImageFormat.Png);
            memoryStream.Seek(0, SeekOrigin.Begin);
            code = await OcrPngImage(memoryStream.GetBuffer());
        }

        return new Answer(code, foldedDots.Count);
    }

    private async Task<string> OcrPngImage(byte[] image)
    {
        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("apikey", _apiKey), 
            new KeyValuePair<string, string>("base64Image", "data:image/png;base64," + Convert.ToBase64String(image)),
            new KeyValuePair<string, string>("filetype", "PNG"),
            new KeyValuePair<string, string>("OCREngine", "2")
        });
            
        var client = new HttpClient();

        var response = await client.PostAsync("https://api.ocr.space/parse/image", formContent);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<OcrResult>();
        if (content.ParsedResults.Count > 0)
        {
            return content.ParsedResults[0].ParsedText;
        }

        return "(OCR failed)";
    }
  
    record Dot(int X, int Y);

    record Answer(string RecognizedCode, long Value) : LongOutput(Value);
}