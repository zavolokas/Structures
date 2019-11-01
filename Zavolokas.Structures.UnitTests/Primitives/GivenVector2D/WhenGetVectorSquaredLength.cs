using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Primitives.GivenVector2D
{
    public class WhenGetVectorSquaredLength
    {
        [Fact]
        public void ShouldReturnSquaredLength()
        {
            const double x = -2;
            const double y = 45;

            var v1 = new Vector2D(x, y);

            v1.SquareLength.ShouldBe(x * x + y * y);
        }


        [Fact]
        public void ShouldReturnSquaredLength2()
        {
            const double x = 3;
            const double y = -10;

            var v1 = new Vector2D(x, y);

            v1.SquareLength.ShouldBe(x * x + y * y);
        }
    }
}