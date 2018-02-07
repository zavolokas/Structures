using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace Zavolokas.Structures
{
    public static partial class ImageExtensions
    {
        // TODO: move this hardcode to some global settings.
        private const int NotDividableMinAmountElements = 90;
        private const int ThreadsCount = 4;

        private static readonly double[] GaussianFilter = new double[]
            {
               1, 4, 6, 4, 1,
               4, 16, 24, 16, 4,
               6, 24, 36, 24, 6,
               4, 16, 24, 16, 4,
               1, 4, 6, 4, 1
            };

        private static readonly double[] GaussianFilter3x3 = new double[]
            {
               1, 2, 1,
               2, 4, 2,
               1, 2, 1,
            };


        public static ZsImage PyramidDownLab(this ZsImage labImage)
        {
            return labImage.PyramidDownLab(true);
        }

        public static ZsImage PyramidDownArgb(this ZsImage argbImage, bool blur = true)
        {
            var filterArea = Area2D.Create(0, 0, argbImage.Width, argbImage.Height);

            return argbImage.PyramidDownArgb(filterArea, blur);
        }

        public static ZsImage PyramidDownArgb(this ZsImage argbImage, Area2D filterArea, bool blur = true)
        {
            if (blur)
            {
                argbImage = argbImage.Filter(filterArea, GaussianFilter, 5, 5, 1.0 / 16.0);
            }

            argbImage = argbImage.ScaleDown2x();
            return argbImage;
        }

        public static ZsImage PyramidDownLab(this ZsImage labImage, bool blur)
        {
            return labImage
                .FromLabToRgb()
                .FromRgbToArgb(Area2D.Create(0, 0, labImage.Width, labImage.Height))
                .PyramidDownArgb(blur)
                .FromArgbToRgb(new[] { 1.0, 1.0, 1.0 })
                .FromRgbToLab();
        }

        public static ZsImage PyramidUpLab(this ZsImage labImage)
        {
            labImage = labImage
                .FromLabToRgb()
                //.FromRgbToArgb()
                .ScaleUp2x()
                .Filter(GaussianFilter, 5, 5, 4.0 / 16.0)
                //.Filter(GaussianFilter3x3, 3, 3)
                //.FromArgbToRgb()
                .FromRgbToLab();

            return labImage;
        }

        public static Area2D FromArgbToArea2D(this ZsImage argbImage)
        {
            //TODO: check the ColorSpace not the components amount
            if (argbImage.NumberOfComponents < 4)
                throw new ArgumentException("Image in ARGB color space is required.");

            var width = argbImage.Width;
            var height = argbImage.Height;
            var pixelsToFilterAmout = width * height;
            var componentsAmount = argbImage.NumberOfComponents;
            var pixelsData = argbImage.PixelsData;

            var pixelsArea = new byte[height][];
            for (int y = 0; y < height; y++)
            {
                pixelsArea[y] = new byte[width];
            }

            var partsCount = pixelsToFilterAmout > NotDividableMinAmountElements
                ? ThreadsCount
                : 1;
            var partSize = pixelsToFilterAmout / partsCount;

            //Parallel.For(0, partsCount, partIndex =>
            for (int partIndex = 0; partIndex < partsCount; partIndex++)
            {
                var firstPointIndex = partIndex * partSize;
                var lastPointIndex = firstPointIndex + partSize - 1;
                if (partIndex == partsCount - 1) lastPointIndex = pixelsToFilterAmout - 1;
                if (lastPointIndex > pixelsData.Length) lastPointIndex = pixelsToFilterAmout - 1;

                for (int ii = firstPointIndex; ii <= lastPointIndex; ii++)
                {
                    if (pixelsData[ii * componentsAmount] > 0)
                    {
                        pixelsArea[ii / width][ii % width] = 1;
                    }
                }
            }
            //);

            return Area2D.Create(0, 0, pixelsArea);
        }

        public static ZsImage Filter(this ZsImage argbImage, double[] filter, byte filterWidth, byte filterHeight, double multiplicator = 1.0)
        {
            var area = Area2D.Create(0, 0, argbImage.Width, argbImage.Height);
            return argbImage.Filter(area, filter, filterWidth, filterHeight, multiplicator);
        }

        public static ZsImage Filter(this ZsImage argbImage, Area2D filterArea, double[] filter, byte filterWidth, byte filterHeight, double multiplicator = 1.0)
        {
            if (filterWidth * filterHeight != filter.Length)
                throw new ArgumentOutOfRangeException("The passed size of the filter doesn't match the filter size.");

            var imageBoundArea = Area2D.Create(0, 0, argbImage.Width, argbImage.Height);

            filterArea = filterArea
                .Intersect(imageBoundArea);

            // The filter should be applied to the specified area of interest
            argbImage.FilterArea(filterArea, filter, filterWidth, filterHeight, multiplicator);

            return argbImage;
        }

        private static void FilterArea(this ZsImage image, Area2D area, double[] filter, byte filterWidth, byte filterHeight, double multiplicator = 1.0)
        {
            if (filterWidth * filterHeight != filter.Length)
                throw new ArgumentOutOfRangeException("The passed size of the filter doesn't match the filter size.");

            var componentsAmount = image.NumberOfComponents;
            var imageWidth = image.Width;
            var imageHeight = image.Height;
            var pixelsToFilterAmout = area.ElementsCount;
            var resultPixelsData = image.PixelsData;

            var pixelsToFilterIndices = new int[pixelsToFilterAmout];
            area.FillMappedPointsIndexes(pixelsToFilterIndices, imageWidth);

            var pixelsData = new double[resultPixelsData.Length];
            resultPixelsData.CopyTo(pixelsData, 0);
            byte offs = (byte)(filterWidth / 2);

            var partsCount = pixelsToFilterAmout > NotDividableMinAmountElements
                ? ThreadsCount
                : 1;
            var partSize = pixelsToFilterAmout / partsCount;

            //Parallel.For(0, partsCount, partIndex =>
            for (int partIndex = 0; partIndex < partsCount; partIndex++)
            {
                var pixelsToFilterIndicesSet = new HashSet<int>(pixelsToFilterIndices);
                var pixel = new double[componentsAmount];

                var firstPointIndex = partIndex * partSize;
                var lastPointIndex = firstPointIndex + partSize - 1;
                if (partIndex == partsCount - 1) lastPointIndex = pixelsToFilterAmout - 1;
                if (lastPointIndex > pixelsData.Length) lastPointIndex = pixelsToFilterAmout - 1;

                for (int ii = firstPointIndex; ii <= lastPointIndex; ii++)
                {
                    int pixelIndex = pixelsToFilterIndices[ii];

                    int ix = pixelIndex % imageWidth;
                    int iy = pixelIndex / imageWidth;

                    var sum = 0.0;

                    for (int fy = -offs; fy <= offs; fy++)
                    {
                        for (int fx = -offs; fx <= offs; fx++)
                        {
                            int x = ix + fx;
                            int y = iy + fy;

                            if (x < 0 || imageWidth <= x ||
                                y < 0 || imageHeight <= y)
                                continue;

                            int neighbourIndex = x + y * imageWidth;

                            if (!pixelsToFilterIndicesSet.Contains(neighbourIndex))
                                continue;

                            var filterVal = filter[(fx + offs) + (fy + offs) * filterWidth] * multiplicator;
                            sum += filterVal;

                            var pi = y * imageWidth + x;
                            var j = pi * componentsAmount;

                            for (int c = 0; c < componentsAmount; c++)
                            {
                                pixel[c] += pixelsData[j + c] * filterVal;
                            }
                        }
                    }

                    if (sum <= 0) sum = 1.0;

                    var pixelPosition = pixelIndex * componentsAmount;

                    for (int c = 0; c < componentsAmount; c++)
                    {
                        resultPixelsData[pixelPosition + c] = pixel[c] / sum;
                        if (resultPixelsData[pixelPosition + c] > 1) resultPixelsData[pixelPosition + c] = 1.0;
                        if (resultPixelsData[pixelPosition + c] < 0) resultPixelsData[pixelPosition + c] = 0.0;
                        pixel[c] = 0;
                    }
                }
            }
            //);
        }

        public static ZsImage CopyFromImage(this ZsImage destImage, Area2D destArea, ZsImage srcImage, Area2D srcArea)
        {
            if (destImage.NumberOfComponents != srcImage.NumberOfComponents)
                throw new DifferentImageFormatException();

            var destImageArea = Area2D.Create(0, 0, destImage.Width, destImage.Height);
            var srcImageArea = Area2D.Create(0, 0, srcImage.Width, srcImage.Height);

            // move areas to origin and intersect them
            var oDestArea = destArea
                .Intersect(destImageArea)
                .Translate(-destArea.Bound.X, -destArea.Bound.Y);

            var oSrcArea = srcArea
                .Intersect(srcImageArea)
                .Translate(-srcArea.Bound.X, -srcArea.Bound.Y);

            var intersection = oDestArea.Intersect(oSrcArea);

            // move 2 intersected areas back
            destArea = intersection.Translate(destArea.Bound.X, destArea.Bound.Y);
            srcArea = intersection.Translate(srcArea.Bound.X, srcArea.Bound.Y);


            // copy pixels from intersection to intersection
            var destPixelIndecies = new int[destArea.ElementsCount];
            var srcPixelIndecies = new int[srcArea.ElementsCount];

            destArea.FillMappedPointsIndexes(destPixelIndecies, destImage.Width);
            srcArea.FillMappedPointsIndexes(srcPixelIndecies, srcImage.Width);

            var dstPixelsData = destImage.PixelsData;
            var srcPixelsData = srcImage.PixelsData;

            //TODO: Speedup using multiple threads and pointers
            for (int i = 0; i < destPixelIndecies.Length; i++)
            {
                var dstPixelIndex = destPixelIndecies[i];
                var srcPixelIndex = srcPixelIndecies[i];

                for (int j = 0; j < destImage.NumberOfComponents; j++)
                {
                    dstPixelsData[dstPixelIndex * destImage.NumberOfComponents + j] =
                        srcPixelsData[srcPixelIndex * destImage.NumberOfComponents + j];
                }
            }

            return destImage;
        }

        public static ZsImage ScaleTo(this ZsImage image, int width, int height)
        {
            while (image.Width < width || image.Height < height)
            {
                image = image.ScaleUp2x();
            }
            return image;
        }
    }
}
