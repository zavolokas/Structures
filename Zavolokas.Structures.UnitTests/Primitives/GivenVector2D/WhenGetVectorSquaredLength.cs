using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Primitives.GivenVector2D
{
    [TestFixture]
    public class WhenGetVectorSquaredLength
    {
        [Test]
        public void ShouldReturnSquaredLength()
        {
            const double x = -2;
            const double y = 45;

            var v1 = new Vector2D(x, y);

            Assert.AreEqual(x * x + y * y, v1.SquareLength);
        }


        [Test]
        public void ShouldReturnSquaredLength2()
        {
            const double x = 3;
            const double y = -10;

            var v1 = new Vector2D(x, y);

            Assert.AreEqual(x * x + y * y, v1.SquareLength);
        }
    }
}