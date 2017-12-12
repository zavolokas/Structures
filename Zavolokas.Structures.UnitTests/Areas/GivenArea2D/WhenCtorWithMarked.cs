using System;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    [TestFixture]
    public class WhenCtorWithMarked
    {
        [TestCase(0, 0,
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            ExpectedResult = typeof(Area2D))]
        public Type Should_Create_An_Instance_Of_Area2D(int x, int y, byte[] r1, byte[] r2, byte[] r3, byte[] r4, byte[] r5)
        {
            var markup = new [] { r1, r2, r3, r4, r5 };
            var area = new Area2D(x, y, markup);
            return area.GetType();
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_Markup_Is_NULL()
        {
            Assert.Throws<ArgumentNullException>(() => { var f = new Area2D(0, 0, null); });
        }

        [Test]
        public void Should_Set_Boundary2()
        {
            var markup = new []
            {
                new byte[] {0, 0, 0, 0, 0, 0, 0, 0},
                new byte[] {0, 0, 0, 1, 0, 0, 0, 0},
                new byte[] {0, 1, 0, 0, 0, 1, 0, 0},
                new byte[] {0, 0, 0, 0, 0, 0, 0, 0},
                new byte[] {0, 0, 0, 1, 0, 0, 0, 0}
            };
            var a = new Area2D(2, -3, markup);

            Assert.That(a.Bound.X, Is.EqualTo(3));
            Assert.That(a.Bound.Y, Is.EqualTo(-2));
            Assert.That(a.Bound.Width, Is.EqualTo(5));
            Assert.That(a.Bound.Height, Is.EqualTo(4));
        }
    }
}
