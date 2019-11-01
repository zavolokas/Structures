using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Primitives.GivenVector2D
{
    public class WhenCreateVector2D
    {
        [Fact]
        public void ShouldSetValues()
        {
            const double x = 23;
            const double y = 1.5;
            var vector = new Vector2D(x, y);

            vector.X.ShouldBe(x);
            vector.Y.ShouldBe(y);
        }
    }
}