using System;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using Zavolokas.GdiExtensions;
using Zavolokas.Structures;

namespace ConvertToLabImage
{
    class Program
    {
        static void Main(string[] args)
        {
            string imagesPath = "..\\..\\images";
            string testSetName = "256x128";
            string fileName = "picture.png";
            string inputPath = $"{imagesPath}\\{testSetName}";
            string inputFileName = $"{inputPath}\\{fileName}";
            string outputPath = $"{imagesPath}\\{testSetName}\\output";
            string outputFileName = $"{outputPath}\\{fileName}";

            var sw = new Stopwatch();

            //open Bitmap and convert it to Lab image via RgbImage
            var bmp = new System.Drawing.Bitmap(inputFileName);

            Console.Write("Converting a GDI+ bitmap to the RgbImage: ");
            sw.Start();
            var image = bmp.ToRgbImage();
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            Console.Write("Converting an RgbImage to the LabImage: ");
            sw.Restart();
            image = image.FromRgbToLab();
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            //convert Lab image back to RGB
            Console.Write("Converting a LabImage to the RgbImage: ");
            sw.Restart();
            image = image.FromLabToRgb();
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            var bmp2 = image.FromRgbToBitmap();
            if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);
            bmp2.Save(outputFileName, ImageFormat.Png);

            //compare original image and the output that we've got from Lab
            var diff = bmp.GetDiff(bmp2);
            diff.Save($"{inputPath}\\diff.png", ImageFormat.Png);
        }
    }
}
