using System.Reflection;
using System.Text;

namespace Thorarin.AdventOfCode.Framework;

public abstract record Output
{
    public static implicit operator Output(long output)
    {
        return new LongOutput(output);
    }

    public static implicit operator Output(string output)
    {
        return new StringOutput(output);
    }

    protected abstract string StringValue { get; }
} 