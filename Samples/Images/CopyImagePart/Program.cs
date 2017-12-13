using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Zavolokas.GdiExtensions;
using Zavolokas.Structures;
using Zavolokas.Utils.Processes;

namespace CopyImagePart
{
    class Program
    {
        static void Main(string[] args)
        {
            const string basePath = "..\\..\\..\\..\\..\\images";

            var bitmap1 = new Bitmap(Path.Combine(basePath, "pm1.png"));
            var image1 = bitmap1.ToRgbImage();
            bitmap1.Dispose();

            var bitmap2 = new Bitmap(Path.Combine(basePath, "pm2.png"));
            var image2 = bitmap2.ToRgbImage();
            bitmap2.Dispose();

            var bitmap3 = new Bitmap(Path.Combine(basePath, "pm2_m.png"));
            var srcArea = bitmap3.ToArea();
            bitmap3.Dispose();

            var dstArea = Area2D.Create(image2.Width / 2, 50, image2.Width / 2, image2.Height);

            image1
                .CopyFromImage(dstArea, image2, srcArea)
                .FromRgbToBitmap()
                .SaveTo(@"..\..\out.png", ImageFormat.Png)
                .ShowFile();
        }
    }
}
