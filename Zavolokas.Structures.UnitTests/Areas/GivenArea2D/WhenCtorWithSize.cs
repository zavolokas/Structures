using System;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    [TestFixture]
    public class WhenCtorWithSize
    {
        [Test]
        public void ShouldInitArea()
        {
            var x = 4;
            var y = 3;
            var width = 3;
            var height = 5;
            var area = new Area2D(x, y, width, height);

            Assert.That(area.ElementsCount, Is.EqualTo(width * height));
            Assert.That(area[0], Is.EqualTo(new Point(x, y)));
            Assert.That(area[area.ElementsCount - 1], Is.EqualTo(new Point(x + width - 1, y + height - 1)));

            Assert.That(area.Bound.X, Is.EqualTo(x));
            Assert.That(area.Bound.Y, Is.EqualTo(y));
            Assert.That(area.Bound.Width, Is.EqualTo(width));
            Assert.That(area.Bound.Height, Is.EqualTo(height));
        }

        //constructor doesn't allow negative values only for the height and width
        [TestCase(0, 0, 0, 0, ExpectedResult = typeof(Area2D))]
        [TestCase(0, 0, 1, 0, ExpectedResult = typeof(Area2D))]
        [TestCase(0, 0, 0, 1, ExpectedResult = typeof(Area2D))]
        [TestCase(0, 0, 1, 1, ExpectedResult = typeof(Area2D))]
        [TestCase(0, 0, 10, 1, ExpectedResult = typeof(Area2D))]
        [TestCase(0, 0, 1, 10, ExpectedResult = typeof(Area2D))]
        [TestCase(0, 0, 10, 10, ExpectedResult = typeof(Area2D))]
        [TestCase(-10, 0, 0, 0, ExpectedResult = typeof(Area2D))]
        [TestCase(-20, 0, 1, 0, ExpectedResult = typeof(Area2D))]
        [TestCase(-11, 0, 0, 1, ExpectedResult = typeof(Area2D))]
        [TestCase(-15, 0, 1, 1, ExpectedResult = typeof(Area2D))]
        [TestCase(-10, 0, 10, 1, ExpectedResult = typeof(Area2D))]
        [TestCase(-16, 0, 1, 10, ExpectedResult = typeof(Area2D))]
        [TestCase(-30, 0, 10, 10, ExpectedResult = typeof(Area2D))]
        [TestCase(0, -10, 0, 0, ExpectedResult = typeof(Area2D))]
        [TestCase(0, -11, 1, 0, ExpectedResult = typeof(Area2D))]
        [TestCase(0, -15, 0, 1, ExpectedResult = typeof(Area2D))]
        [TestCase(0, -5, 1, 1, ExpectedResult = typeof(Area2D))]
        [TestCase(0, -70, 10, 1, ExpectedResult = typeof(Area2D))]
        [TestCase(0, -12, 1, 10, ExpectedResult = typeof(Area2D))]
        [TestCase(0, -10, 10, 10, ExpectedResult = typeof(Area2D))]
        [TestCase(-17, -1, 0, 0, ExpectedResult = typeof(Area2D))]
        [TestCase(-1, -11, 1, 0, ExpectedResult = typeof(Area2D))]
        [TestCase(-12, -10, 0, 1, ExpectedResult = typeof(Area2D))]
        [TestCase(-10, -19, 1, 1, ExpectedResult = typeof(Area2D))]
        [TestCase(-15, -40, 10, 1, ExpectedResult = typeof(Area2D))]
        [TestCase(-19, -16, 1, 10, ExpectedResult = typeof(Area2D))]
        [TestCase(-23, -10, 10, 10, ExpectedResult = typeof(Area2D))]
        public Type Should_Create_An_Instance_Of_Area2D(int x, int y, int width, int height)
        {
            var result = new Area2D(x,y,width,height);
            return result.GetType();
        }

        [TestCase(1, 0, -31, -35, ExpectedResult = typeof(ArgumentException))]
        [TestCase(-1, 5, 31, -35, ExpectedResult = typeof(ArgumentException))]
        [TestCase(6, 1, -31, 35, ExpectedResult = typeof(ArgumentException))]
        public Type Should_Throw_Exception_When_Height_Or_Width_Negative(int x, int y, int width, int height)
        {
            var result = typeof(object);
            try
            {
                var area = new Area2D(x, y, width, height);
            }
            catch (Exception ex)
            {
                result = ex.GetType();
            }
            return result;
        }
    }
}
