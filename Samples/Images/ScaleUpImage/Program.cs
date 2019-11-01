using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Zavolokas.GdiExtensions;
using Zavolokas.Structures;
using Zavolokas.Utils.Processes;

namespace ScaleUpImage
{
    class Program
    {
        static void Main(string[] args)
        {
            const string basePath = "..\\..\\..\\..\\..\\..\\images";

            var bitmap = new Bitmap(Path.Combine(basePath, "t009.jpg"));
            var image = bitmap.ToRgbImage()
                .FromRgbToLab();
            bitmap.Dispose();

            image = image.ScaleUp2x();
            //image = image.ScaleDown2x();

            image = image
                .ScaleDown2x()
                .ScaleDown2x()
                .ScaleDown2x()
                .ScaleDown2x();

            image
                .FromLabToRgb()
                .FromRgbToBitmap()
                .SaveTo(@"..\..\..\out.png", ImageFormat.Png)
                .ShowFile();
        }
    }
}
