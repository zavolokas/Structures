using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Primitives.GivenVector2D
{
    [TestFixture]
    public class WhenGetOrtoronal
    {
        [Test]
        public void ShouldReturn2OrtoVectors()
        {
            var vector = new Vector2D(2, 5);

            var ortVector1 = vector.Ortogonal1;
            var ortVector2 = vector.Ortogonal2;

            Assert.AreEqual(vector.Y, ortVector1.X);
            Assert.AreEqual(-1 * vector.X, ortVector1.Y);

            Assert.AreEqual(-1 * vector.Y, ortVector2.X);
            Assert.AreEqual(vector.X, ortVector2.Y);
        }

        [Test]
        public void ShouldReturn2OrtoVectors2()
        {
            var vector = new Vector2D(-10, 3);

            var ortVector1 = vector.Ortogonal1;
            var ortVector2 = vector.Ortogonal2;

            Assert.AreEqual(vector.Y, ortVector1.X);
            Assert.AreEqual(-1 * vector.X, ortVector1.Y);

            Assert.AreEqual(-1 * vector.Y, ortVector2.X);
            Assert.AreEqual(vector.X, ortVector2.Y);
        }

        [Test]
        public void ShouldReturn2OrtoVectors3()
        {
            var vector = new Vector2D(6, -5);

            vector.X = 10;
            vector.Y = 7;

            var ortVector1 = vector.Ortogonal1;
            var ortVector2 = vector.Ortogonal2;

            Assert.AreEqual(vector.Y, ortVector1.X);
            Assert.AreEqual(-1 * vector.X, ortVector1.Y);

            Assert.AreEqual(-1 * vector.Y, ortVector2.X);
            Assert.AreEqual(vector.X, ortVector2.Y);
        }
    }
}