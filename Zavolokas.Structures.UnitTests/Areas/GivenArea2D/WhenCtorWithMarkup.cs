using Shouldly;
using System;
using Xunit;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    public class WhenCtorWithMarkup
    {
        [Theory]
        [InlineData(0, 0,
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 })]
        public void Should_Create_An_Instance_Of_Area2D(int x, int y, byte[] r1, byte[] r2, byte[] r3, byte[] r4, byte[] r5)
        {
            var markup = new[] { r1, r2, r3, r4, r5 };
            var area = new Area2D(x, y, markup);
            area.ShouldBeOfType<Area2D>();
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_Markup_Is_NULL()
        {
            Should.Throw<ArgumentNullException>(() => { var f = new Area2D(0, 0, null); });
        }

        [Fact]
        public void Should_Set_Boundary2()
        {
            var markup = new[]
            {
                new byte[] {0, 0, 0, 0, 0, 0, 0, 0},
                new byte[] {0, 0, 0, 1, 0, 0, 0, 0},
                new byte[] {0, 1, 0, 0, 0, 1, 0, 0},
                new byte[] {0, 0, 0, 0, 0, 0, 0, 0},
                new byte[] {0, 0, 0, 1, 0, 0, 0, 0}
            };
            var area = new Area2D(2, -3, markup);

            area.Bound.X.ShouldBe(3);
            area.Bound.Y.ShouldBe(-2);
            area.Bound.Width.ShouldBe(5);
            area.Bound.Height.ShouldBe(4);
        }
    }
}
