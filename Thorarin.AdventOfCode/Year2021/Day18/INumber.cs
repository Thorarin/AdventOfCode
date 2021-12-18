namespace Thorarin.AdventOfCode.Year2021.Day18;

public interface INumber
{
    INumber Add(INumber other);

    INumber Reduce();

    NumberPair? Parent { get; set; }

    bool Split();
    
    int Magnitude();
}