using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Zavolokas.Structures
{
    public static class Area2DGdiExtensions
    {
        /// <summary>
        /// Converts an area with given point darkness values to the bitmap.
        /// </summary>
        /// <param name="pointDarkness">The point darkness.</param>
        /// <param name="area">The area.</param>
        /// <param name="width">The width of the result Bitmap.</param>
        /// <param name="height">The height of the result Bitmap.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">There are not enough darkness values for the provided area</exception>
        public static Bitmap ToBitmap(this double[] pointDarkness, Area2D area, int width, int height)
        {
            if (pointDarkness.Length < area.ElementsCount)
                throw new Exception("There are not enough darkness values for the provided shape");

            var result = new Bitmap(width, height);

            //using (var g = Graphics.FromImage(result))
            //using (var b = new SolidBrush(Color.Black))
            //{
            //    g.FillRectangle(b, 0, 0, result.Width, result.Height);
            //}

            var pointIndexes = new int[area.ElementsCount];
            area.FillMappedPointsIndexes(pointIndexes, width);

            for (int i = 0; i < pointIndexes.Length; i++)
            {
                var x = pointIndexes[i] % width;
                var y = pointIndexes[i] / width;
                var val = (int)System.Math.Max(0, System.Math.Min((1.0 - pointDarkness[i]) * 255, 255));
                var c = Color.FromArgb(255, val, val, val);
                result.SetPixel(x, y, c);
            }

            return result;
        }

        /// <summary>
        /// Converts an area with given color to the bitmap.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static Bitmap ToBitmap(this Area2D area, Color color)
        {
            var w = area.Bound.Width > 0 ? area.Bound.X + area.Bound.Width : 256;
            var h = area.Bound.Height > 0 ? area.Bound.Y + area.Bound.Height : 256;

            var bmp = area.ToBitmap(color, w, h);
            return bmp;
        }

        //private static Bitmap ConvertToBitmap(byte[][] markup, Color emptyAreaColor, Color markedAreaColor)
        //{
        //    var width = markup[0].Length;
        //    var height = markup.Length;
        //    var bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

        //    var palette = bmp.Palette;
        //    palette.Entries[0] = emptyAreaColor;
        //    palette.Entries[1] = markedAreaColor;
        //    bmp.Palette = palette;

        //    var boundsRect = new System.Drawing.Rectangle(0, 0, width, height);
        //    var bmpData = bmp.LockBits(boundsRect, ImageLockMode.WriteOnly, bmp.PixelFormat);
        //    var ptr = bmpData.Scan0;

        //    byte[] extra = new byte[bmpData.Stride - width];
        //    for (var i = 0; i < markup.Length; i++)
        //    {
        //        Marshal.Copy(markup[i], 0, ptr, width);
        //        ptr += width;
        //        Marshal.Copy(extra, 0, ptr, extra.Length);
        //        ptr += extra.Length;
        //    }

        //    bmp.UnlockBits(bmpData);
        //    return bmp;
        //}

        //TODO: optimize performance of this method
        /// <summary>
        /// Converts an area with given color to the bitmap.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <param name="areaColor">Color of the area.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Bitmap ToBitmap(this Area2D area, Color areaColor, int width, int height)
        {
            var b = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            var pointIndexes = new int[area.ElementsCount];
            area.FillMappedPointsIndexes(pointIndexes, width);

            for (var i = 0; i < pointIndexes.Length; i++)
            {
                var x = pointIndexes[i] % width;
                var y = pointIndexes[i] / width;

                if (width <= x || height <= y)
                    continue;

                b.SetPixel(x, y, areaColor);
            }
            return b;
        }

        /// <summary>
        /// Converts an area to the confidence map.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <returns></returns>
        public static Bitmap ToConfidenceMap(this Area2D area)
        {
            var confidence = area.CalculatePointsConfidence(1.5, 1.3);
            return confidence.ToBitmap(area, area.Bound.Width, area.Bound.Height);
        }

        /// <summary>
        /// Converts a Bitmap to an area.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <returns></returns>
        public static Area2D ToArea(this Bitmap bitmap)
        {
            byte[][] markup = ConvertToBinaryJaggedArray(bitmap);
            return Area2D.Create(0, 0, markup);
        }

        private static unsafe byte[][] ConvertToBinaryJaggedArray(Bitmap bitmap)
        {
            var width = bitmap.Width;
            var height = bitmap.Height;
            var data = new byte[height][];
            var bd = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            byte curValue;
            try
            {
                byte* curpos;
                for (int h = 0; h < height; h++)
                {
                    data[h] = new byte[width];
                    curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                    for (int w = 0; w < width; w++)
                    {
                        curpos += 3;
                        curValue = *(curpos++);
                        data[h][w] = (curValue > 0) ? (byte)1 : (byte)0;
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bd);
            }
            return data;
        }
    }
}