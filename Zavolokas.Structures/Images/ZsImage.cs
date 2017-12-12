using System;
using System.Linq;
using System.Threading.Tasks;

namespace Zavolokas.Structures
{
    // 
    // Width > 0
    // Height > 0
    // NumberOfCOmponents > 0
    // Stride > 0
    // Stride == width * number of components
    // PixelsData != null
    // PixelsData.Length >= Stride * Height
    // PixelsData can be changed only by ColorSpace changing operation or Scaling
    // 
    // ? _pixelsData array can be bigger than it is required
    // ? PixelsData returns a copy of data & in order to apply changes the copy should be assigned to PixelsData and it will copy that?
    public sealed class ZsImage
    {
        const int NotDividableMinAmountElements = 80;
        const int ThreadsCount = 4;

        private double[] _pixelsData;

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int Stride { get; private set; }

        public byte NumberOfComponents { get; private set; }

        public double[] PixelsData { get { return _pixelsData; } }

        public ZsImage(double[] pixelsData, int width, int height, byte numberOfComponents)
        {
            if (pixelsData == null)
                throw new ArgumentNullException($"Argument '{nameof(pixelsData)}' can not be Null when constructing an image");

            int expectedPixelsLength = width * height * numberOfComponents;
            if (pixelsData.Length != expectedPixelsLength)
                throw new ArgumentException($"Expected pixels length is {expectedPixelsLength}, but actual is {pixelsData.Length}");

            _pixelsData = pixelsData;
            NumberOfComponents = numberOfComponents;
            Stride = width * numberOfComponents;
            Width = width;
            Height = height;
        }

        public ZsImage RemoveComponents(params byte[] indecies)
        {
            if (indecies == null)
                throw new ArgumentNullException(nameof(indecies));

            if (indecies.Any(i => i >= NumberOfComponents))
                throw new ArgumentOutOfRangeException(nameof(indecies), $"One of the '{nameof(indecies)}' exceed the components.");

            {
                var indeciesList = indecies
                    .Distinct()
                    .ToList();
                indeciesList.Sort();
                indecies = indeciesList.ToArray();
            }

            var numberOfComponents = (byte)(NumberOfComponents - indecies.Length);

            if (numberOfComponents == 0)
                throw new ArgumentException(nameof(indecies), "It is not allowd to remove all the components.");

            var stride = Width * numberOfComponents;
            var pixelsData = new double[Height * stride];
            var pixelsAmount = Height * Width;

            byte[] toLeave = new byte[numberOfComponents];
            for (byte i = 0, j = 0; i < NumberOfComponents; i++)
            {
                if (!indecies.Contains(i)) toLeave[j++] = i;
            }

            // Decide on how many partitions we should divade the processing
            // of the elements.
            var partsCount = pixelsAmount > NotDividableMinAmountElements
                ? ThreadsCount
                : 1;
            var partSize = (int)(pixelsAmount / partsCount);

            //for (int partIndex = 0; partIndex < partsCount; partIndex++)
            Parallel.For(0, partsCount, partIndex =>
            {
                var firstPointIndex = partIndex * partSize;
                var lastPointIndex = firstPointIndex + partSize - 1;
                if (partIndex == partsCount - 1) lastPointIndex = pixelsAmount - 1;
                if (lastPointIndex > pixelsAmount) lastPointIndex = pixelsAmount;

                for (int pointIndex = firstPointIndex; pointIndex <= lastPointIndex; pointIndex++)
                {
                    var srcPixPosition = pointIndex * NumberOfComponents;
                    var destPixPosition = pointIndex * numberOfComponents;

                    for (int ci = 0; ci < toLeave.Length; ci++)
                    {
                        pixelsData[destPixPosition + ci] = _pixelsData[srcPixPosition + toLeave[ci]];
                    }
                }
            });

            NumberOfComponents = numberOfComponents;
            Stride = stride;
            _pixelsData = pixelsData;

            return this;
        }

        public ZsImage InsertComponents(double[] components, byte position)
        {
            if (components == null)
                throw new ArgumentNullException(nameof(components));

            if (components.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(components));

            if (NumberOfComponents < position)
                throw new ArgumentOutOfRangeException(nameof(position));

            var numberOfComponents = (byte)(NumberOfComponents + components.Length);
            var stride = Width * numberOfComponents;
            var pixelsData = new double[Height * stride];
            var pixelsAmount = Height * Width;

            // Decide on how many partitions we should divade the processing
            // of the elements.
            var partsCount = pixelsAmount > NotDividableMinAmountElements
                ? ThreadsCount
                : 1;
            var partSize = (int)(pixelsAmount / partsCount);

            //for (int partIndex = 0; partIndex < partsCount; partIndex++)
            Parallel.For(0, partsCount, partIndex =>
            {
                var firstPointIndex = partIndex * partSize;
                var lastPointIndex = firstPointIndex + partSize - 1;
                if (partIndex == partsCount - 1) lastPointIndex = pixelsAmount - 1;
                if (lastPointIndex > pixelsAmount) lastPointIndex = pixelsAmount;

                for (int i = firstPointIndex; i <= lastPointIndex; i++)
                {
                    int newIndex = i * numberOfComponents;
                    int oldIndex = i * NumberOfComponents;
                    int dt = 0;

                    for (int j = 0; j < NumberOfComponents; j++)
                    {
                        if (j == position)
                        {
                            dt = components.Length;
                            for (int k = 0; k < components.Length; k++)
                            {
                                pixelsData[newIndex + j + k] = components[k];
                            }
                        }

                        pixelsData[newIndex + j + dt] = _pixelsData[oldIndex + j];
                    }

                    if (position == NumberOfComponents)
                    {
                        for (int k = 0; k < components.Length; k++)
                        {
                            pixelsData[newIndex + NumberOfComponents + k] = components[k];
                        }
                    }
                }
            }
            );
            NumberOfComponents = numberOfComponents;
            Stride = stride;
            _pixelsData = pixelsData;

            return this;
        }

