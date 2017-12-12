using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Primitives.GivenVector2D
{
    [TestFixture]
    public class WhenSumVectors
    {
        [Test]
        public void ShouldSumVectors()
        {
            var v1 = new Vector2D(-2, 45);
            var v2 = new Vector2D(20, 5);

            var sum = v1 + v2;

            Assert.AreEqual(18, sum.X);
            Assert.AreEqual(50, sum.Y);
        }

        [Test]
        public void ShouldMinusVectors()
        {
            var v1 = new Vector2D(12, 25);
            var v2 = new Vector2D(20, 5);

            var sum = v1 - v2;

            Assert.AreEqual(-8, sum.X);
            Assert.AreEqual(20, sum.Y);
        }

        [Test]
        public void ShouldSumVectorAndPoint()
        {
            var v1 = new Vector2D(12, 25);
            var pt = new Point(-30, 10);

            var sum = v1 + pt;

            Assert.AreEqual(-18, sum.X);
            Assert.AreEqual(35, sum.Y);

        }

        [Test]
        public void ShouldSumPointAndVector()
        {
            var v1 = new Vector2D(12, 25);
            var pt = new Point(-30, 10);

            var sum = pt + v1;

            Assert.AreEqual(-18, sum.X);
            Assert.AreEqual(35, sum.Y);

        }

        [Test]
        public void ShouldMinusVectorAndPoint()
        {
            var v1 = new Vector2D(12, 25);
            var pt = new Point(-30, 10);

            var sum = v1 - pt;

            Assert.AreEqual(42, sum.X);
            Assert.AreEqual(15, sum.Y);
        }

        [Test]
        public void ShouldMinusPointAndVector()
        {
            var v1 = new Vector2D(12, 25);
            var pt = new Point(-30, 10);

            var sum = pt - v1;

            Assert.AreEqual(-42, sum.X);
            Assert.AreEqual(-15, sum.Y);
        }
    }
}
