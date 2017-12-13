using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Zavolokas.GdiExtensions;
using Zavolokas.Structures;
using Zavolokas.Utils.Processes;

namespace Trancparancy
{
    /// <summary>
    /// The sample demonstrates how the transparency can be lost and restored.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            const string basePath = "..\\..\\..\\..\\..\\images";

            using (var bitmap = new Bitmap(Path.Combine(basePath, "pm2.png")))
            using (var markup = new Bitmap(Path.Combine(basePath, "pm2_m.png")))
            {
                var opaqueArea = markup.ToArea();

                var bmp = bitmap
                    .ToRgbImage() // here we loose the information about transparency 
                    .InsertComponents(new[] { 0.0 }, 0) // add extra component at the 0 position (alpha channel)
                    .SetComponentsValues(opaqueArea, new[] { 1.0 }, 0);

                bmp.FromArgbToBitmap()
                    .SaveTo("..\\..\\argb.png", ImageFormat.Png);
                    
                bmp
                    .RemoveComponents(0)
                    .FromRgbToBitmap()
                    .SaveTo("..\\..\\rgb.png", ImageFormat.Png)
                    .ShowFile();
            }
        }
    }
}
