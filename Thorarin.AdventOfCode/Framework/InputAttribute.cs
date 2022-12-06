using JetBrains.Annotations;

namespace Thorarin.AdventOfCode.Framework;

[MeansImplicitUse]
public class InputAttribute : Attribute
{
    public string FileName { get; }

    public InputAttribute(string fileName)
    {
        FileName = fileName;
    }
}