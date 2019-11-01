using System;
using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    public class WhenCtorWithSize
    {
        [Fact]
        public void ShouldInitArea()
        {
            var x = 4;
            var y = 3;
            var width = 3;
            var height = 5;
            var area = new Area2D(x, y, width, height);


            area.ElementsCount.ShouldBe(width * height);
            area[0].ShouldBe(new Point(x, y));
            area[area.ElementsCount - 1].ShouldBe(new Point(x + width - 1, y + height - 1));

            area.Bound.X.ShouldBe(x);
            area.Bound.Y.ShouldBe(y);
            area.Bound.Width.ShouldBe(width);
            area.Bound.Height.ShouldBe(height);
        }

        //constructor doesn't allow negative values only for the height and width
        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(0, 0, 1, 0)]
        [InlineData(0, 0, 0, 1)]
        [InlineData(0, 0, 1, 1)]
        [InlineData(0, 0, 10, 1)]
        [InlineData(0, 0, 1, 10)]
        [InlineData(0, 0, 10, 10)]
        [InlineData(-10, 0, 0, 0)]
        [InlineData(-20, 0, 1, 0)]
        [InlineData(-11, 0, 0, 1)]
        [InlineData(-15, 0, 1, 1)]
        [InlineData(-10, 0, 10, 1)]
        [InlineData(-16, 0, 1, 10)]
        [InlineData(-30, 0, 10, 10)]
        [InlineData(0, -10, 0, 0)]
        [InlineData(0, -11, 1, 0)]
        [InlineData(0, -15, 0, 1)]
        [InlineData(0, -5, 1, 1)]
        [InlineData(0, -70, 10, 1)]
        [InlineData(0, -12, 1, 10)]
        [InlineData(0, -10, 10, 10)]
        [InlineData(-17, -1, 0, 0)]
        [InlineData(-1, -11, 1, 0)]
        [InlineData(-12, -10, 0, 1)]
        [InlineData(-10, -19, 1, 1)]
        [InlineData(-15, -40, 10, 1)]
        [InlineData(-19, -16, 1, 10)]
        [InlineData(-23, -10, 10, 10)]
        public void Should_Create_An_Instance_Of_Area2D(int x, int y, int width, int height)
        {
            var result = new Area2D(x, y, width, height);
            result.ShouldBeOfType<Area2D>();
        }

        [Theory]
        [InlineData(1, 0, -31, -35)]
        [InlineData(-1, 5, 31, -35)]
        [InlineData(6, 1, -31, 35)]
        public Type Should_Throw_Exception_When_Height_Or_Width_Negative(int x, int y, int width, int height)
        {
            var result = typeof(object);
            try
            {
                var area = new Area2D(x, y, width, height);
            }
            catch (Exception ex)
            {
                result = ex.GetType();
            }
            return result;
        }
    }
}