        public ZsImage ScaleDown2x()
        {
            var pixelsData = this.PixelsData;
            var componentsAmount = this.NumberOfComponents;
            var width = this.Width;
            var height = this.Height;

            var pixelsData05x = new double[pixelsData.Length / 4];

            var partsCount = (pixelsData05x.Length / componentsAmount) > NotDividableMinAmountElements
                ? ThreadsCount
                : 1;
            int partHeight = height / partsCount;

            Parallel.For(0, partsCount, partIndex =>
            //for (int partIndex = 0; partIndex < partsCount; partIndex++)
            {
                int startYPos = partHeight * partIndex;
                int endYPos = startYPos + partHeight - 1;

                if (partIndex == partsCount - 1) endYPos = height - 1;
                if (endYPos > height) endYPos = height - 1;

                int ci = (startYPos / 2) * (width / 2) * componentsAmount;

                for (int y = startYPos; y <= endYPos; y += 2)
                {
                    for (int x = 0; x < width; x += 2, ci += componentsAmount)
                    {
                        int i = (y * width + x) * componentsAmount;

                        for (int j = 0; j < componentsAmount; j++)
                        {
                            pixelsData05x[ci + j] = pixelsData[i + j];
                        }
                    }
                }
            }
            );

            _pixelsData = pixelsData05x;
            Width /= 2;
            Height /= 2;
            return this;
        }

        public ZsImage ScaleUp2x()
        {
            var width = Width;
            var pixelsData = PixelsData;
            var cmps = NumberOfComponents;

            var pixels2x = new double[pixelsData.Length * 4];

            var partsCount = (pixelsData.Length / cmps) > NotDividableMinAmountElements
                ? ThreadsCount
                : 1;
            var partSize = (pixelsData.Length / cmps) / partsCount;

            Parallel.For(0, partsCount, partIndex =>
            //for (int partIndex = 0; partIndex < partsCount; partIndex++)
            {
                var pixIndeces = new int[4];
                var firstPointIndex = (partIndex * partSize) * cmps;
                var lastPointIndex = (firstPointIndex + partSize - 1) * cmps;
                if (partIndex == partsCount - 1) lastPointIndex = pixelsData.Length - 1;
                if (lastPointIndex > pixelsData.Length) lastPointIndex = pixelsData.Length - 1;

                for (int i = firstPointIndex; i <= lastPointIndex; i += cmps)
                {
                    var pixIndex = i / cmps;

                    // Calculate indeces of a scaled up pixel

                    //Original Pixels
                    // 0 1
                    // 2 3

                    //Scaled up pixels
                    // 0  1  2  3
                    // 4  5  6  7
                    // 8  9  10 11
                    // 12 13 14 15

                    // Indexes per a pixel
                    // 0 -> 0 1 4 5
                    // 1 -> 2 3 6 7

                    // f(x) = [x / w * w * 2 + x * 2, 
                    //         x / w * w * 2 + x * 2 + 1, 
                    //         x / w * w * 2 + x * 2 + w * 2, 
                    //         x / w * w * 2 + x * 2 + w * 2 + 1
                    // f(3) = 4+6, 4+7, 4+6+4

                    int newWidth = width * 2;
                    int newIndex = pixIndex * 2;
                    int offs = pixIndex / width * newWidth;

                    pixIndeces[0] = offs + newIndex;
                    pixIndeces[1] = offs + newIndex + 1;
                    pixIndeces[2] = offs + newIndex + newWidth;
                    pixIndeces[3] = offs + newIndex + newWidth + 1;

                    for (int j = 0; j < cmps; j++)
                    {
                        var val = pixelsData[i + j];
                        for (int k = 0; k < pixIndeces.Length; k++)
                        {
                            pixels2x[pixIndeces[k] * cmps + j] = val;
                        }
                    }
                }
            });

            _pixelsData = pixels2x;
            Width *= 2;
            Height *= 2;

            return this;
        }

        public ZsImage Clone()
        {
            var pixels = new double[_pixelsData.Length];
            _pixelsData.CopyTo(pixels, 0);

            var clone = new ZsImage(pixels, Width, Height, NumberOfComponents);

            return clone;
        }
    }
}