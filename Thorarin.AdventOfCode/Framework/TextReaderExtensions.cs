using System.Text;

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

    public static string? ReadUntil(this TextReader reader, char until)
    {
        StringBuilder sb = new StringBuilder();
        while (true)
        {
            int ch = reader.Read();
            if (ch == -1) break;
            if (ch == until)
            {
                return sb.ToString();
            }
            sb.Append((char)ch);
        }
        if (sb.Length > 0)
        {
            return sb.ToString();
        }
        return null;        
    }
}