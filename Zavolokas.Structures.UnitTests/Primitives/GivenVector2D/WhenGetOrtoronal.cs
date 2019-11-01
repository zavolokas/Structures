using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Primitives.GivenVector2D
{
    public class WhenGetOrtoronal
    {
        [Fact]
        public void ShouldReturn2OrtoVectors()
        {
            var vector = new Vector2D(2, 5);

            var ortVector1 = vector.Ortogonal1;
            var ortVector2 = vector.Ortogonal2;

            ortVector1.X.ShouldBe(vector.Y);
            ortVector1.Y.ShouldBe(-1 * vector.X);

            ortVector2.X.ShouldBe(-1 * vector.Y);
            ortVector2.Y.ShouldBe(vector.X);
        }

        [Fact]
        public void ShouldReturn2OrtoVectors2()
        {
            var vector = new Vector2D(-10, 3);

            var ortVector1 = vector.Ortogonal1;
            var ortVector2 = vector.Ortogonal2;

            ortVector1.X.ShouldBe(vector.Y);
            ortVector1.Y.ShouldBe(-1 * vector.X);

            ortVector2.X.ShouldBe(-1 * vector.Y);
            ortVector2.Y.ShouldBe(vector.X);
        }

        [Fact]
        public void ShouldReturn2OrtoVectors3()
        {
            var vector = new Vector2D(6, -5);

            vector.X = 10;
            vector.Y = 7;

            var ortVector1 = vector.Ortogonal1;
            var ortVector2 = vector.Ortogonal2;

            ortVector1.X.ShouldBe(vector.Y);
            ortVector1.Y.ShouldBe(-1 * vector.X);

            ortVector2.X.ShouldBe(-1 * vector.Y);
            ortVector2.Y.ShouldBe(vector.X);
        }
    }
}