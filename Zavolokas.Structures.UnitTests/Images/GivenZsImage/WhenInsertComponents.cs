using System;
using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Images.GivenZsImage
{
    public class WhenInsertComponents
    {
        [Theory]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,    // width, height, numberOfComponents
            null, 1,       // components, positionIndex
            typeof(ArgumentNullException))]
        public void Should_Throw_ArgumentNullException_When_Components_IsNull(double[] pixels, int width, int height, byte componentsAmount, double[] components, byte index, Type expectedType)
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
            result.ShouldBe(expectedType);
        }

        [Theory]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,    // width, height, numberOfComponents
            new double[] { }, 1,       // components, positionIndex
            typeof(ArgumentOutOfRangeException))]
        public void Should_Throw_ArgumentOutOfRangeException_When_ComponentsLength_IsZero(double[] pixels, int width, int height, byte componentsAmount, double[] components, byte index, Type expectedType)
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
            result.ShouldBe(expectedType);
        }

        [Theory]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,    // width, height, numberOfComponents
            new[] { 1.0 }, 4,       // components, positionIndex
            typeof(ArgumentOutOfRangeException))]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00,  0.50,
                0.25, 0.25, 0.25,  0.125,
            },
            2, 2, 2,    // width, height, numberOfComponents
            new[] { 1.0 }, 3,       // components, positionIndex
            typeof(ArgumentOutOfRangeException))]
        public void Should_Throw_ArgumentOutOfRangeException_Index_Greater_Than_Components_Amount(double[] pixels, int width, int height, byte componentsAmount, double[] components, byte index, Type expectedType)
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
            result.ShouldBe(expectedType);
        }

        [Theory]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,    // width, height, numberOfComponents
            new[] { 1.0 }, 3,       // components, positionIndex
            typeof(object))]

        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,    // width, height, numberOfComponents
            new[] { 0.0 }, 0,       // components, positionIndex
            typeof(object))]

        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            2, 2, 3,    // width, height, numberOfComponents
            new[] { 0.5 }, 1,       // components, positionIndex
            typeof(object))]

        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00,  0.50,
                0.25, 0.25, 0.25,  0.125,
            },
            2, 2, 2,    // width, height, numberOfComponents
            new[] { 1.0 }, 2,       // components, positionIndex
            typeof(object))]
        public void Should_Not_Throw_ArgumentOutOfRangeException(double[] pixels, int width, int height, byte componentsAmount, double[] components, byte index, Type expectedType)
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
            result.ShouldBe(expectedType);
        }

        [Theory]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00, 0.50,
                0.25, 0.25, 0.25, 0.125,
            },
            2, 2, 2, // width, height, numberOfComponents
            new[] { 1.0 }, 2, // components, positionIndex
            12)]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00, 0.50,
                0.25, 0.25, 0.25, 0.125,
            },
            2, 2, 2, // width, height, numberOfComponents
            new[] { 1.0, 0.5, 0.3 }, 0, // components, positionIndex
            20)]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00, 0.50, 0.50, 0.50,
                0.25, 0.25, 0.25, 0.125, 0.125, 0.125
            },
            2, 2, 3, // width, height, numberOfComponents
            new[] { 1.0 }, 0, // components, positionIndex
            16)]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00, 0.50, 0.50, 0.50,
                0.25, 0.25, 0.25, 0.125, 0.125, 0.125
            },
            2, 2, 3, // width, height, numberOfComponents
            new[] { 1.0, 0.0 }, 0, // components, positionIndex
            20)]
        public void Should_Increase_PixelsData_Array(double[] pixels, int width, int height, byte componentsAmount,
            double[] components, byte position, int result)
        {
            var image = new ZsImage(pixels, width, height, componentsAmount);
            image.InsertComponents(components, position);
            image.PixelsData.Length.ShouldBe(result);
        }

        [Theory]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00, 0.50,
                0.25, 0.25, 0.25, 0.125,
            },
            2, 2, 2, // width, height, numberOfComponents
            new[] { 1.0 }, 2, // components, positionIndex
            3)]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00, 0.50,
                0.25, 0.25, 0.25, 0.125,
            },
            2, 2, 2, // width, height, numberOfComponents
            new[] { 1.0, 1, 1 }, 0, // components, positionIndex
            5)]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00, 0.50, 0.50, 0.50,
                0.25, 0.25, 0.25, 0.125, 0.125, 0.125
            },
            2, 2, 3, // width, height, numberOfComponents
            new[] { 1.0 }, 0, // components, positionIndex
            4)]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00, 0.50, 0.50, 0.50,
                0.25, 0.25, 0.25, 0.125, 0.125, 0.125
            },
            2, 2, 3, // width, height, numberOfComponents
            new[] { 1.0, 0 }, 0, // components, positionIndex
            5)]
        public void Should_Increase_NumberOfComponents(double[] pixels, int width, int height, byte componentsAmount,
            double[] components, byte position, byte result)
        {
            var image = new ZsImage(pixels, width, height, componentsAmount);
            image.InsertComponents(components, position);
            image.NumberOfComponents.ShouldBe(result);
        }

        [Theory]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00, 0.50,
                0.25, 0.25, 0.25, 0.125,
            },
            2, 2, 2, // width, height, numberOfComponents
            new[] { 1.0 }, 2, // components, positionIndex
            6)]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00, 0.50,
                0.25, 0.25, 0.25, 0.125,
            },
            2, 2, 2, // width, height, numberOfComponents
            new[] { 1.0, 1, 1 }, 0, // components, positionIndex
            10)]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00, 0.50, 0.50, 0.50,
                0.25, 0.25, 0.25, 0.125, 0.125, 0.125
            },
            2, 2, 3, // width, height, numberOfComponents
            new[] { 1.0 }, 0, // components, positionIndex
            8)]
        [InlineData(
            new[]
            {
                1.00, 1.00, 1.00, 0.50, 0.50, 0.50,
                0.25, 0.25, 0.25, 0.125, 0.125, 0.125
            },
            2, 2, 3, // width, height, numberOfComponents
            new[] { 1.0, 0 }, 0, // components, positionIndex
            10)]
        public void Should_Increase_Stride(double[] pixels, int width, int height, byte componentsAmount,
            double[] components, byte position, int result)
        {
            var image = new ZsImage(pixels, width, height, componentsAmount);
            image.InsertComponents(components, position);
            image.Stride.ShouldBe(result);
        }

        [Theory]
        [InlineData(
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
            true)]

        [InlineData(
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
            true)]

        [InlineData(
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
            true)]

        [InlineData(
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
            true)]

        [InlineData(
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
            true)]

        [InlineData(
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
            true)]
        public void Should_Insert_Provided_Values(double[] pixels, int width, int height, byte componentsAmount, double[] components, byte index, double[] result, bool expected)
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

            equal.ShouldBe(expected);
        }
    }
}
