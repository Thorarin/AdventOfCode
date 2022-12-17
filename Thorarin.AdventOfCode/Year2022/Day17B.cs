using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022;

[Puzzle(Year = 2022, Day = 17, Part = 2)]
public class Day17B : Puzzle
{ 
    private string _jets = "";

    public override void ParseInput(TextReader reader)
    {
        _jets = reader.ReadLine()!;
    }

    public override Output SampleExpectedOutput => 1_514_285_714_288;

    public override Output ProblemExpectedOutput => 1_577_077_363_915;

    public override Output Run()
    {
        long rocks = 1_000_000_000_000;
        var blocks = GetBlockDefinitions();

        // Keep track of position in puzzle input and the alternating block types
        int jetPos = 0;
        int blockType = 0;

        // The height of the similated stack of blocks, plus the height to be
        // added for found repeat patterns.
        int height = -1;
        long extraHeight = 0;

        bool[,] tower = new bool[10_000, 7];
        Dictionary<(int Pos, int BlockType, long Pattern), (long Count, int Height)> repeatDetector = new();
        bool doneRepeats = false;

        long count = 0;
        while (count < rocks)
        {
            var block = NextBlock();
            int blockX = 2;
            int blockY = height + block.GetLength(0) + 3;

            while (true)
            {
                // Move horizontally
                int offset = NextJet();
                TryMove(offset, 0, block, ref blockX, ref blockY);

                // Move down
                bool success = TryMove(0, -1, block, ref blockX, ref blockY);
                if (!success)
                {
                    DropBlock(block, blockX, blockY);
                    break;
                }
            }

            count++;

            if (!doneRepeats)
            {
                // Try and find a pattern in how the blocks are stacking up
                var repeat = FindPattern(jetPos, blockType);
                if (repeat.HasValue)
                {
                    // A repeating pattern has been found.
                    // How many times do we have to repeat it?
                    var repeats = (rocks - count) / repeat.Value.BlockCount;
                    extraHeight = repeats * repeat.Value.RowCount;
                    count += repeats * repeat.Value.BlockCount;
                    doneRepeats = true;
                }
            }

            if (height >= tower.GetLength(0) - 10)
            {
                return "Ran out of space before finding pattern";
            }
        }

        return height + 1 + extraHeight;

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

        (long BlockCount, int RowCount)? FindPattern(int jet, int blockNr)
        {
            if (height <= 10) return default;

            // Mash 8 rows into a 64 bit integer for tracking purposes.
            // More than 8 rows could be relevant, but I can't easily fit them
            // in a variable and the ConfirmPattern method should take care of false positives.
            long pattern = 0;
            for (int y = height; y >= height - 8; y--)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (tower[y, x])
                    {
                        pattern |= 1L << x;
                    }
                }

                pattern <<= 8;
            }

            // The combined state of position in the input and the next block type
            // are used to detect patterns. The top 8 rows are also included.
            var key = (Pos: jet, Type: blockNr, pattern);
            if (repeatDetector.TryGetValue(key, out var lastSeen))
            {
                var patternCandidate = (Count: count - lastSeen.Count, Height: height - lastSeen.Height);

                if (ConfirmPattern(patternCandidate.Height))
                {
                    return patternCandidate;
                }
            }

            repeatDetector[key] = (count, height);

            return default;
        }

        bool ConfirmPattern(int patternHeight)
        {
            // In order to confirm, we need at least 2 repeats of the pattern
            // For debugging, I might want to check more...
            int repeats = height / patternHeight - 1;
            if (repeats < 2) return false;

            for (int i = 1; i < repeats; i++)
            {
                for (int y = height; y >= height - patternHeight; y--)
                {
                    for (int x = 0; x < 7; x++)
                    {
                        if (tower[y - i * patternHeight, x] != tower[y, x])
                            return false;
                    }
                }
            }

            return true;
        }

        int NextJet()
        {
            int result = _jets[jetPos] == '<' ? -1 : 1;
            jetPos = (jetPos + 1) % _jets.Length;
            return result;
        }

        bool[,] NextBlock()
        {
            var block = blocks[blockType];
            blockType = (blockType + 1) % 5;
            return block;
        }
    }

    private bool[][,] GetBlockDefinitions()
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

        return blocks;
    }
}