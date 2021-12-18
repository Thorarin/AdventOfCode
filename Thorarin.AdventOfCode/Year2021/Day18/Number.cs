namespace Thorarin.AdventOfCode.Year2021.Day18;

public class Number
{
    private Number(NumberPair root)
    {
        Root = root;
    }
    
    private NumberPair Root { get; }
    
    public static Number Parse(string str)
    {
        return Parse(new StringReader(str));
    }

    public static Number Parse(StringReader reader)
    {
        return new Number(NumberPair.Parse(reader));
    }

    public int GetMagnitude()
    {
        return Root.GetMagnitude();
    }
    
    public static Number operator +(Number left, Number right)
    {
        var root = new NumberPair(left.Root.Clone(), right.Root.Clone());
        root.Reduce();
        return new Number(root);
    }

    public override string ToString() => Root.ToString();
}