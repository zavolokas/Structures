using System;
using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Images.GivenZsImage
{
    public class WhenRemoveComponents
    {
        private double[] _pixelsData;
        private ZsImage _image;

        public WhenRemoveComponents()
        {
            _pixelsData = new[]
            {
                0.00, 1.00, 2.00, 3.00,  0.50, 1.50, 2.50, 3.50,
                0.25, 1.25, 2.25, 3.25,  0.125, 1.125, 2.125, 3.125
            };

            _image = new ZsImage(_pixelsData, 2, 2, 4);
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_Inxecies_IsNull()
        {
            Should.Throw<ArgumentNullException>(() => _image.RemoveComponents(null));
        }

        [Theory]
        [InlineData(new byte[] { 4 })]
        [InlineData(new byte[] { 5, 6 })]
        [InlineData(new byte[] { 40, 41, 42 })]
        [InlineData(new byte[] { 1, 4 })]
        public void Should_Throw_ArgumentOutOfRangeException_When_Index_Greater_Than_Components_Amount(byte[] indecies)
        {
            Should.Throw<ArgumentOutOfRangeException>(() => _image.RemoveComponents(indecies));
        }

        [Theory]
        [InlineData(new byte[] { 1, 2, 3, 4 })]
        [InlineData(new byte[] { 2, 3, 4 })]
        [InlineData(new byte[] { 3, 4 })]
        public void Should_Throw_ArgumentOutOfRangeException_When_RemoveAmount_Exceeds_Pixel_Components(byte[] indecies)
        {
            Should.Throw<ArgumentOutOfRangeException>(() => _image.RemoveComponents(indecies));
        }

        [Fact]
        public void Should_Throw_ArgumentException_When_Attempt_To_Remove_All_Components()
        {
            Should.Throw<ArgumentException>(() => _image.RemoveComponents(0, 1, 2, 3));
        }

        [Theory]
        [InlineData(new byte[] { 0 }, 12)]
        [InlineData(new byte[] { 1 }, 12)]
        [InlineData(new byte[] { 2 }, 12)]
        [InlineData(new byte[] { 3 }, 12)]
        [InlineData(new byte[] { 0, 1 }, 8)]
        [InlineData(new byte[] { 1, 2 }, 8)]
        [InlineData(new byte[] { 2, 3 }, 8)]
        [InlineData(new byte[] { 1, 3 }, 8)]
        [InlineData(new byte[] { 0, 2 }, 8)]
        [InlineData(new byte[] { 0, 1, 2 }, 4)]
        [InlineData(new byte[] { 1, 2, 3 }, 4)]
        [InlineData(new byte[] { 0, 2, 3 }, 4)]
        [InlineData(new byte[] { 0, 1, 3 }, 4)]
        [InlineData(new byte[] { 3, 0, 2 }, 4)]
        [InlineData(new byte[] { 3, 1, 2 }, 4)]
        [InlineData(new byte[] { 3, 3, 3 }, 12)]
        [InlineData(new byte[] { 3, 1, 3 }, 8)]
        public void Should_Reduce_Data_Array_Length(byte[] indecies, int expectedLength)
        {
            _image.RemoveComponents(indecies);
            var pp = _image.PixelsData;
            pp.Length.ShouldBe(expectedLength);
        }

        [Theory]
        [InlineData(new byte[] { 0 }, 6)]
        [InlineData(new byte[] { 1 }, 6)]
        [InlineData(new byte[] { 2 }, 6)]
        [InlineData(new byte[] { 3 }, 6)]
        [InlineData(new byte[] { 0, 1 }, 4)]
        [InlineData(new byte[] { 1, 2 }, 4)]
        [InlineData(new byte[] { 2, 3 }, 4)]
        [InlineData(new byte[] { 0, 1 }, 4)]
        [InlineData(new byte[] { 1, 3 }, 4)]
        [InlineData(new byte[] { 2, 1 }, 4)]
        [InlineData(new byte[] { 3, 0 }, 4)]
        [InlineData(new byte[] { 0, 1, 2 }, 2)]
        [InlineData(new byte[] { 1, 2, 3 }, 2)]
        [InlineData(new byte[] { 3, 1, 2 }, 2)]
        [InlineData(new byte[] { 3, 3, 3 }, 6)]
        [InlineData(new byte[] { 3, 1, 3 }, 4)]
        public void Should_Increase_Stride(byte[] indecies, int expectedStride)
        {
            _image.RemoveComponents(indecies);
            _image.Stride.ShouldBe(expectedStride);
        }

        [Theory]
        [InlineData(new byte[] { 0 }, 3)]
        [InlineData(new byte[] { 1 }, 3)]
        [InlineData(new byte[] { 2 }, 3)]
        [InlineData(new byte[] { 3 }, 3)]
        [InlineData(new byte[] { 0, 1 }, 2)]
        [InlineData(new byte[] { 1, 2 }, 2)]
        [InlineData(new byte[] { 2, 3 }, 2)]
        [InlineData(new byte[] { 0, 1, 2 }, 1)]
        [InlineData(new byte[] { 1, 2, 3 }, 1)]
        [InlineData(new byte[] { 3, 1, 2 }, 1)]
        [InlineData(new byte[] { 3, 3, 3 }, 3)]
        [InlineData(new byte[] { 3, 1, 3 }, 2)]
        public void Should_Decrease_NumberOfComponents(byte[] indecies, byte expectedNumberOfComponents)
        {
            _image.RemoveComponents(indecies);
            _image.NumberOfComponents.ShouldBe(expectedNumberOfComponents);
        }

        [Theory]
        [InlineData(new byte[] { 0 },
            new[]
            {
                1.00, 2.00, 3.00,  1.50, 2.50, 3.50,
                1.25, 2.25, 3.25,  1.125, 2.125, 3.125
            })]
        [InlineData(new byte[] { 1 },
            new[]
            {
                0.00, 2.00, 3.00,  0.50, 2.50, 3.50,
                0.25, 2.25, 3.25,  0.125,2.125, 3.125
            })]
        [InlineData(new byte[] { 2 },
            new[]
            {
                0.00, 1.00, 3.00,  0.50, 1.50, 3.50,
                0.25, 1.25, 3.25,  0.125, 1.125, 3.125
            })]
        [InlineData(new byte[] { 3 },
            new[]
            {
                0.00, 1.00, 2.00,  0.50, 1.50, 2.50,
                0.25, 1.25, 2.25,  0.125, 1.125, 2.125
            })]
        [InlineData(new byte[] { 0, 1 },
            new[]
            {
                2.00, 3.00,  2.50, 3.50,
                2.25, 3.25,  2.125, 3.125
            })]
        [InlineData(new byte[] { 1, 2 },
            new[]
            {
                0.00, 3.00,  0.50, 3.50,
                0.25, 3.25,  0.125, 3.125
            })]
        [InlineData(new byte[] { 2, 3 },
            new[]
            {
                0.00, 1.00,  0.50, 1.50,
                0.25, 1.25,  0.125, 1.125
            })]
        [InlineData(new byte[] { 0, 1, 2 },
            new[]
            {
                3.00, 3.50,
                3.25, 3.125
            })]
        [InlineData(new byte[] { 1, 2, 3 },
            new[]
            {
                0.00, 0.50,
                0.25, 0.125
            })]
        [InlineData(new byte[] { 0, 2 },
            new[]
            {
                1.00, 3.00,  1.500, 3.50,
                1.25, 3.25,  1.125, 3.125
            })]
        [InlineData(new byte[] { 1, 2 },
            new[]
            {
                0.00, 3.00,  0.50, 3.50,
                0.25, 3.25,  0.125, 3.125
            })]
        [InlineData(new byte[] { 1, 3 },
            new[]
            {
                0.00, 2.00,  0.50,  2.50,
                0.25, 2.25,  0.125, 2.125,
            })]
        [InlineData(new byte[] { 2, 0 },
            new[]
            {
                1.00, 3.00,  1.500,3.50,
                1.25, 3.25,  1.125, 3.125
            })]
        [InlineData(new byte[] { 3, 1, 3 },
            new[]
            {
                0.00, 2.00,  0.50, 2.50,
                0.25, 2.25,  0.125,2.125
            })]
        [InlineData(new byte[] { 3, 1, 2 },
            new[]
            {
                0.00,  0.50,
                0.25,  0.125
            })]
        [InlineData(new byte[] { 3, 3, 3 },
            new[]
            {
                0.00, 1.00, 2.00,  0.50, 1.50, 2.50,
                0.25, 1.25, 2.25,  0.125, 1.125, 2.125,
            })]
        public bool Should_Remove_Proper_Component(byte[] indecies, double[] result)
        {
            _image.RemoveComponents(indecies);
            var pix = _image.PixelsData;
            var allPixEqual = true;
            for (var i = 0; i < pix.Length; i++)
            {
                if (pix[i] != result[i])
                    allPixEqual = false;
            }

            return allPixEqual;
        }
    }
}
