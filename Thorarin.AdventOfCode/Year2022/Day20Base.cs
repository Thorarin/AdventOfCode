using Thorarin.AdventOfCode.Extensions;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

public abstract class Day20Base : Puzzle
{
    protected IReadOnlyList<int> _input;

    public override void ParseInput(TextReader reader)
    {
        _input = reader.AsLines().Select(int.Parse).ToList();
    }

    protected long Mix(IEnumerable<int> list, long key, int cycles)
    {
        var mixer = list.Select((value, pos) => (Value: value * key, Position: pos)).ToArray();

        for (int cycle = 0; cycle < cycles; cycle++)
        {
            for (int j = 0; j < mixer.Length; j++)
            {
                int index = Array.FindIndex(mixer, x => x.Position == j);
                var value = mixer[index];
                int newIndex = (int)MathEx.Modulo(index + value.Value, mixer.Length - 1);
                
                mixer.Move(index, newIndex);
            }
        }

        var zero = Array.FindIndex(mixer, x => x.Value == 0);

        return mixer[(zero + 1000) % mixer.Length].Value +
               mixer[(zero + 2000) % mixer.Length].Value +
               mixer[(zero + 3000) % mixer.Length].Value;
    }
}