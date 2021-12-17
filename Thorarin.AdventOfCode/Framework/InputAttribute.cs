namespace Thorarin.AdventOfCode.Framework;

public class InputAttribute : Attribute
{
    public string FileName { get; }

    public InputAttribute(string fileName)
    {
        FileName = fileName;
    }
}