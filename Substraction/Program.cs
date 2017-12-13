using System.Drawing;
using System.Drawing.Imaging;
using Zavolokas.GdiExtensions;
using Zavolokas.Structures;
using Zavolokas.Utils.Processes;

namespace Substraction
{
    class Program
    {
        static void Main(string[] args)
        {
            var area1 = Area2D.Create(10, 10, 20, 20);
            var area2 = Area2D.Create(15, 15, 10, 10);

            var area3 = area1.Substract(area2);

            area3
                .ToBitmap(Color.Red)
                .SaveTo("..\\..\\out.png", ImageFormat.Png)
                .ShowFile();

        }
    }
}
