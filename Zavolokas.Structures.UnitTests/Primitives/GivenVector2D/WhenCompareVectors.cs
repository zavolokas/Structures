using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Primitives.GivenVector2D
{
    public class WhenCompareVectors
    {
        [Fact]
        public void RightShouldBeGreater()
        {
            var v1 = new Vector2D(1, 1);
            var v2 = new Vector2D(2, 2);

            v1.ShouldBeLessThan(v2);
        }


        [Fact]
        public void LeftShouldBeGreater()
        {
            var v1 = new Vector2D(1, 5);
            var v2 = new Vector2D(2, 2);

            v1.ShouldBeGreaterThan(v2);
        }

        [Fact]
        public void ShouldBeEqual()
        {
            var v1 = new Vector2D(1, 5);
            var v2 = new Vector2D(1, 5);

            (v1 == v2).ShouldBeTrue();
        }

        [Fact]
        public void ShouldBeNotEqual()
        {
            var v1 = new Vector2D(2, 5);
            var v2 = new Vector2D(1, 2);

            (v1 == v2).ShouldBeFalse();
        }

        [Fact]
        public void ShouldBeNotEqual2()
        {
            var v1 = new Vector2D(2, 5);
            var v2 = new Vector2D(1, 5);

            (v1 != v2).ShouldBeTrue();
        }

        [Fact]
        public void ShouldBeEqual2()
        {
            var v1 = new Vector2D(2, 5);
            var v2 = new Vector2D(2, 5);

            (v1 != v2).ShouldBeFalse();
        }
    }
}
