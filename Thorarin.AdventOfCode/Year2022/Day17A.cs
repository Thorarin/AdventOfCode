using System.Diagnostics;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 17, Part = 1)]
public class Day17A : Puzzle
{ 
    private string _jets;

    public override void ParseInput(TextReader reader)
    {
        _jets = reader.ReadLine();
    }

    public override Output SampleExpectedOutput => 3068;

    public override Output ProblemExpectedOutput => 3184;

    public override Output Run()
    {
        int height = -1;
        bool[,] tower = new bool[10000, 7];

        using var jets = Jets().GetEnumerator();
        var blocks = Blocks().Take(2022);

        foreach (var block in blocks)
        {
            int blockX = 2;
            int blockY = height + block.GetLength(0) + 3;

            while (true)
            {
                jets.MoveNext();

                TryMove(jets.Current, 0, block, ref blockX, ref blockY);

                bool success = TryMove(0, -1, block, ref blockX, ref blockY);

                if (!success)
                {
                    DropBlock(block, blockX, blockY);
                    break;
                }
            }
        }

        return height + 1;

        bool TryMove(int offsetX, int offsetY, bool[,] block, ref int blockX, ref int blockY)
        {
            if (blockX + offsetX < 0) return false;
            if (blockX + offsetX + block.GetLength(1) - 1 >= 7) return false;
            if (blockY + offsetY - block.GetLength(0) + 1 < 0) return false;

            for (int y = block.GetLength(0) - 1; y >= 0; y--)
            {
                for (int x = 0; x < block.GetLength(1); x++)
                {
                    if (block[y, x] && tower[blockY - y + offsetY, blockX + x + offsetX])
                    {
                        return false;
                    }
                }
            }

            blockX += offsetX;
            blockY += offsetY;

            return true;
        }

        void DropBlock(bool[,] block, int blockX, int blockY)
        {
            for (int x = 0; x < block.GetLength(1); x++)
            {
                for (int y = 0; y < block.GetLength(0); y++)
                {
                    if (block[y, x])
                    {
                        if (blockY - y > height) height = blockY - y;
                        tower[blockY - y, blockX + x] = true;
                    }
                }
            }
        }
    }

    private IEnumerable<bool[,]> Blocks()
    {
        var blocks = new bool[5][,];

        blocks[0] = new[,] { { true, true, true, true } };
        blocks[1] = new[,]
        {
            { false, true, false },
            { true,  true, true },
            { false, true, false }
        };
        blocks[2] = new[,]
        {
            { false, false, true },
            { false, false, true },
            { true,  true, true }
        };
        blocks[3] = new[,]
        {
            { true },
            { true },
            { true },
            { true }
        };
        blocks[4] = new[,]
        {
            { true, true },
            { true, true }
        };

        while (true)
        {
            foreach (var block in blocks)
            {
                yield return block;
            }
        }
    }

    private IEnumerable<int> Jets()
    {
        while (true)
        {
            foreach (char jet in _jets)
            {
                yield return jet == '<' ? -1 : 1;
            }
        }
    }
}