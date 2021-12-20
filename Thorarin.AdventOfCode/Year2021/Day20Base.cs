using System.Diagnostics;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2021;

public abstract class Day20Base : Puzzle
{
    private bool[] _algorithm;
    protected Image _image;

    private Dictionary<Point, int> _map = new()
    {
        { new Point(-1, -1), 256 },
        { new Point(0, -1), 128 },
        { new Point(1, -1), 64 },
        { new Point(-1, 0), 32 },
        { new Point(0, 0), 16 },
        { new Point(1, 0), 8 },
        { new Point(-1, 1), 4 },
        { new Point(0, 1), 2 },
        { new Point(1, 1), 1 }
    };

    public override void ParseInput(TextReader reader)
    {
        _algorithm = new bool[512];
        _image = new Image();
        var algLine = reader.ReadLine();
        for (int i = 0; i < 512; i++)
        {
            _algorithm[i] = algLine[i] == '#';
        }

        reader.ReadLine();

        int y = 0;
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == '#')
                {
                    _image[new Point(x, y)] = true;
                }
            }
            y++;
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
                Console.Write(image[new Point(x, y)] ? "#" : '.');
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

        for (int x = oldImage.MinX - 2; x < oldImage.MaxX + 2; x++)
        {
            for (int y = oldImage.MinY - 2; y < oldImage.MaxY + 2; y++)
            {
                CalcPixel(new Point(x, y), oldImage, newImage);
            }
        }

        return newImage;
    }

    private void CalcPixel(Point point, Image oldImage, Image newImage)
    {
        newImage[point] = GetEnhancedPixel(point, oldImage);
    }

    private bool GetEnhancedPixel(Point point, Image image)
    {
        int value = 0;

        foreach (var (offset, multiplier) in _map)
        {
            if (image[point + offset])
            {
                value += multiplier;
            }
        }

        return _algorithm[value];
    }

    protected readonly struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public int X { get; }
        public int Y { get; }
        

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }
    
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }        
    }

    protected class Image
    {
        public Image()
        {
            _image = new Dictionary<Point, bool>();
        }

        public Image(Image old) : this()
        {
            MinX = old.MinX;
            MaxX = old.MaxX;
            MinY = old.MinY;
            MaxY = old.MaxY;

            Infinite = !old.Infinite;
        }
        
        public bool Infinite { get; set; }
        private Dictionary<Point, bool> _image;

        public int MinX { get; private set; } = 0;
        public int MaxX { get; private set;} = 0;
        public int MinY { get; private set;} = 0;
        public int MaxY { get; private set;} = 0;

        public bool this[Point point]
        {
            get
            {
                if (_image.TryGetValue(point, out bool pixel))
                {
                    return pixel;
                }

                return Infinite;
            }
            set
            {
                if (value == Infinite) return;

                //if (point.X < -1 || point.X > 111) return;
                //if (point.Y < -10 || point.Y > 111) return;
                _image[point] = value;

                MinX = Math.Min(MinX, point.X);
                MaxX = Math.Max(MaxX, point.X);
                MinY = Math.Min(MinY, point.Y);
                MaxY = Math.Max(MaxY, point.Y);
            }
            
        }

        public int Count => _image.Count;
    }
    
}