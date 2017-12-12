using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Zavolokas.Structures
{
    public static class BinaryMarkupExtensions
    {
        public static void Dilation(this byte[][] binaryMarkup, int kernelSize)
        {
            Debug.Assert(kernelSize > 0, "Size should be > 0 while Dilation");
            Debug.Assert(kernelSize % 2 == 1, "Size sould be odd while dilation");

            var frontier = CloneJaggedArray(binaryMarkup);
            LeaveFrontier(frontier);

            var width = frontier[0].Length;
            var height = frontier.Length;
            var frontierArea = Area2D.Create(0, 0, frontier);
            var frontierPointIndices = new int[frontierArea.ElementsCount];
            frontierArea.FillMappedPointsIndexes(frontierPointIndices, width);

            int offset = (kernelSize - 1) / 2;

            var rangePartitioner = Partitioner.Create(0, frontierArea.ElementsCount);

            Parallel.ForEach(rangePartitioner, range =>
            {
                for (int frontierPointIndex = range.Item1; frontierPointIndex < range.Item2; frontierPointIndex++)
                {
                    var globalPointIndex = frontierPointIndices[frontierPointIndex];
                    var y = globalPointIndex / width;
                    var x = globalPointIndex % width;

                    for (int ky = 0, iy = y - offset; ky < kernelSize; ky++, iy++)
                    {
                        if (iy < 0)
                            continue;
                        if (iy >= height)
                            break;

                        for (int kx = 0, ix = x - offset; kx < kernelSize; kx++, ix++)
                        {
                            if (ix < 0)
                                continue;

                            if (ix >= width)
                                break;

                            binaryMarkup[iy][ix] = 1;
                        }

                    }
                }
            });
        }

        private static void LeaveFrontier(byte[][] binaryMarkup)
        {
            const int insideColor = 128;

            var rangePartitioner = Partitioner.Create(0, binaryMarkup.Length);

            Parallel.ForEach(rangePartitioner, range =>
                    {
                        //for (int y = 0; y < jaggedArray.Length; y++)
                        for (int y = range.Item1; y < range.Item2; y++)
                        {
                            byte[] row = binaryMarkup[y];
                            for (int x = 0; x < row.Length; x++)
                            {
                                if (row[x] != 0)
                                {
                                    bool hasLeftSpace = (x == 0 || row[x - 1] == 0);
                                    bool hasRightSpace = (x == row.Length - 1 ||
                                                            row[x + 1] == 0);
                                    bool hasTopSpace = (y == 0 ||
                                                        binaryMarkup[y - 1][x] == 0);
                                    bool hasBottomSpace = (y == binaryMarkup.Length - 1 ||
                                                            binaryMarkup[y + 1][x] == 0);
                                    if (
                                        !(hasLeftSpace || hasRightSpace || hasTopSpace ||
                                            hasBottomSpace))
                                        row[x] = insideColor;
                                }
                            }
                        }
                    });

            Parallel.ForEach(rangePartitioner, range =>
                    {
                        for (int y = range.Item1; y < range.Item2; y++)
                        {
                            byte[] row = binaryMarkup[y];
                            for (int x = 0; x < row.Length; x++)
                            {
                                if (row[x] == insideColor)
                                    row[x] = 0;
                            }
                        }
                    });
        }

        private static T[][] CloneJaggedArray<T>(T[][] source)
        {
            int height = source.Length;
            int width = source[0].Length;

            var clone = new T[height][];

            for (int y = 0; y < height; y++)
            {
                clone[y] = new T[width];
                source[y].CopyTo(clone[y], 0);
            }

            return clone;
        }
    }
}
