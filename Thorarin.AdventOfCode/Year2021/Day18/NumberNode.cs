namespace Thorarin.AdventOfCode.Year2021.Day18;

internal abstract class NumberNode
{
    protected internal NumberPair? Parent { get; set; }

    protected internal abstract bool Split();
    
    protected internal abstract int GetMagnitude();

    protected internal abstract NumberNode Clone();
    
}