namespace Thorarin.AdventOfCode.Ocr;

public interface IOcrService
{
    Task<string> OcrBitmap(IEnumerable<bool> pixels, int width, int height);
    Task<string> OcrBitmap(bool[,] grid);
}