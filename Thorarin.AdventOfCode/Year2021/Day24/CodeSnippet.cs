using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021.Day24;

public class CodeSnippet
{
    public List<Operation> Operations { get; }

    public int[] ResetsRegisters { get; set; } 
    
    public CodeSnippet(List<Operation> ops)
    {
        Operations = ops;
        Analyze();
    }

    public static CodeSnippet Parse(TextReader reader)
    {
        var ops = new List<Operation>();
        
        foreach (var line in reader.AsLines())
        {
            var op = Operation.Parse(line);
            if (op.Instruction == Instruction.Inp) break;
            ops.Add(op);    
        }

        return new CodeSnippet(ops);
    }
    
    public bool Run(long[] registers, int input)
    {
        // Write input into register "w"
        registers[0] = input;

        foreach (var op in Operations)
        {
            if (!op.Execute(registers)) return false;
        }

        return true;
    }

    public void Analyze()
    {
        Span<bool> usedRegisters = stackalloc bool[4];
        List<int> resetRegisters = new();
        
        foreach (var op in Operations)
        {
            if (op.Instruction == Instruction.Mul && op.Literal == 0)
            {
                resetRegisters.Add(op.Register);
            }
            else
            {
                usedRegisters[op.Register] = true;
                if (op.Register2.HasValue) usedRegisters[op.Register2.Value] = true;
            }
        }

        ResetsRegisters = resetRegisters.Distinct().ToArray();
    }
}