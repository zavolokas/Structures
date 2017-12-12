using System;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Images.GivenZsImage
{
    [TestFixture]
    public class WhenRemoveComponents
    {
        private double[] _pixelsData;
        private ZsImage _image;

        [SetUp]
        public void Setup()
        {
            _pixelsData = new[]
            {
                0.00, 1.00, 2.00, 3.00,  0.50, 1.50, 2.50, 3.50,
                0.25, 1.25, 2.25, 3.25,  0.125, 1.125, 2.125, 3.125
            };

            _image = new ZsImage(_pixelsData, 2, 2, 4);
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_Inxecies_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _image.RemoveComponents(null));
        }

        [TestCase(new byte[] { 4 }, ExpectedResult = typeof(ArgumentOutOfRangeException))]
        [TestCase(new byte[] { 5, 6 }, ExpectedResult = typeof(ArgumentOutOfRangeException))]
        [TestCase(new byte[] { 40, 41, 42 }, ExpectedResult = typeof(ArgumentOutOfRangeException))]
        [TestCase(new byte[] { 1, 4 }, ExpectedResult = typeof(ArgumentOutOfRangeException))]
        public Type Should_Throw_ArgumentOutOfRangeException_When_Index_Greater_Than_Components_Amount(byte[] indecies)
        {
            Type result = typeof(object);
            try
            {
                _image.RemoveComponents(indecies);
            }
            catch (Exception ex)
            {
                result = ex.GetType();
            }
            return result;
        }

        [TestCase(new byte[] { 1, 2, 3, 4 }, ExpectedResult = typeof(ArgumentOutOfRangeException))]
        [TestCase(new byte[] { 2, 3, 4 }, ExpectedResult = typeof(ArgumentOutOfRangeException))]
        [TestCase(new byte[] { 3, 4 }, ExpectedResult = typeof(ArgumentOutOfRangeException))]
        public Type Should_Throw_ArgumentOutOfRangeException_When_RemoveAmount_Exceeds_Pixel_Components(byte[] indecies)
        {
            Type result = typeof(object);
            try
            {
                _image.RemoveComponents(indecies);
            }
            catch (Exception ex)
            {
                result = ex.GetType();
            }
            return result;
        }

        public void Should_Throw_ArgumentException_When_Attempt_To_Remove_All_Components()
        {
            Assert.Throws<ArgumentException>(() => _image.RemoveComponents(0, 1, 2, 3));
        }

        [TestCase(new byte[] { 0 }, ExpectedResult = 12)]
        [TestCase(new byte[] { 1 }, ExpectedResult = 12)]
        [TestCase(new byte[] { 2 }, ExpectedResult = 12)]
        [TestCase(new byte[] { 3 }, ExpectedResult = 12)]
        [TestCase(new byte[] { 0, 1 }, ExpectedResult = 8)]
        [TestCase(new byte[] { 1, 2 }, ExpectedResult = 8)]
        [TestCase(new byte[] { 2, 3 }, ExpectedResult = 8)]
        [TestCase(new byte[] { 1, 3 }, ExpectedResult = 8)]
        [TestCase(new byte[] { 0, 2 }, ExpectedResult = 8)]
        [TestCase(new byte[] { 0, 1, 2 }, ExpectedResult = 4)]
        [TestCase(new byte[] { 1, 2, 3 }, ExpectedResult = 4)]
        [TestCase(new byte[] { 0, 2, 3 }, ExpectedResult = 4)]
        [TestCase(new byte[] { 0, 1, 3 }, ExpectedResult = 4)]
        [TestCase(new byte[] { 3, 0, 2 }, ExpectedResult = 4)]
        [TestCase(new byte[] { 3, 1, 2 }, ExpectedResult = 4)]
        [TestCase(new byte[] { 3, 3, 3 }, ExpectedResult = 12)]
        [TestCase(new byte[] { 3, 1, 3 }, ExpectedResult = 8)]
        public int Should_Reduce_Data_Array_Length(byte[] indecies)
        {
            _image.RemoveComponents(indecies);
            var pp = _image.PixelsData;
            return pp.Length;
        }

        [TestCase(new byte[] { 0 }, ExpectedResult = 6)]
        [TestCase(new byte[] { 1 }, ExpectedResult = 6)]
        [TestCase(new byte[] { 2 }, ExpectedResult = 6)]
        [TestCase(new byte[] { 3 }, ExpectedResult = 6)]
        [TestCase(new byte[] { 0, 1 }, ExpectedResult = 4)]
        [TestCase(new byte[] { 1, 2 }, ExpectedResult = 4)]
        [TestCase(new byte[] { 2, 3 }, ExpectedResult = 4)]
        [TestCase(new byte[] { 0, 1 }, ExpectedResult = 4)]
        [TestCase(new byte[] { 1, 3 }, ExpectedResult = 4)]
        [TestCase(new byte[] { 2, 1 }, ExpectedResult = 4)]
        [TestCase(new byte[] { 3, 0 }, ExpectedResult = 4)]
        [TestCase(new byte[] { 0, 1, 2 }, ExpectedResult = 2)]
        [TestCase(new byte[] { 1, 2, 3 }, ExpectedResult = 2)]
        [TestCase(new byte[] { 3, 1, 2 }, ExpectedResult = 2)]
        [TestCase(new byte[] { 3, 3, 3 }, ExpectedResult = 6)]
        [TestCase(new byte[] { 3, 1, 3 }, ExpectedResult = 4)]
        public int Should_Increase_Stride(byte[] indecies)
        {
            _image.RemoveComponents(indecies);
            return _image.Stride;
        }

        [TestCase(new byte[] { 0 }, ExpectedResult = 3)]
        [TestCase(new byte[] { 1 }, ExpectedResult = 3)]
        [TestCase(new byte[] { 2 }, ExpectedResult = 3)]
        [TestCase(new byte[] { 3 }, ExpectedResult = 3)]
        [TestCase(new byte[] { 0, 1 }, ExpectedResult = 2)]
        [TestCase(new byte[] { 1, 2 }, ExpectedResult = 2)]
        [TestCase(new byte[] { 2, 3 }, ExpectedResult = 2)]
        [TestCase(new byte[] { 0, 1, 2 }, ExpectedResult = 1)]
        [TestCase(new byte[] { 1, 2, 3 }, ExpectedResult = 1)]
        [TestCase(new byte[] { 3, 1, 2 }, ExpectedResult = 1)]
        [TestCase(new byte[] { 3, 3, 3 }, ExpectedResult = 3)]
        [TestCase(new byte[] { 3, 1, 3 }, ExpectedResult = 2)]
        public int Should_Decrease_NumberOfComponents(byte[] indecies)
        {
            _image.RemoveComponents(indecies);
            return _image.NumberOfComponents;
        }

        [TestCase(new byte[] { 0 },
            new[]
            {
                1.00, 2.00, 3.00,  1.50, 2.50, 3.50,
                1.25, 2.25, 3.25,  1.125, 2.125, 3.125
            },
            ExpectedResult = true)]
        [TestCase(new byte[] { 1 },
            new[]
            {
                0.00, 2.00, 3.00,  0.50, 2.50, 3.50,
                0.25, 2.25, 3.25,  0.125,2.125, 3.125
            },
            ExpectedResult = true)]
        [TestCase(new byte[] { 2 },
            new[]
            {
                0.00, 1.00, 3.00,  0.50, 1.50, 3.50,
                0.25, 1.25, 3.25,  0.125, 1.125, 3.125
            },
            ExpectedResult = true)]
        [TestCase(new byte[] { 3 },
            new[]
            {
                0.00, 1.00, 2.00,  0.50, 1.50, 2.50,
                0.25, 1.25, 2.25,  0.125, 1.125, 2.125
            },
            ExpectedResult = true)]
        [TestCase(new byte[] { 0, 1 },
            new[]
            {
                2.00, 3.00,  2.50, 3.50,
                2.25, 3.25,  2.125, 3.125
            },
            ExpectedResult = true)]
        [TestCase(new byte[] { 1, 2 },
            new[]
            {
                0.00, 3.00,  0.50, 3.50,
                0.25, 3.25,  0.125, 3.125
            },
            ExpectedResult = true)]
        [TestCase(new byte[] { 2, 3 },
            new[]
            {
                0.00, 1.00,  0.50, 1.50,
                0.25, 1.25,  0.125, 1.125
            },
            ExpectedResult = true)]
        [TestCase(new byte[] { 0, 1, 2 },
            new[]
            {
                3.00, 3.50,
                3.25, 3.125
            },
            ExpectedResult = true)]
        [TestCase(new byte[] { 1, 2, 3 },
            new[]
            {
                0.00, 0.50,
                0.25, 0.125
            },
            ExpectedResult = true)]
        [TestCase(new byte[] { 0, 2 },
            new[]
            {
                1.00, 3.00,  1.500, 3.50,
                1.25, 3.25,  1.125, 3.125
            },
            ExpectedResult = true)]

        [TestCase(new byte[] { 1, 2 },
            new[]
            {
                0.00, 3.00,  0.50, 3.50,
                0.25, 3.25,  0.125, 3.125
            },
            ExpectedResult = true)]

        [TestCase(new byte[] { 1, 3 },
            new[]
            {
                0.00, 2.00,  0.50,  2.50,
                0.25, 2.25,  0.125, 2.125,
            },
            ExpectedResult = true)]

        [TestCase(new byte[] { 2, 0 },
            new[]
            {
                1.00, 3.00,  1.500,3.50,
                1.25, 3.25,  1.125, 3.125
            },
            ExpectedResult = true)]

        [TestCase(new byte[] { 3, 1, 3 },
            new[]
            {
                0.00, 2.00,  0.50, 2.50,
                0.25, 2.25,  0.125,2.125
            },
            ExpectedResult = true)]

        [TestCase(new byte[] { 3, 1, 2 },
            new[]
            {
                0.00,  0.50,
                0.25,  0.125
            },
            ExpectedResult = true)]

        [TestCase(new byte[] { 3, 3, 3 },
            new[]
            {
                0.00, 1.00, 2.00,  0.50, 1.50, 2.50,
                0.25, 1.25, 2.25,  0.125, 1.125, 2.125,
            },
            ExpectedResult = true)]

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
