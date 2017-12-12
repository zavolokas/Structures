using System;
using System.Threading.Tasks;
using SMath = System.Math;

namespace Zavolokas.Structures
{
    public static partial class ImageExtensions
    {
        public static ZsImage FromGrayToRgb(this ZsImage grayImage)
        {
            GrayToRgb(grayImage);
            return grayImage;
        }

        public static ZsImage FromRgbToGray(this ZsImage rgbImage)
        {
            RgbToGray(rgbImage);
            return rgbImage;
        }

        public static ZsImage FromLabToRgb(this ZsImage image)
        {
            LabToRgb(image);
            return image;
        }

        public static ZsImage FromRgbToLab(this ZsImage image)
        {
            RgbToLab(image);
            return image;
        }

        public static ZsImage FromArgbToRgb(this ZsImage argbImage, double[] background)
        {
            return argbImage
                        .MergeAlphaWithBackground(background)
                        .RemoveComponents(0);
        }

        public static ZsImage FromRgbToArgb(this ZsImage rgbImage, Area2D opaqueArea)
        {
            return rgbImage.InsertComponents(new[] { 0.0 }, 0)
                           .SetComponentsValues(opaqueArea, new[] { 1.0 }, 0);
        }

        // ===========================

        //All calculations are from here http://www.easyrgb.com/index.php?X=MATH&H=08#text8
        // http://www.brucelindbloom.com/index.html?Math.html can be useful as well.

        private const double e = 0.008856;
        private const double k = 7.787;
        private const double S = 16.0 / 116.0;
        private const double S2 = 1.0 / 2.4;
        private const double OneThird = 1.0 / 3.0;

        private static unsafe void RgbToGray(ZsImage rgbImage)
        {
            if (rgbImage == null)
                throw new ArgumentNullException();

            const int NotDividableMinAmountElements = 80;

            double[] pixelsData = rgbImage.PixelsData;

            int pointsAmount = rgbImage.Width * rgbImage.Height;

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

                fixed (double* pixelsDataP = pixelsData)
                {
                    for (int pointIndex = lastPointIndex; pointIndex >= firstPointIndex; pointIndex--)
                    {
                        int i = pointIndex * 3;

                        // components should be in the range [0.0 , 1.0]
                        double R = *(pixelsDataP + i + 0);
                        double G = *(pixelsDataP + i + 1);
                        double B = *(pixelsDataP + i + 2);

                        *(pixelsDataP + i + 0) = R * 0.2989 + G * 0.5870 + B * 0.1140;
                    }
                }
            });

