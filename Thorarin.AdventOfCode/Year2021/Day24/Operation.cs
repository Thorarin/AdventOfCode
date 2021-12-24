using System.Text.RegularExpressions;

namespace Thorarin.AdventOfCode.Year2021.Day24;

// I expected this to be faster as a readonly struct, but it wasn't ¯\_(ツ)_/¯
public record Operation(Instruction Instruction, int Register, int? Register2, long? Literal)
{
    private static readonly Regex OpRegex = new("(?<instruction>[a-z]{3}) (?<reg>[wxyz])( )?((?<reg2>[wxyz])|(?<literal>-?\\d+))?");

    public static Operation Parse(string line)
    {
        var match = OpRegex.Match(line);
        if (!match.Success) throw new Exception();

        var instruction = Enum.Parse<Instruction>(match.Groups["instruction"].Value, true);
        var register = match.Groups["reg"].Value[0] - 'w';
        long? literal = default;
        int? register2 = default;
        
        if (match.Groups["literal"].Success)
        {
            literal = int.Parse(match.Groups["literal"].Value);
        }
        else if (match.Groups["reg2"].Success)
        {
            register2 = match.Groups["reg2"].Value[0] - 'w';
        }

        if (instruction != Instruction.Inp && !literal.HasValue && !register2.HasValue)
        {
            throw new Exception(
                $"Expected second operand for {instruction.ToString().ToLowerInvariant()} instruction.");
        }

        return new Operation(instruction, register, register2, literal);
    }

    public bool Execute(long[] registers)
    {
        switch (Instruction)
        {
            case Instruction.Add:
                registers[Register] += GetValue();
                return true;
            case Instruction.Mul:
                registers[Register] *= GetValue();
                return true;
            case Instruction.Div:
            {
                var b = GetValue();
                if (b == 0) return false;
                registers[Register] /= b;
                return true;
            }
            case Instruction.Mod:
            {
                if (registers[Register] < 0) return false;
                var b = GetValue();
                if (b <= 0) return false;
                registers[Register] %= b;
                return true;
            }
            case Instruction.Eql:
                registers[Register] = registers[Register] == GetValue() ? 1 : 0;
                return true;
            default:
                return false;
        }

        long GetValue()
        {
            if (Literal.HasValue) return Literal.Value;
            return registers[Register2!.Value];
        }
    }

    public override string ToString()
    {
        var reg = (char)(Register + 'w');
        string reg2 = Register2.HasValue ? "" + (char)(Register2.Value + 'w') : "";
        return $"{Instruction.ToString().ToLowerInvariant()} {reg} {Literal}{reg2}";
    }
}