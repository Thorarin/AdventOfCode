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
        var mixer = list.Select((value, pos) => (Value: value * key, Position: pos)).ToList();

        for (int cycle = 0; cycle < cycles; cycle++)
        {
            for (int j = 0; j < mixer.Count; j++)
            {
                int index = mixer.FindIndex(x => x.Position == j);
                var value = mixer[index];
                mixer.RemoveAt(index);

                int newIndex = (int)((index + value.Value) % mixer.Count);
                if (newIndex < 0) newIndex += mixer.Count;
                mixer.Insert(newIndex, value);
            }
        }

        var zero = mixer.FindIndex(x => x.Value == 0);

        return mixer[(zero + 1000) % mixer.Count].Value +
               mixer[(zero + 2000) % mixer.Count].Value +
               mixer[(zero + 3000) % mixer.Count].Value;
    }
}