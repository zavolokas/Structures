using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Zavolokas.GdiExtensions;
using Zavolokas.Structures;
using Zavolokas.Utils.Processes;

namespace Convolution
{
    class Program
    {
        static void Main(string[] args)
        {
            const string basePath = "..\\..\\..\\..\\..\\images";

            var bitmap = new Bitmap(Path.Combine(basePath, "pm2.png"));
            var image = bitmap.ToRgbImage();
            bitmap.Dispose();

            var markup = new Bitmap(Path.Combine(basePath, "pm2_m.png"));
            var filterArea = markup.ToArea();
            markup.Dispose();

            var edgeFilter = new double[]
            {
               0, 1, 0,
               1, -4, 1,
                0, 1, 0,
            };

            var blurFilter = new double[]
            {
               1, 4, 6, 4, 1,
               4, 16, 24, 16, 4,
               6, 24, 36, 24, 6,
               4, 16, 24, 16, 4,
               1, 4, 6, 4, 1
            };

            image
                //.Filter(filterArea, blurFilter, 5, 5, 1.0 / 16.0)
                .Filter(filterArea, edgeFilter, 3, 3)
                .FromRgbToBitmap()
                .SaveTo(@"..\..\out.png", ImageFormat.Png)
                .ShowFile();
        }
    }
}
