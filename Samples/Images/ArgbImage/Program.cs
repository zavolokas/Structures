using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Zavolokas;
using Zavolokas.ImageProcessing;
using Zavolokas.Structures;

namespace ArgbImage
{
    /// <summary>
    /// The sample demonstrates:
    ///  - how a bitmap can be converted to the argbImage
    ///  - how the argbImage can be converted to an Area 
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
                
                bitmap
                    .ToRgbImage()
                    .FromRgbToArgb(opaqueArea)              // adds transparent area
                    .SetComponentsValues(opaqueArea, new[] { 0.5 }, 0)
                    .FromArgbToRgb(new[] { 0.5, 0.3, 0.8 })   // converts the semi-transparent image into opaque applying bg color.
                    .FromRgbToBitmap()
                    .SaveTo("..\\..\\out.png", ImageFormat.Png)
                    .ShowFile();
            }
        }
    }
}
