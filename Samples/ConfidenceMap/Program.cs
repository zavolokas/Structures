using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Zavolokas;
using Zavolokas.ImageProcessing;
using Zavolokas.Structures;

namespace ConfidenceMap
{
    class Program
    {
        static void Main(string[] args)
        {
            var markup = Area2D.Create(0, 0, 5, 5);
            markup = markup.Join(Area2D.Create(5, 5, 16, 16));

            markup
                .CalculatePointsConfidence(1.5, 1.3)
                .ToBitmap(markup, 25, 25)
                .ScaleTo(400, 400, InterpolationMode.NearestNeighbor)
                .SaveTo(@"..\..\conf.png", ImageFormat.Png)
                .ShowFile();
        }
    }
}
