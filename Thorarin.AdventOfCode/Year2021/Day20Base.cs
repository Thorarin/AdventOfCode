using System.Diagnostics;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

public abstract class Day20Base : Puzzle
{
    private bool[] _algorithm;
    protected Image _image;

    public override void ParseInput(TextReader reader)
    {
        _algorithm = reader.ReadLine()!.Select(c => c == '#').ToArray();
        
        reader.ReadLine();

        string[] imageData = reader.ToLineArray();
        _image = new Image(imageData[0].Length, imageData.Length, _algorithm[0], _algorithm[^1]);
        for (int y = 0; y < imageData.Length; y++)
        {
            for (int x = 0; x < imageData[y].Length; x++)
            {
                _image[x, y] = imageData[y][x] == '#';
            }            
        }
    }

    [Conditional("DEBUG")]
    protected void Dump(Image image, long expected)
    {
        if (image.Count == expected) return;
        
        Console.WriteLine();
        for (int y = image.MinY; y < image.MaxY; y++)
        {
            for (int x = image.MinX; x < image.MaxX; x++)
            {
                Console.Write(image[x, y] ? "#" : '.');
            }

            Console.WriteLine();
        }
    }

    protected Image EnhanceTimes(Image oldImage, int times)
    {
        var image = oldImage;
        
        for (int i = 0; i < times; i++)
        {
            image = Enhance(image);
        }
        
        return image;
    }

    protected Image Enhance(Image oldImage)
    {
        var newImage = new Image(oldImage);

        Parallel.ForEach(
            Enumerable.Range(newImage.MinX, newImage.Width),
            x =>
            {
                for (int y = newImage.MinY; y <= newImage.MaxY; y++)
                {
                    CalcPixel(x, y, oldImage, newImage);
                }
            });
            
        return newImage;
    }

    private void CalcPixel(int x, int y, Image oldImage, Image newImage)
    {
        newImage[x, y] = GetEnhancedPixel(x, y, oldImage);
    }

    private bool GetEnhancedPixel(int x, int y, Image image)
    {
        int value = 0;
        
        // Optimized version without use of Point structs
        if (image[x - 1, y - 1]) value |= 256;
        if (image[x, y - 1])     value |= 128;
        if (image[x + 1, y - 1]) value |= 64;
        if (image[x - 1, y])     value |= 32;
        if (image[x, y])         value |= 16;
        if (image[x + 1, y])     value |= 8;
        if (image[x - 1, y + 1]) value |= 4;
        if (image[x, y + 1])     value |= 2;
        if (image[x + 1, y + 1]) value |= 1;
         
         return _algorithm[value];
    }

    protected class Image
    {
        private const int Padding = 1;
        private readonly bool[,] _image;

        // Used for offsetting coordinates to support negative ones,
        // since I can't be bothered with Array class for non-zero indexes.
        private readonly int _offsetX;
        private readonly int _offsetY;
        
        public Image(int width, int height, bool allOff, bool allOn)
        {
            AllOff = allOff;
            AllOn = allOn;
            _image = new bool[width + Padding * 2, height + Padding * 2];
            _offsetX = Padding;
            _offsetY = Padding;

            MinX = -Padding;
            MaxX = width - 1 + Padding;
            MinY = -Padding;
            MaxY = height - 1 + Padding;
        }

        public Image(Image oldImage)
        {
            AllOff = oldImage.AllOff;
            AllOn = oldImage.AllOn;
            InfinityLit = oldImage.InfinityLit ? AllOn : AllOff;

            int width = oldImage.Width + Padding * 2;
            int height = oldImage.Height + Padding * 2;
            
            _image = new bool[width, height];
            _offsetX = -oldImage.MinX + Padding;
            _offsetY = -oldImage.MinY + Padding;
            
            MinX = -_offsetX;
            MaxX = oldImage.MaxX + Padding;
            MinY = -_offsetY;
            MaxY = oldImage.MaxY + Padding;
        }
        
        public bool InfinityLit { get; }
        public bool AllOff { get; }
        public bool AllOn { get; }

        public int MinX { get; }
        public int MaxX { get; }
        public int MinY { get; }
        public int MaxY { get; }

        public int Width => MaxX - MinX + 1;
        public int Height => MaxY - MinY + 1;

        public bool this[int x, int y]
        {
            get
            {
                if (x < MinX || x > MaxX || y < MinY || y > MaxY) return InfinityLit;
                return _image[x + _offsetX, y + _offsetY];
            }
            set
            {
                int x1 = x + _offsetX;
                int y1 = y + _offsetY;
                
                #if DEBUG
                if (x1 < 0 || x1 > _image.GetLength(0)) throw new Exception();
                if (y1 < 0 || y1 > _image.GetLength(1)) throw new Exception();
                #endif
                
                _image[x1, y1] = value;
            }
        }

        public int Count
        {
            get
            {
                int count = 0;
                for (int x = 0; x < _image.GetLength(0); x++)
                {
                    for (int y = 0; y < _image.GetLength(1); y++)
                    {
                        if (_image[x, y]) count++;
                    }
                }

                return count;
            }
        }
    }
}