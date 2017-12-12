using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zavolokas.Structures
{
    public static class Area2DExtensions
    {
        /// <summary>
        /// Calculates the confidence value of each point of the area.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <param name="confidentValue">The confident value.</param>
        /// <param name="gamma">The gamma.</param>
        /// <returns></returns>
        public static double[] CalculatePointsConfidence(this Area2D area, double confidentValue, double gamma)
        {
            area = area.Translate(-area.Bound.X, -area.Bound.Y);

            var result = new double[area.ElementsCount];

            const double a = 1.0;
            const double b = 1.5;
            var vals = new double[4];
            var width = area.Bound.Width;
            var height = area.Bound.Height;

            var pointIndexes = new int[area.ElementsCount];
            area.FillMappedPointsIndexes(pointIndexes, width);
            var pointIndexMap = new Dictionary<int, int>(pointIndexes.Length);
            for (var i = 0; i < pointIndexes.Length; i++)
            {
                pointIndexMap.Add(pointIndexes[i], i);
            }

            for (var i = 0; i < pointIndexes.Length; i++)
            {
                var x = pointIndexes[i] % width;
                var y = pointIndexes[i] / width;
                var lx = x - 1;
                var ty = y - 1;
                var rx = x + 1;

                int[][] cs =
                {
                    new [] {lx, ty},
                    new [] {x, ty},
                    new [] {rx, ty},
                    new [] {lx, y},
                };

                var ads = new[] { b, a, b, a };

                for (int j = 0; j < vals.Length; j++)
                {
                    int cx = cs[j][0];
                    int cy = cs[j][1];

                    if (0 <= cx && cx < width && 0 <= cy && cy < height)
                    {
                        var npointIndex = cx + cy * width;
                        var v = 0.0;

                        if (pointIndexMap.ContainsKey(npointIndex))
                        {
                            var index = pointIndexMap[npointIndex];
                            v = result[index];
                        }
                        vals[j] = v + ads[j];
                    }
                    else
                    {
                        vals[j] = a;
                    }
                }

                result[i] = vals.Min();
            }

            vals = new double[5];
            for (var i = pointIndexes.Length - 1; i >= 0; i--)
            {
                var x = pointIndexes[i] % width;
                var y = pointIndexes[i] / width;
                var lx = x - 1;
                var by = y + 1;
                var rx = x + 1;

                int[][] cs =
                {
                    new [] {lx, by},
                    new [] {x, by},
                    new [] {rx, by},
                    new [] {rx, y},
                };

                var ads = new[] { b, a, b, a };

                for (int j = 0; j < vals.Length - 1; j++)
                {
                    int cx = cs[j][0];
                    int cy = cs[j][1];

                    if (0 <= cx && cx < width && 0 <= cy && cy < height)
                    {
                        var npointIndex = cx + cy * width;
                        var v = 0.0;

                        if (pointIndexMap.ContainsKey(npointIndex))
                        {
                            var index = pointIndexMap[npointIndex];
                            v = result[index];
                        }
                        vals[j] = v + ads[j];
                    }
                    else
                    {
                        vals[j] = a;
                    }
                }
                vals[4] = result[i];

                result[i] = vals.Min();
            }

            var gammaMinusOne = System.Math.Pow(gamma, -1);

            var rangePartitioner = Partitioner.Create(0, result.Length);

            Parallel.ForEach(rangePartitioner, range =>
            {
                for (var i = range.Item1; i < range.Item2; i++)
                {
                    if (result[i] == 0)
                    {
                        result[i] = confidentValue;
                    }
                    else
                    {
                        result[i] = System.Math.Pow(gammaMinusOne, result[i]);
                    }
                }
            });

            return result;
        }

        public static Area2D Dilation(this Area2D area, int kernelSize)
        {
            var bound = area.Bound;
            var margin = (kernelSize - 1) / 2;

            // Convert area to a jagged array with a margin
            byte[][] mrkp = new byte[2 * margin + bound.Height][];
            for (int y = 0; y < mrkp.Length; y++)
            {
                byte[] row = new byte[2 * margin + bound.Width];
                mrkp[y] = row;
            }

            for (int i = 0; i < area.ElementsCount; i++)
            {
                var p = area[i];
                mrkp[margin + (p.Y - area.Bound.Y)][margin + (p.X - area.Bound.X)] = 1;
            }

            // To check wether it was converted correctly we can convert array back to area:
            // var ar = Area2D.Create(area.Bound.X - margin, area.Bound.Y - margin, mrkp);
            // Console.WriteLine(area);
            // Console.WriteLine(ar);

            mrkp.Dilation(kernelSize);
            var result = Area2D.Create(area.Bound.X - margin, area.Bound.Y - margin, mrkp);
            return result;
        }

        public static byte[][] ToBinaryMarkup(this Area2D area)
        {
            byte[][] result = new byte[area.Bound.Height][];
            for (int y = 0; y < result.Length; y++)
            {
                byte[] row = new byte[area.Bound.Width];
                result[y] = row;
            }

            for (int i = 0; i < area.ElementsCount; i++)
            {
                var p = area[i];
                result[p.Y - area.Bound.Y][p.X - area.Bound.X] = 1;
            }

            return result;
        }
    }
}