            rgbImage.RemoveComponents(1, 2);
        }

        private static unsafe void GrayToRgb(ZsImage grayImage)
        {
            if (grayImage == null)
                throw new ArgumentNullException();

            grayImage.InsertComponents(new[] {0.0, 0.0}, 1);

            const int NotDividableMinAmountElements = 80;

            double[] pixelsData = grayImage.PixelsData;

            int pointsAmount = grayImage.Width * grayImage.Height;

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

                fixed (double* pixelsDataP = pixelsData)
                {
                    for (int pointIndex = lastPointIndex; pointIndex >= firstPointIndex; pointIndex--)
                    {
                        // Step 1. Inverse Companding
                        int i = pointIndex * 3;

                        // components should be in the range [0.0 , 1.0]
                        double gray = *(pixelsDataP + i + 0);

                        //*(pixelsDataP + i + 0) = gray;
                        *(pixelsDataP + i + 1) = gray;
                        *(pixelsDataP + i + 2) = gray;
                    }
                }
            });
        }

        private static unsafe void RgbToLab(ZsImage rgbImage)
        {
            if (rgbImage == null)
                throw new ArgumentNullException();

            const double K1 = 1.0 / 1.055;
            const double K2 = 1.0 / 12.92;

            const double Xr = 1.0 / 95.047;
            const double Yr = 1.0 / 100.0;
            const double Zr = 1.0 / 108.883;

            const int NotDividableMinAmountElements = 80;

            double[] pixelsData = rgbImage.PixelsData;

            int pointsAmount = rgbImage.Width * rgbImage.Height;

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

                fixed (double* pixelsDataP = pixelsData)
                {
                    for (int pointIndex = lastPointIndex; pointIndex >= firstPointIndex; pointIndex--)
                    {
                        // Step 1. Inverse Companding
                        int i = pointIndex * 3;

                        // components should be in the range [0.0 , 1.0]
                        double R = *(pixelsDataP + i + 0);
                        double G = *(pixelsDataP + i + 1);
                        double B = *(pixelsDataP + i + 2);

                        // Inverse sRBG Companding http://www.brucelindbloom.com/index.html?Math.html
                        var r = R > 0.04045 ? SMath.Pow((R + 0.055) * K1, 2.4) * 100.0 : R * K2 * 100.0;
                        var g = G > 0.04045 ? SMath.Pow((G + 0.055) * K1, 2.4) * 100.0 : G * K2 * 100.0;
                        var b = B > 0.04045 ? SMath.Pow((B + 0.055) * K1, 2.4) * 100.0 : B * K2 * 100.0;

                        // Step 2. Linear RGB to XYZ
                        double xr = (r * 0.4124 + g * 0.3576 + b * 0.1805) * Xr;
                        double yr = (r * 0.2126 + g * 0.7152 + b * 0.0722) * Yr;
                        double zr = (r * 0.0193 + g * 0.1192 + b * 0.9505) * Zr;

                        double fx = xr > e ? SMath.Pow(xr, OneThird) : (k * xr) + S;
                        double fy = yr > e ? SMath.Pow(yr, OneThird) : (k * yr) + S;
                        double fz = zr > e ? SMath.Pow(zr, OneThird) : (k * zr) + S;

                        *(pixelsDataP + i + 0) = 116.0 * fy - 16.0;
                        *(pixelsDataP + i + 1) = 500.0 * (fx - fy);
                        *(pixelsDataP + i + 2) = 200.0 * (fy - fz);
                    }
                }
            });
        }

        public static unsafe ZsImage SetComponentsValues(this ZsImage image, Area2D opaqueArea, double[] values, byte index)
        {
            if (image == null)
                throw new ArgumentNullException();

            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (values.Length == 0)
                throw new ArgumentException("values can not be empty", nameof(values));

            if (index >= image.NumberOfComponents)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (opaqueArea == null)
                throw new ArgumentNullException(nameof(opaqueArea));

            const int NotDividableMinAmountElements = 80;

            var imageArea = Area2D.Create(0, 0, image.Width, image.Height);
            opaqueArea = opaqueArea.Intersect(imageArea);
            if (!opaqueArea.IsEmpty)
            {
                byte componentsAmount = image.NumberOfComponents;
                int pointsAmount = opaqueArea.ElementsCount;
                int valuesAmount = componentsAmount >= index + values.Length
                    ? values.Length
                    : values.Length - ((index + values.Length) - componentsAmount);

                double[] pixelsData = image.PixelsData;

                var pointIndices = new int[opaqueArea.ElementsCount];
                opaqueArea.FillMappedPointsIndexes(pointIndices, image.Width);

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

                    fixed (double* pixelsDataP = pixelsData)
                    {
                        for (int pointIndex = lastPointIndex; pointIndex >= firstPointIndex; pointIndex--)
                        {
                            int i = pointIndices[pointIndex] * componentsAmount;

                            for (int j = 0; j < valuesAmount; j++)
                            {
                                *(pixelsDataP + i + j + index) = values[j];
                            }
                        }
                    }
                });
            }
            return image;
        }

        private static unsafe void LabToRgb(ZsImage labImage)
        {
            //LabToXyz(ref labImage);
            if (labImage == null)
                throw new ArgumentNullException();

            double[] pixelsData = labImage.PixelsData;

            const int NotDividableMinAmountElements = 80;
            const double Xr = 95.047;
            const double Yr = 100.000;
            const double Zr = 108.883;

            const double K1 = 1.0 / 116.0;
            const double K2 = 1.0 / 500.0;
            const double K3 = 1.0 / 200.0;
            const double K4 = 1.0 / k;
            const double W = 0.0031308;

            const double Xr2 = 1.0 / 100.0;
            const double Yr2 = 1.0 / 100.0;
            const double Zr2 = 1.0 / 100.0;

            int pointsAmount = labImage.Width * labImage.Height;

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

                fixed (double* pixelsDataP = pixelsData)
                {
                    for (int pointIndex = lastPointIndex; pointIndex >= firstPointIndex; pointIndex--)
                    {
                        // Step 1. Inverse Companding
                        int i = pointIndex * 3;

                        double L = *(pixelsDataP + i + 0);
                        double fy = (L + 16.0) * K1;
                        double fx = (*(pixelsDataP + i + 1) * K2) + fy;
                        double fz = fy - (*(pixelsDataP + i + 2) * K3);

                        var yr = fy * fy * fy;
                        if (yr <= e) yr = (fy - S) * K4;
                        var xr = fx * fx * fx;
                        if (xr <= e) xr = (fx - S) * K4;
                        var zr = fz * fz * fz;
                        if (zr <= e) zr = (fz - S) * K4;

                        double X = xr * Xr * Xr2;
                        double Y = yr * Yr * Yr2;
                        double Z = zr * Zr * Zr2;

                        // Step 1.  XYZ to Linear RGB
                        double r = X * 3.2406 + Y * -1.5372 + Z * -0.4986;
                        double g = X * -0.9689 + Y * 1.8758 + Z * 0.0415;
                        double b = X * 0.0557 + Y * -0.2040 + Z * 1.0570;

                        // Step 2.  Companding
                        // sRGB Companding
                        *(pixelsDataP + i + 0) = r > W ? 1.055 * SMath.Pow(r, S2) - 0.055 : 12.92 * r;
                        *(pixelsDataP + i + 1) = g > W ? 1.055 * SMath.Pow(g, S2) - 0.055 : 12.92 * g;
                        *(pixelsDataP + i + 2) = b > W ? 1.055 * SMath.Pow(b, S2) - 0.055 : 12.92 * b;
                    }
                }
            });
        }

        private static unsafe ZsImage MergeAlphaWithBackground(this ZsImage argbImage, double[] background)
        {
            if (argbImage == null)
                throw new ArgumentNullException();

            const int NotDividableMinAmountElements = 80;

            int pointsAmount = argbImage.Width * argbImage.Height;

            double[] pixelsData = argbImage.PixelsData;

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

                fixed (double* pixelsDataP = pixelsData)
                {
                    double alpha = 0.0;
                    double nalpha = 1.0;

                    for (int pointIndex = lastPointIndex; pointIndex >= firstPointIndex; pointIndex--)
                    {
                        int i = pointIndex * 4;

                        // components should be in the range [0.0 , 1.0]
                        alpha = *(pixelsDataP + i);
                        nalpha = 1.0 - alpha;
                        *(pixelsDataP + i + 1) = alpha * *(pixelsDataP + i + 1) + nalpha * background[0];
                        *(pixelsDataP + i + 2) = alpha * *(pixelsDataP + i + 2) + nalpha * background[1];
                        *(pixelsDataP + i + 3) = alpha * *(pixelsDataP + i + 3) + nalpha * background[2];
                    }
                }
            });

            return argbImage;
        }
    }
}
