namespace Thorarin.AdventOfCode.Framework;

public static class TextReaderExtensions
{
    public static string[] ToLineArray(this TextReader reader)
    {
        string? line;
        List<string> lines = new();

        while ((line = reader.ReadLine()) != null)
        {
            lines.Add(line);
        }

        return lines.ToArray();
    }

    public static IEnumerable<string> AsLines(this TextReader reader)
    {
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }

    public static IEnumerable<string> UntilNextEmptyLine(this TextReader reader)
    {
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.Length == 0) yield break;
            yield return line;
        }
    }
}