namespace Thorarin.AdventOfCode.Year2021.Day24;

internal record State(int Pos, long Number, CodeSnippet[] Snippets, long[] Registers)
{
    /// <summary>
    /// Returns new State if the code was successfully executed, or null otherwise.
    /// </summary>
    public State? TryExecute(int input)
    {
        int pos = Pos + 1;
        var snippet = Snippets[pos];
        var registers = Registers.ToArray();

        if (snippet.Run(registers, input))
        {
            return new State(pos, Number + MathEx.Pow(10L, 13 - pos) * input, Snippets, registers);
        }

        return null;
    }

    public long Z => Registers[3];
}