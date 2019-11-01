using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Primitives.GivenVector2D
{
    public class WhenSumVectors
    {
        [Fact]
        public void ShouldSumVectors()
        {
            var v1 = new Vector2D(-2, 45);
            var v2 = new Vector2D(20, 5);

            var sum = v1 + v2;

            sum.X.ShouldBe(18);
            sum.Y.ShouldBe(50);
        }

        [Fact]
        public void ShouldMinusVectors()
        {
            var v1 = new Vector2D(12, 25);
            var v2 = new Vector2D(20, 5);

            var sum = v1 - v2;

            sum.X.ShouldBe(-8);
            sum.Y.ShouldBe(20);
        }

        [Fact]
        public void ShouldSumVectorAndPoint()
        {
            var v1 = new Vector2D(12, 25);
            var pt = new Point(-30, 10);

            var sum = v1 + pt;

            sum.X.ShouldBe(-18);
            sum.Y.ShouldBe(35);

        }

        [Fact]
        public void ShouldSumPointAndVector()
        {
            var v1 = new Vector2D(12, 25);
            var pt = new Point(-30, 10);

            var sum = pt + v1;

            sum.X.ShouldBe(-18);
            sum.Y.ShouldBe(35);

        }

        [Fact]
        public void ShouldMinusVectorAndPoint()
        {
            var v1 = new Vector2D(12, 25);
            var pt = new Point(-30, 10);

            var sum = v1 - pt;

            sum.X.ShouldBe(42);
            sum.Y.ShouldBe(15);
        }

        [Fact]
        public void ShouldMinusPointAndVector()
        {
            var v1 = new Vector2D(12, 25);
            var pt = new Point(-30, 10);

            var sum = pt - v1;

            sum.X.ShouldBe(-42);
            sum.Y.ShouldBe(-15);
        }
    }
}
