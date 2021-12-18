namespace Thorarin.AdventOfCode.Year2021.Day18;

public class SingleNumber : INumber
{
    public int Number { get; set; }

    public SingleNumber(int number)
    {
        Number = number;
    }
        
    public int Magnitude()
    {
        return Number;
    }

    public INumber Add(INumber other)
    {
        throw new NotImplementedException();
    }

    public INumber Reduce()
    {
        throw new NotImplementedException();
    }

    public NumberPair? Parent { get; set; }
    public bool Split()
    {
        if (Number < 10) return false;

        var (quotient, remainder) = Math.DivRem(Number, 2);

        var replacement = new NumberPair(new SingleNumber(quotient), new SingleNumber(quotient + remainder));

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