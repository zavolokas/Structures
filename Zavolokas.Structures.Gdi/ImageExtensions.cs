using System.Drawing;
using System.Drawing.Imaging;

namespace Zavolokas.Structures
{
    public static class ImageGdiExtensions
    {
        /// <summary>
        /// Converts ZsImage in RGB format to the Bitmap image.
        /// </summary>
        /// <param name="rgbImage">The RGB image.</param>
        /// <returns></returns>
        public static unsafe Bitmap FromRgbToBitmap(this ZsImage rgbImage)
        {
            var width = rgbImage.Width;
            var height = rgbImage.Height;
            double[] pixelsData = rgbImage.PixelsData;

            Bitmap result = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            BitmapData bd = result.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            fixed (double* pixelsDataP = pixelsData)
            {
                try
                {
                    byte* curpos = (byte*)bd.Scan0;
                    for (int i = 0; i < pixelsData.Length; i += 3)
                    //for (int ii = 0; ii < pixelIndices.Length; ii++)
                    {
                        //int i = pixelIndices[ii]*3;

                        byte A = 255;
                        byte R = (byte)(*(pixelsDataP + i + 0) * 255);
                        byte G = (byte)(*(pixelsDataP + i + 1) * 255);
                        byte B = (byte)(*(pixelsDataP + i + 2) * 255);

                        if (R < 0) R = 0;
                        else if (R > 255) R = 255;

                        if (G < 0) G = 0;
                        else if (G > 255) G = 255;

                        if (B < 0) B = 0;
                        else if (B > 255) B = 255;

                        *(curpos++) = B;
                        *(curpos++) = G;
                        *(curpos++) = R;
                        *(curpos++) = A;
                    }
                }
                finally
                {
                    result.UnlockBits(bd);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts ZsImage in ARGB format into the Bitmap.
        /// </summary>
        /// <param name="argbImage">The ARGB image.</param>
        /// <returns></returns>
        public static unsafe Bitmap FromArgbToBitmap(this ZsImage argbImage)
        {
            var width = argbImage.Width;
            var height = argbImage.Height;
            double[] pixelsData = argbImage.PixelsData;
            var componentsAmount = argbImage.NumberOfComponents;

            Bitmap result = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            BitmapData bd = result.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            fixed (double* componentsP = pixelsData)
            {
                try
                {
                    byte* curpos = (byte*)bd.Scan0;
                    for (int i = 0; i < pixelsData.Length; i += componentsAmount)
                    {
                        byte A = (byte)(*(componentsP + i + 0) * 255);
                        byte R = (byte)(*(componentsP + i + 1) * 255);
                        byte G = (byte)(*(componentsP + i + 2) * 255);
                        byte B = (byte)(*(componentsP + i + 3) * 255);

                        if (A < 0) A = 0;
                        else if (A > 255) A = 255;

                        if (R < 0) R = 0;
                        else if (R > 255) R = 255;

                        if (G < 0) G = 0;
                        else if (G > 255) G = 255;

                        if (B < 0) B = 0;
                        else if (B > 255) B = 255;

                        *(curpos++) = B;
                        *(curpos++) = G;
                        *(curpos++) = R;
                        *(curpos++) = A;
                    }
                }
                finally
                {
                    result.UnlockBits(bd);
                }
            }

            return result;
        }


        /// <summary>
        /// Converts Bitmap to the ZsImage in ARGB format.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <returns></returns>
        public static unsafe ZsImage ToArgbImage(this Bitmap bitmap)
        {
            var width = bitmap.Width;
            var height = bitmap.Height;

            var components = new double[4 * width * height];

            const double N = 1.0 / 255.0;

            BitmapData bd = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            fixed (double* componentsP = components)
            {
                try
                {
                    byte* curpos = (byte*)bd.Scan0;
                    //TODO: invert and paralelize 
                    for (int i = 0; i < components.Length; i += 4)
                    {
                        // components should be in the range [0.0 , 1.0]
                        // but we currently have them in the [0, 255] range
                        double B = (*(curpos++)) * N;
                        double G = (*(curpos++)) * N;
                        double R = (*(curpos++)) * N;
                        double A = (*(curpos++)) * N;

                        if (A < 0) A = 0;
                        else if (A > 1) A = 1;

                        if (R < 0) R = 0;
                        else if (R > 1) R = 1;

                        if (G < 0) G = 0;
                        else if (G > 1) G = 1;

                        if (B < 0) B = 0;
                        else if (B > 1) B = 1;

                        *(componentsP + i + 0) = A;
                        *(componentsP + i + 1) = R;
                        *(componentsP + i + 2) = G;
                        *(componentsP + i + 3) = B;
                    }
                }
                finally
                {
                    bitmap.UnlockBits(bd);
                }
            }

            var result = new ZsImage(components, width, height, 4);
            return result;
        }

        /// <summary>
        /// Converts Bitmap into ZsImage in the RGB format.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <returns></returns>
        public static unsafe ZsImage ToRgbImage(this Bitmap bitmap)
        {
            var width = bitmap.Width;
            var height = bitmap.Height;

            var components = new double[3 * width * height];

            /*
            var pixelsArea = new byte[height][];
            for (int y = 0; y < height; y++)
            {
                pixelsArea[y] = new byte[width];
            }

            var isSemiTransparent = false;
            */

            const double N = 1.0 / 255.0;

            BitmapData bd = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            fixed (double* componentsP = components)
            {
                try
                {
                    byte* curpos = (byte*)bd.Scan0;
                    //TODO: invert and paralelize 
                    for (int i = 0; i < components.Length; i += 3)
                    {
                        // components should be in the range [0.0 , 1.0]
                        // but we currently have them in the [0, 255] range
                        double B = (*(curpos++)) * N;
                        double G = (*(curpos++)) * N;
                        double R = (*(curpos++)) * N;
                        curpos++;

                        /*
                        double A = (*(curpos++)) * N;

                        if (A != 0)
                        {
                            pixelsArea[(i / 3) / width][(i / 3) % width] = 1;
                        }
                        else isSemiTransparent = true;
                        */

                        if (R < 0) R = 0;
                        else if (R > 1) R = 1;

                        if (G < 0) G = 0;
                        else if (G > 1) G = 1;

                        if (B < 0) B = 0;
                        else if (B > 1) B = 1;

                        *(componentsP + i + 0) = R;
                        *(componentsP + i + 1) = G;
                        *(componentsP + i + 2) = B;
                    }
                }
                finally
                {
                    bitmap.UnlockBits(bd);
                }
            }

            //if (isSemiTransparent)
            //{
            //    var area = Area2D.Create(0, 0, pixelsArea);
            //    result = new ImageProcessing.ZsImage(components, area, width, height, 3);
            //}
            //else
            var result = new ZsImage(components, width, height, 3);

            return result;
        }
    }
}
