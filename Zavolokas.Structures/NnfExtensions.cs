using System;
using System.Threading.Tasks;

namespace Zavolokas.Structures
{
    public static class NnfExtensions
    {
        public static unsafe ZsImage ToRgbImage(this Nnf nnf)
        {
            const int NotDividableMinAmountElements = 80;
            var nnfItems = nnf.GetNnfItems();
            var width = nnf.DstWidth;
            var height = nnf.DstHeight;

            var pointsAmount = width * height;

            var rgb = new double[pointsAmount * 3];

            // Decide on how many partitions we should divade the processing
            // of the elements.
            var partsCount = pointsAmount > NotDividableMinAmountElements
                ? Environment.ProcessorCount
                : 1;

            var partSize = (int)(pointsAmount / partsCount);

            Parallel.For(0, partsCount, partIndex =>
            {
                var firstPointIndex = partIndex * partSize;
                var lastPointIndex = firstPointIndex + partSize - 1;
                if (partIndex == partsCount - 1) lastPointIndex = pointsAmount - 1;
                if (lastPointIndex > pointsAmount) lastPointIndex = pointsAmount - 1;

                fixed (double* nnfItemsP = nnfItems)
                fixed (double* rgbP = rgb)
                {
                    var maxY = nnf.SourceHeight;
                    var maxX = nnf.SourceWidth;
                    var maxD = nnf.PatchSize * 100;

                    for (var pointIndex = firstPointIndex; pointIndex <= lastPointIndex; pointIndex++)
                    {
                        var nnfindex = (pointIndex / width) * width * 2 + (pointIndex % width) * 2;
                        var index = *(nnfItemsP + nnfindex + 0);
                        var distance = nnfItems[nnfindex + 1];
                        *(rgbP + pointIndex * 3 + 0) = (double)(((int)index) % maxX) / maxX;
                        *(rgbP + pointIndex * 3 + 1) = (double)(((int)index) / maxX) / maxY;
                        *(rgbP + pointIndex * 3 + 2) = distance / maxD;
                    }
                }
            });
            var result = new ZsImage(rgb, width, height, 3);
            return result;
        }

        public static ZsImage RestoreRgbImage(this Nnf nnf, ZsImage image, byte componentsCount, byte patchSize)
        {
            var lab = nnf.RestoreImage(image, componentsCount, patchSize);
            return lab.FromLabToRgb();
        }

        public static unsafe ZsImage RestoreImage(this Nnf nnf, ZsImage image, byte componentsCount, byte patchSize)
        {
            const int NotDividableMinAmountElements = 80;

            var width = nnf.DstWidth;
            var height = nnf.DstHeight;
            var nnfItems = nnf.GetNnfItems();

            double destWidth = width;
            double destHeight = height;

            double sourceWidth = image.Width;
            double sourceHeight = image.Height;

            int patchOffs = (patchSize - 1) / 2;

            var result = new double[width * height * componentsCount];
            var srcPixelsData = image.PixelsData;
            var pointsAmount = width * height;

            // Decide on how many partitions we should divade the processing
            // of the elements.
            var partsCount = pointsAmount > NotDividableMinAmountElements
                ? Environment.ProcessorCount
                : 1;

            var partSize = (int)(pointsAmount / partsCount);

            Parallel.For(0, partsCount, partIndex =>
            {
                var firstPointIndex = partIndex * partSize;
                var lastPointIndex = firstPointIndex + partSize - 1;
                if (partIndex == partsCount - 1) lastPointIndex = pointsAmount - 1;
                if (lastPointIndex > pointsAmount) lastPointIndex = pointsAmount - 1;

                var color = new double[componentsCount];

                fixed (double* resultP = result)
                fixed (double* srcPixelsP = srcPixelsData)
                fixed (double* colorP = color)
                fixed (double* nnfItemsP = nnfItems)
                {
                    for (var pointIndex = firstPointIndex; pointIndex <= lastPointIndex; pointIndex++)
                    {
                        int count = 0;

                        int y = pointIndex / width;
                        int x = pointIndex % width;

                        //go thru the patch
                        for (int py = y - patchOffs, yi = 0; yi < patchSize; py++, yi++)
                        {
                            for (int px = x - patchOffs, xi = 0; xi < patchSize; px++, xi++)
                            {
                                if ((0 <= py && py < destHeight && 0 <= px && px < destWidth))
                                {
                                    var deltaX = xi - patchOffs;
                                    var deltaY = yi - patchOffs;

                                    var index = (int)*(nnfItemsP + py * width * 2 + px * 2);
                                    var sourcePixelPosX = index % nnf.SourceWidth - deltaX;
                                    var sourcePixelPosY = index / nnf.SourceWidth - deltaY;

                                    if ((0 <= sourcePixelPosY && sourcePixelPosY < sourceHeight &&
                                         0 <= sourcePixelPosX && sourcePixelPosX < sourceWidth))
                                    {
                                        var ppos = (sourcePixelPosY * (int)sourceWidth + sourcePixelPosX) * componentsCount;
                                        for (int j = 0; j < componentsCount; j++)
                                        {
                                            *(colorP + j) += *(srcPixelsP + ppos + j);
                                        }
                                        count++;
                                    }
                                }
                            }
                        }

                        var pos = (y * width + x) * componentsCount;
                        for (int j = 0; j < componentsCount; j++)
                        {
                            *(resultP + pos + j) = *(colorP + j) / count;
                            *(colorP + j) = 0;
                        }
                    }
                }
            });

            return new ZsImage(result, width, height, componentsCount);
        }
    }
}