using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Zavolokas.GdiExtensions;
using Zavolokas.Structures;
using Zavolokas.Utils.Processes;

namespace SetComponents
{
    class Program
    {
        static void Main(string[] args)
        {
            const string basePath = "..\\..\\..\\..\\..\\images";
            using (var bitmap = new Bitmap(Path.Combine(basePath, "pm2.png")))
            using (var mask2Bmp = new Bitmap(Path.Combine(basePath, "m007.png")))
            {
                var mask0 = mask2Bmp.ToArea();
                var rect = Area2D.Create(mask0.Bound.X, mask0.Bound.Y, mask0.Bound.Width, mask0.Bound.Height);
                var grect = rect.Dilation(3).Substract(rect);

                var mask2 = mask2Bmp.ToArea()
                    .Translate(240, 80)
                    .Dilation(71);

                var mask1 = mask2.Translate(mask2.Bound.Width / 3, mask2.Bound.Height / 2);
                var mask3 = mask2.Translate(-mask2.Bound.Width / 3, mask2.Bound.Height / 2);

                bitmap.ToRgbImage()
                    .SetComponentsValues(mask1, new[] { 1.0 }, 0)
                    .SetComponentsValues(mask2, new[] { 1.0 }, 1)
                    .SetComponentsValues(mask3, new[] { 1.0 }, 2)
                    .SetComponentsValues(grect, new[] { 1.0, 1.0, 1.0 }, 0)
                    .SetComponentsValues(mask0, new[] { 1.0, 1.0, 1.0 }, 0)
                    .FromRgbToBitmap()
                    .SaveTo("..\\..\\output.png", ImageFormat.Png)
                    .ShowFile();
            }
        }
    }
}
