using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Primitives.GivenVector2D
{
    public class WhenMultiplyVectors
    {
        [Fact]
        public void ShouldReturnNumber()
        {
            var v1 = new Vector2D(10, -5);
            var v2 = new Vector2D(40, 6);

            var result = v1 * v2;

            result.ShouldBe(370);
        }
    }
}