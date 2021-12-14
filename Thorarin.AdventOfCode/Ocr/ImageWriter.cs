using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace Thorarin.AdventOfCode.Ocr;

public class ImageWriter
{
    public void SaveAsPng(bool[,] bitmap, string fileName)
    {
        int width = bitmap.GetLength(0);
        int height = bitmap.GetLength(1);
        const int mag = 2;
        var image = new Image<Argb32>(Configuration.Default, width * mag + 20, height * mag + 20, Color.White);

        Argb32 black = Color.Black;
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (bitmap[x, y])
                {
                    image[x * mag + 10, y * mag + 10] = black;
                    image[x * mag + 11, y * mag + 10] = black;
                    image[x * mag + 10, y * mag + 11] = black;
                    image[x * mag + 11, y * mag + 11] = black;
                    
                    if (y < height - 1 && x < width - 1)
                    {
                        if (bitmap[x + 1, y + 1] && !bitmap[x + 1, y] && !bitmap[x, y + 1])
                        {
                            image[x * mag + 12, y * mag + 11] = black;
                            image[x * mag + 11, y * mag + 12] = black;
                        }
                    }
                    
                    if (y > 0 && x < width - 1)
                    {
                        if (bitmap[x + 1, y - 1] && !bitmap[x + 1, y] && !bitmap[x, y - 1])
                        {
                            image[x * mag + 12, y * mag + 10] = black;
                            image[x * mag + 11, y * mag + 9] = black;
                        }
                    }                    
                }
            }
        }
        
        image.Save(fileName, new PngEncoder());
    }
}