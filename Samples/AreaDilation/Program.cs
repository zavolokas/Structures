using System.Drawing;
using System.Drawing.Imaging;
using Zavolokas;
using Zavolokas.GdiExtensions;
using Zavolokas.Structures;
using Zavolokas.Utils.Processes;

namespace AreaDilation
{
    // Expands the area.
    // Saves the area as a semitransparent image.

    class Program
    {
        static void Main(string[] args)
        {
            var imageArea = Area2D.Create(0, 0, 200, 140);
            var shape = Area2D.Create(0, 0, 50, 50);
            var biggerSHape = shape.Dilation(23);

            biggerSHape
                .Translate(64, 0)
                .Join(shape.Translate(0, 5))
                .Join(shape.Translate(64, 64))
                .Intersect(imageArea)
                .ToBitmap(Color.Red, 200, 140)
                .SaveTo("..\\..\\out.png", ImageFormat.Png)
                .ShowFile();
        }
    }
}
