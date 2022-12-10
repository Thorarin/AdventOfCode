using Thorarin.AdventOfCode.Framework;
using Thorarin.AdventOfCode.Ocr;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 10, Part = 2)]
public class Day10B : IPuzzle
{
    private readonly IOcrService _ocrService;
    private List<string> _lines;
    private readonly bool[] _screen = new bool[240];

    public Day10B(IOcrService ocrService)
    {
        _ocrService = ocrService;
    }

    public void ParseInput(TextReader reader)
    {
        _lines = reader.AsLines().ToList();
    }

    public Output SampleExpectedOutput => "";

    public Output ProblemExpectedOutput => "EGLHBLFJ";

    public async Task<Output> Run()
    {
        int x = 1;
        int cycle = 0;

        foreach (var line in _lines)
        {
            switch (line.Substring(0, 4))
            {
                case "addx":
                {
                    int increment = int.Parse(line.Substring(5));

                    cycle++;
                    UpdateScreen(cycle, x);
                    // Nothing happens first cycle

                    cycle++;
                    UpdateScreen(cycle, x);
                    x += increment;
                    break;
                }
                case "noop":
                    // Do nothing
                    cycle++;
                    UpdateScreen(cycle, x);
                    break;
            }
        }

        // DumpScreen();

        return await _ocrService.OcrBitmap(_screen, 40, 6);
    }

    private void UpdateScreen(int cycle, int x)
    {
        var (screenY, screenX) = Math.DivRem(cycle - 1, 40);

        if (screenX >= x - 1 && screenX <= x + 1)
        {
            _screen[cycle - 1] = true;
        }
    }

    private void DumpScreen()
    {
        Console.WriteLine();
        for (int v = 0; v < 6; v++)
        {
            for (int h = 0; h < 40; h++)
            {
                Console.Write(_screen[v * 40 + h] ? "#" : ".");
            }
            Console.WriteLine();
        }
    }
}