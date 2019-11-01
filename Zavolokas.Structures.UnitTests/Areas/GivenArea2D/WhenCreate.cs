using System;
using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    public class WhenCreate
    {
        [Fact]
        public void ShouldReturnArea()
        {
            var area = Area2D.Create(0, 0, 10, 10);
            area.ShouldBeOfType<Area2D>();
            area.Bound.X.ShouldBe(0);
            area.Bound.Y.ShouldBe(0);
            area.Bound.Width.ShouldBe(10);
            area.Bound.Height.ShouldBe(10);
            area.IsEmpty.ShouldBeFalse();
        }

        [Fact]
        public void ShouldCreateArea()
        {
            const int x = 3;
            const int y = 5;

            var markup = new[]
            {
                new byte[] {0,1,0,0},
                new byte[] {1,1,1,1},
                new byte[] {0,0,1,1},
                new byte[] {0,1,1,1},
            };

            var area = Area2D.Create(x, y, markup);

            area.ShouldBeOfType<Area2D>();
            area.Bound.X.ShouldBe(3);
            area.Bound.Y.ShouldBe(5);
            area.Bound.Width.ShouldBe(4);
            area.Bound.Height.ShouldBe(4);
            area.ElementsCount.ShouldBe(10);
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_Markup_Is_Null()
        {
            const int x = 3;
            const int y = 5;

            Should.Throw<ArgumentNullException>(() => Area2D.Create(x, y, null));
        }
    }
}
