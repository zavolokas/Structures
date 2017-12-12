using System;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Images.GivenZsImage
{
    [TestFixture]
    public class WhenSetComponentsValues
    {
        private double[] _pixelsData;
        private ZsImage _image;
        private Area2D _area;

        [SetUp]
        public void Setup()
        {
            _pixelsData = new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 0.125
            };

            _image = new ZsImage(_pixelsData, 2, 2, 3);

            _area = Area2D.Create(0, 0, 2, 2);
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_Values_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _image.SetComponentsValues(_area, null, 0));
        }

        [Test]
        public void Should_Throw_ArgumentException_When_Values_IsEmpty()
        {
            Assert.Throws<ArgumentException>(() => _image.SetComponentsValues(_area, new double[] { }, 0));
        }

        [Test]
        public void Should_Throw_ArgumentOutOfRangeExceptoin_When_Index_Outside_Image_Components()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _image.SetComponentsValues(_area, new[] { 1.0 }, 3));
        }

        [Test]
        public void Should_Leave_Image_Intact_When_The_Area_IsEmpty()
        {
            var oldPixels = new double[_image.PixelsData.Length];
            _image.PixelsData.CopyTo(oldPixels, 0);

            _image.SetComponentsValues(Area2D.Empty, new[] { 1.0 }, 0);
            var allPixEqual = true;
            for (var i = 0; i < _image.PixelsData.Length; i++)
            {
                if (_image.PixelsData[i] != oldPixels[i])
                    allPixEqual = false;
            }

            Assert.IsTrue(allPixEqual);
        }

        [Test]
        public void Should_Leave_Image_Intact_When_The_Area_Outside_Image()
        {
            var oldPixels = new double[_image.PixelsData.Length];
            _image.PixelsData.CopyTo(oldPixels, 0);

            _image.SetComponentsValues(Area2D.Create(-5, 0, 4, 2), new[] { 1.0 }, 0);
            var allPixEqual = true;
            for (var i = 0; i < _image.PixelsData.Length; i++)
            {
                if (_image.PixelsData[i] != oldPixels[i])
                    allPixEqual = false;
            }

            Assert.IsTrue(allPixEqual);
        }

        [TestCase(-2, -2, 6, 6,
            new[] { 3.0 }, 0,
            new[]
            {
                3.00, 1.00, 1.00,  3.00, 0.50, 0.50,
                3.00, 0.25, 0.25,  3.00, 0.125, 0.125
            },
            ExpectedResult = true)]
        [TestCase(0, 0, 1, 2,
            new[] { 3.0 }, 0,
            new[]
            {
                3.00, 1.00, 1.00,  0.50, 0.50, 0.50,
                3.00, 0.25, 0.25,  0.125, 0.125, 0.125
            },
            ExpectedResult = true)]
        [TestCase(1, 0, 1, 2,
            new[] { 2.5, 4.5 }, 1,
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 2.5, 4.5,
                0.25, 0.25, 0.25,  0.125, 2.5, 4.5
            },
            ExpectedResult = true)]
        [TestCase(1, 0, 1, 2,
            new[] { 7.5, 4.5, 5, 6 }, 2,
            new[]
            {
                1.00, 1.00, 1.00,  0.50, 0.50, 7.50,
                0.25, 0.25, 0.25,  0.125, 0.125, 7.5
            },
            ExpectedResult = true)]

        public bool Should_Change_Components(int areaX, int areaY, int areaW, int areaH,
            double[] values, byte index, double[] resultPixels)
        {
            _image.SetComponentsValues(Area2D.Create(areaX, areaY, areaW, areaH), values, index);
            var allPixEqual = true;
            for (var i = 0; i < _image.PixelsData.Length; i++)
            {
                if (_image.PixelsData[i] != resultPixels[i])
                    allPixEqual = false;
            }

            return allPixEqual;
        }
    }
}
