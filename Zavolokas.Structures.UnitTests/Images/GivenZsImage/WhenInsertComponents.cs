using System;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Images.GivenZsImage
{
    [TestFixture]
    public class WhenInsertComponents
    {
        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,    // width, height, numberOfComponents
            null, 1,       // components, positionIndex
            ExpectedResult = typeof(ArgumentNullException))]
        public Type Should_Throw_ArgumentNullException_When_Components_IsNull(double[] pixels, int width, int height, byte componentsAmount, double[] components, byte index)
        {
            var image = new ZsImage(pixels, width, height, componentsAmount);

            var result = typeof(object);
            try
            {
                image.InsertComponents(components, index);
            }
            catch (Exception ex)
            {
                result = ex.GetType();
            }
            return result;
        }

        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,    // width, height, numberOfComponents
            new double[] { }, 1,       // components, positionIndex
            ExpectedResult = typeof(ArgumentOutOfRangeException))]
        public Type Should_Throw_ArgumentOutOfRangeException_When_ComponentsLength_IsZero(double[] pixels, int width, int height, byte componentsAmount, double[] components, byte index)
        {
            var image = new ZsImage(pixels, width, height, componentsAmount);

            var result = typeof(object);
            try
            {
                image.InsertComponents(components, index);
            }
            catch (Exception ex)
            {
                result = ex.GetType();
            }
            return result;
        }

        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,    // width, height, numberOfComponents
            new[] { 1.0 }, 4,       // components, positionIndex
            ExpectedResult = typeof(ArgumentOutOfRangeException))]
        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00,  0.50,
                0.25, 0.25, 0.25,  0.125,
            },
            2, 2, 2,    // width, height, numberOfComponents
            new[] { 1.0 }, 3,       // components, positionIndex
            ExpectedResult = typeof(ArgumentOutOfRangeException))]
        public Type Should_Throw_ArgumentOutOfRangeException_Index_Greater_Than_Components_Amount(double[] pixels, int width, int height, byte componentsAmount, double[] components, byte index)
        {
            var image = new ZsImage(pixels, width, height, componentsAmount);

            var result = typeof(object);
            try
            {
                image.InsertComponents(components, index);
            }
            catch (Exception ex)
            {
                result = ex.GetType();
            }
            return result;
        }


        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,    // width, height, numberOfComponents
            new[] { 1.0 }, 3,       // components, positionIndex
            ExpectedResult = typeof(object))]

        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,    // width, height, numberOfComponents
            new[] { 0.0 }, 0,       // components, positionIndex
            ExpectedResult = typeof(object))]

        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,    // width, height, numberOfComponents
            new[] { 0.5 }, 1,       // components, positionIndex
            ExpectedResult = typeof(object))]

        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00,  0.50,
                0.25, 0.25, 0.25,  0.125,
            },
            2, 2, 2,    // width, height, numberOfComponents
            new[] { 1.0 }, 2,       // components, positionIndex
            ExpectedResult = typeof(object))]
        public Type Should_Not_Throw_ArgumentOutOfRangeException(double[] pixels, int width, int height, byte componentsAmount, double[] components, byte index)
        {
            var image = new ZsImage(pixels, width, height, componentsAmount);

            var result = typeof(object);
            try
            {
                image.InsertComponents(components, index);
            }
            catch (Exception ex)
            {
                result = ex.GetType();
            }
            return result;
        }

        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00, 0.50,
                0.25, 0.25, 0.25, 0.125,
            },
            2, 2, 2, // width, height, numberOfComponents
            new[] { 1.0 }, 2, // components, positionIndex
            ExpectedResult = 12)]
        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00, 0.50,
                0.25, 0.25, 0.25, 0.125,
            },
            2, 2, 2, // width, height, numberOfComponents
            new[] { 1.0, 0.5, 0.3 }, 0, // components, positionIndex
            ExpectedResult = 20)]
        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00, 0.50, 0.50, 0.50,
                0.25, 0.25, 0.25, 0.125, 0.125, 0.125
            },
            2, 2, 3, // width, height, numberOfComponents
            new[] { 1.0 }, 0, // components, positionIndex
            ExpectedResult = 16)]
        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00, 0.50, 0.50, 0.50,
                0.25, 0.25, 0.25, 0.125, 0.125, 0.125
            },
            2, 2, 3, // width, height, numberOfComponents
            new[] { 1.0, 0.0 }, 0, // components, positionIndex
            ExpectedResult = 20)]
        public int Should_Increase_PixelsData_Array(double[] pixels, int width, int height, byte componentsAmount,
            double[] components, byte position)
        {
            var image = new ZsImage(pixels, width, height, componentsAmount);
            image.InsertComponents(components, position);
            return image.PixelsData.Length;
        }


        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00, 0.50,
                0.25, 0.25, 0.25, 0.125,
            },
            2, 2, 2, // width, height, numberOfComponents
            new[] { 1.0 }, 2, // components, positionIndex
            ExpectedResult = 3)]
        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00, 0.50,
                0.25, 0.25, 0.25, 0.125,
            },
            2, 2, 2, // width, height, numberOfComponents
            new[] { 1.0, 1, 1 }, 0, // components, positionIndex
            ExpectedResult = 5)]
        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00, 0.50, 0.50, 0.50,
                0.25, 0.25, 0.25, 0.125, 0.125, 0.125
            },
            2, 2, 3, // width, height, numberOfComponents
            new[] { 1.0 }, 0, // components, positionIndex
            ExpectedResult = 4)]
        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00, 0.50, 0.50, 0.50,
                0.25, 0.25, 0.25, 0.125, 0.125, 0.125
            },
            2, 2, 3, // width, height, numberOfComponents
            new[] { 1.0, 0 }, 0, // components, positionIndex
            ExpectedResult = 5)]
        public int Should_Increase_NumberOfComponents(double[] pixels, int width, int height, byte componentsAmount,
            double[] components, byte position)
        {
            var image = new ZsImage(pixels, width, height, componentsAmount);
            image.InsertComponents(components, position);
            return image.NumberOfComponents;
        }


        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00, 0.50,
                0.25, 0.25, 0.25, 0.125,
            },
            2, 2, 2, // width, height, numberOfComponents
            new[] { 1.0 }, 2, // components, positionIndex
            ExpectedResult = 6)]
        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00, 0.50,
                0.25, 0.25, 0.25, 0.125,
            },
            2, 2, 2, // width, height, numberOfComponents
            new[] { 1.0, 1, 1 }, 0, // components, positionIndex
            ExpectedResult = 10)]
        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00, 0.50, 0.50, 0.50,
                0.25, 0.25, 0.25, 0.125, 0.125, 0.125
            },
            2, 2, 3, // width, height, numberOfComponents
            new[] { 1.0 }, 0, // components, positionIndex
            ExpectedResult = 8)]
        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00, 0.50, 0.50, 0.50,
                0.25, 0.25, 0.25, 0.125, 0.125, 0.125
            },
            2, 2, 3, // width, height, numberOfComponents
            new[] { 1.0, 0 }, 0, // components, positionIndex
            ExpectedResult = 10)]
        public int Should_Increase_Stride(double[] pixels, int width, int height, byte componentsAmount,
            double[] components, byte position)
        {
            var image = new ZsImage(pixels, width, height, componentsAmount);
            image.InsertComponents(components, position);
            return image.Stride;
        }

        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,                // width, height, numberOfComponents
            new[] { 1.0 }, 0,       // components, positionIndex
            new[]
            {
                1.00, 1.00, 1.00, 1.00,  1.00, 0.50, 0.50, 0.50,
                1.00, 0.25, 0.25, 0.25,  1.00, 0.125, 0.125, 0.125
            },
            ExpectedResult = true)]

        [TestCase(
            new[]
            {
                1.00, 2.00, 3.00,  4.50, 5.50, 6.50,
                7.25, 8.25, 9.25,  10.125, 11.125, 12.125
            },
            2, 2, 3,                // width, height, numberOfComponents
            new[] { 0.0 }, 0,       // components, positionIndex
            new[]
            {
                0, 1.00, 2.00, 3.00,  0, 4.50, 5.50, 6.50,
                0, 7.25, 8.25, 9.25,  0, 10.125, 11.125, 12.125
            },
            ExpectedResult = true)]

        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,                // width, height, numberOfComponents
            new[] { 1.0 }, 1,       // components, positionIndex
            new[]
            {
                1.00, 1.00, 1.00, 1.00,  0.50, 1.00, 0.50, 0.50,
                0.25, 1.00, 0.25, 0.25,  0.125, 1.00, 0.125, 0.125
            },
            ExpectedResult = true)]

        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,                // width, height, numberOfComponents
            new[] { 1.0 }, 3,       // components, positionIndex
            new[]
            {
                1.00, 1.00, 1.00, 1.00,   0.50, 0.50, 0.50, 1.00,
                0.25, 0.25, 0.25, 1.00,   0.125, 0.125, 0.125, 1.00
            },
            ExpectedResult = true)]

        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,                // width, height, numberOfComponents
            new[] { 0.5, 0.3 }, 1,       // components, positionIndex
            new[]
            {
                1.00, 0.50, 0.30, 1.00, 1.00,   0.50, 0.50, 0.30, 0.50, 0.50,
                0.25, 0.50, 0.30, 0.25, 0.25,   0.125, 0.50, 0.30, 0.125, 0.125
            },
            ExpectedResult = true)]

        [TestCase(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,                // width, height, numberOfComponents
            new[] { 0.1, 0.5, 0.3 }, 2,       // components, positionIndex
            new[]
            {
                1.00, 1.00, 0.10, 0.50, 0.30, 1.00,   0.50, 0.50, 0.10, 0.50, 0.30, 0.50,
                0.25, 0.25, 0.10, 0.50, 0.30, 0.25,   0.125, 0.125, 0.10, 0.50, 0.30, 0.125
            },
            ExpectedResult = true)]
        public bool Should_Insert_Provided_Values(double[] pixels, int width, int height, byte componentsAmount, double[] components, byte index, double[] result)
        {
            var image = new ZsImage(pixels, width, height, componentsAmount);
            image.InsertComponents(components, index);
            var pixels2 = image.PixelsData;

            var equal = result.Length == pixels2.Length;

            for (int i = 0; i < result.Length && equal; i++)
            {
                if (result[i] != pixels2[i])
                    equal = false;
            }

            return equal;
        }
    }
}
