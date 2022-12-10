namespace Thorarin.AdventOfCode.Ocr;

public class OcrResult
{
    public List<Ocr> ParsedResults { get; set; }
    public int OcrExitCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorDetails { get; set; }

}