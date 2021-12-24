using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021.Day24;

public abstract class Day24Base : Puzzle
{
    protected IReadOnlyList<CodeSnippet> Snippets { get; private set; }
    
    public override void ParseInput(TextReader reader)
    {
        reader.ReadLine();

        var snippets = new List<CodeSnippet>();

        while (reader.Peek() != -1)
        {
            snippets.Add(CodeSnippet.Parse(reader));
        }

        Snippets = snippets.ToArray();
    }
    

    protected Func<long, int, bool> Prune(int pos)
    {
        var addOp = Snippets[pos].Operations
            .Find(op => op.Instruction == Instruction.Add &&
                        op.Register == 1 && op.Literal.HasValue);

        var divOp = Snippets[pos].Operations
            .Find(op => op.Instruction == Instruction.Div &&
                        op.Register == 3 && op.Literal.HasValue);

        long addAmount = addOp.Literal.Value;
        long divBy = divOp.Literal.Value;

        if (divBy == 1) return (long z, int w) => true;
        return (long z, int w) => z % 26 == w - addAmount;
    }


    protected Func<long, int, bool>[] GetAllPruneFunctions()
    {
        return Enumerable.Range(0, 14).Select(Prune).ToArray();
    }
}