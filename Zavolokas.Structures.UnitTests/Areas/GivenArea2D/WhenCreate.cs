using System;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    [TestFixture]
    public class WhenCreate
    {
        [Test]
        public void ShouldReturnArea()
        {
            var area = Area2D.Create(0, 0, 10, 10);

            Assert.That(area, Is.TypeOf(typeof(Area2D)));
            Assert.That(area.Bound.X, Is.EqualTo(0));
            Assert.That(area.Bound.Y, Is.EqualTo(0));
            Assert.That(area.Bound.Width, Is.EqualTo(10));
            Assert.That(area.Bound.Height, Is.EqualTo(10));
            Assert.That(area.IsEmpty, Is.False);
        }

        [Test]
        public void ShouldCreateArea()
        {
            const int x = 3;
            const int y = 5;

            var markup = new[]
            {
                new byte[] {0,1,0,0},
                new byte[] {1,1,1,1},
                new byte[] {0,0,1,1},
                new byte[] {0,1,1,1},
            };

            var area = Area2D.Create(x, y, markup);

            Assert.That(area, Is.TypeOf(typeof(Area2D)));
            Assert.That(area.Bound.X, Is.EqualTo(3));
            Assert.That(area.Bound.Y, Is.EqualTo(5));
            Assert.That(area.Bound.Width, Is.EqualTo(4));
            Assert.That(area.Bound.Height, Is.EqualTo(4));
            Assert.That(area.ElementsCount, Is.EqualTo(10));
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_Markup_Is_Null()
        {
            const int x = 3;
            const int y = 5;

            Assert.Throws<ArgumentNullException>(() => Area2D.Create(x, y, null));
        }
    }
}
