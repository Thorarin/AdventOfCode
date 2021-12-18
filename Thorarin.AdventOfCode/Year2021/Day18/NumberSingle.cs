namespace Thorarin.AdventOfCode.Year2021.Day18;

internal class NumberSingle : NumberNode
{
    public NumberSingle(int number)
    {
        Number = number;
    }
    
    public int Number { get; set; }
   
    protected internal override int GetMagnitude()
    {
        return Number;
    }

    protected internal override NumberNode Clone()
    {
        return new NumberSingle(Number);
    }

    protected internal override bool Split()
    {
        if (Number < 10) return false;
        var (quotient, remainder) = Math.DivRem(Number, 2);
        var replacement = new NumberPair(new NumberSingle(quotient), new NumberSingle(quotient + remainder));

        if (Parent == null)
        {
            throw new InvalidOperationException("Expected a parent NumberPair");
        }

        if (Parent.Left == this)
        {
            Parent.Left = replacement;
        }
        else if (Parent.Right == this)
        {
            Parent.Right = replacement;
        }

        return true;
    }

    public override string ToString()
    {
        return Number.ToString();
    }
}