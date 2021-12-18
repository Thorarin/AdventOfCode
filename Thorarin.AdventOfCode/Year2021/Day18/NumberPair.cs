using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021.Day18;

internal class NumberPair : NumberNode
{
    private NumberNode _left;
    private NumberNode _right;

    public NumberPair(NumberNode left, NumberNode right)
    {
        Left = left;
        Right = right;
    }
    
    public NumberNode Left
    {
        get => _left;
        set
        {
            _left = value;
            _left.Parent = this;
        }
    }

    public NumberNode Right
    {
        get => _right;
        set
        {
            _right = value;
            _right.Parent = this;
        }
    }

    public static NumberPair Parse(StringReader reader)
    {
        var start = reader.Read();
        if (start != '[') throw new Exception();

        NumberNode left;
            
        if (reader.Peek() == '[')
        {
            left = Parse(reader);
            if (reader.Read() != ',') throw new Exception();
        }
        else
        {
            string str = reader.ReadUntil(',')!;
            left = new NumberSingle(int.Parse(str));
        }
            
        NumberNode right;
        if (reader.Peek() == '[')
        {
            right = Parse(reader);
            if (reader.Read() != ']') throw new Exception();
        }
        else
        {
            string str = reader.ReadUntil(']')!;
            right = new NumberSingle(int.Parse(str));
        }

        return new NumberPair(left, right);
    }

    protected internal override int GetMagnitude()
    {
        return Left.GetMagnitude() * 3 + Right.GetMagnitude() * 2;
    }

    protected internal void Reduce()
    {
        while (Explode() || Split()) { }
    }

    internal bool Explode(int depth = 0)
    {
        if (depth == 4)
        {
            var numberLeft = FindLeftAdjacentRegularNumber();
            if (numberLeft != null)
            {
                numberLeft.Number += ((NumberSingle)Left).Number;
            }
            var numberRight = FindRightAdjacentRegularNumber();
            if (numberRight != null)
            {
                numberRight.Number += ((NumberSingle)Right).Number;
            }

            var replacement = new NumberSingle(0);
                
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
            
        bool exploded = false;
        if (Left is NumberPair left)
        {
            exploded = left.Explode(depth + 1);
        }

        if (!exploded && Right is NumberPair right)
        {
            exploded = right.Explode(depth + 1);
        }

        return exploded;
    }

    protected internal override bool Split()
    {
        return Left.Split() || Right.Split();
    }
        
    private NumberSingle? FindLeftAdjacentRegularNumber()
    {
        var current = Parent;
        var previous = this;

        while (current != null && current.Left == previous)
        {
            previous = current;
            current = current.Parent;
        }

        if (current == null) return null; 
            
        return current.Left switch
        {
            NumberSingle l => l,
            NumberPair p => GetRightMostSingleNumber(p),
            _ => throw new ArgumentOutOfRangeException()
        };            
    }
        
    private NumberSingle? FindRightAdjacentRegularNumber()
    {
        var current = Parent;
        var previous = this;

        while (current != null && current.Right == previous)
        {
            previous = current;
            current = current.Parent;
        }

        if (current == null) return null;

        return current.Right switch
        {
            NumberSingle l => l,
            NumberPair p => GetLeftMostSingleNumber(p),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private NumberSingle GetLeftMostSingleNumber(NumberPair number)
    {
        if (number.Left is NumberPair p)
        {
            return GetLeftMostSingleNumber(p);
        }
        return (NumberSingle)number.Left;
    }
        
    private NumberSingle GetRightMostSingleNumber(NumberPair number)
    {
        if (number.Right is NumberPair p)
        {
            return GetRightMostSingleNumber(p);
        }
        return (NumberSingle)number.Right;
    }

    protected internal override NumberNode Clone()
    {
        return new NumberPair(Left.Clone(), Right.Clone());
    }
    
    public override string ToString()
    {
        return $"[{Left},{Right}]";
    }
}