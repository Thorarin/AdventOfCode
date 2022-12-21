using System.Diagnostics;

namespace Thorarin.AdventOfCode.Year2022.Day21;

public class Monkey
{
    public long? Number { get; set; }
    public Monkey? Left { get; set; }
    public Monkey? Right { get; set; }
    public char? Operator { get; set; }

    public long? GetValue()
    {
        if (!Operator.HasValue) return Number;

        switch (Operator)
        {
            case '+':
                return Left!.GetValue() + Right!.GetValue();
            case '-':
                return Left!.GetValue() - Right!.GetValue();
            case '*':
                return Left!.GetValue() * Right!.GetValue();
            case '/':
                return Left!.GetValue() / Right!.GetValue();
            default:
                throw new Exception();
        }
    }

    /// <summary>
    /// Used for part 2 of the puzzle.
    /// </summary>
    public void SetValue(long value)
    {
        if (!Operator.HasValue)
        {
            Number = value;
            return;
        }

        var left = Left!.GetValue();
        var right = Right!.GetValue();

        if (left.HasValue && right.HasValue)
        {
            throw new Exception("Cannot set value");
        }
        else if (left.HasValue)
        {
            switch (Operator)
            {
                case '+':
                    Right.SetValue(value - left.Value);
                    break;
                case '-':
                    Right.SetValue(left.Value - value);
                    break;
                case '*':
                    Right.SetValue(value / left.Value);
                    break;
                case '/':
                    Right.SetValue(left.Value / value);
                    break;
            }
        }
        else if (right.HasValue)
        {
            switch (Operator)
            {
                case '+':
                    Left.SetValue(value - right.Value);
                    break;
                case '-':
                    Left.SetValue(right.Value + value);
                    break;
                case '*':
                    Left.SetValue(value / right.Value);
                    break;
                case '/':
                    Left.SetValue(value * right.Value);
                    break;
            }
        }

        CheckSetValue(value);
    }

    [Conditional("DEBUG")]
    private void CheckSetValue(long value)
    {
        if (GetValue() == value) return;
        var left = Left!.GetValue();
        var right = Right!.GetValue();
        Debug.WriteLine($"{left} {Operator} {right} ≠ {value}");
    }
}