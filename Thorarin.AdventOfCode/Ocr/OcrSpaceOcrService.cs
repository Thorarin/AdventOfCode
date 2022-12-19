using System.Drawing.Imaging;
using System.Drawing;
using System.Net.Http.Json;
using System.Runtime.Versioning;
using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Ocr
{
    public class OcrSpaceOcrService : IOcrService
    {
        private readonly string _apiKey;

        public OcrSpaceOcrService(SecretStore secretStore)
        {
            _apiKey = secretStore.GetSecret("OcrApiKey");
        }

        [SupportedOSPlatform("windows")]
        public async Task<string> OcrBitmap(IEnumerable<bool> pixels, int width, int height)
        {
            bool[,] bitmap = new bool[width, height];

            int y = 0;
            foreach (var line in pixels.Chunk(width))
            {
                for (int x = 0; x < width; x++)
                {
                    bitmap[x, y] = line[x];
                }
                y++;
            }

            return await OcrBitmap(bitmap);
        }

        [SupportedOSPlatform("windows")]
        public async Task<string> OcrBitmap(bool[,] grid)
        {
            var png = MakePng(grid);
            return await OcrPngImage(png);
        }

        [SupportedOSPlatform("windows")]
        private byte[] MakePng(bool[,] grid, int margin = 10, int mag = 2)
        {
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);

            Bitmap bitmap = new Bitmap(width * mag + margin * 2, height * mag + margin * 2);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(Brushes.White, 0, 0, (width * mag) + margin * 2, (height * mag) + margin * 2);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (grid[x, y])
                    {
                        graphics.FillRectangle(Brushes.Black, x * mag + 10, y * mag + 10, mag, mag);

                        if (y < height - 1 && x < width - 1)
                        {
                            if (grid[x + 1, y + 1] && !grid[x + 1, y] && !grid[x, y + 1])
                            {
                                bitmap.SetPixel(x * mag + 12, y * mag + 11, Color.Black);
                                bitmap.SetPixel(x * mag + 11, y * mag + 12, Color.Black);
                            }
                        }

                        if (y > 0 && x < width - 1)
                        {
                            if (grid[x + 1, y - 1] && !grid[x + 1, y] && !grid[x, y - 1])
                            {
                                bitmap.SetPixel(x * mag + 12, y * mag + 10, Color.Black);
                                bitmap.SetPixel(x * mag + 11, y * mag + 9, Color.Black);
                            }
                        }

                    }

                }
            }

            using var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream.GetBuffer();
        }


        private async Task<string> OcrPngImage(byte[] image)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("apikey", _apiKey),
                new KeyValuePair<string, string>("base64Image", "data:image/png;base64," + Convert.ToBase64String(image)),
                new KeyValuePair<string, string>("filetype", "PNG"),
                new KeyValuePair<string, string>("OCREngine", "2")
            });

            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var response = await client.PostAsync("https://api.ocr.space/parse/image", formContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<OcrResult>();
            if (content != null && content.ParsedResults.Count > 0)
            {
                return content.ParsedResults[0].ParsedText;
            }

            return "(OCR failed)";
        }

    }
}
