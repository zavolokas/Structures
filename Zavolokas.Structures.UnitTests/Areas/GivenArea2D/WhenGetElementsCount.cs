
using Shouldly;
using Xunit;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    public class WhenGetElementsCount
    {
        [Theory]
        [InlineData(0, 0,
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, 0)]
        [InlineData(-2, 0,
                  new byte[] { 1, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 1, 0, 1, 0, 0, 0 },
                  new byte[] { 0, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 1, 0, 0, 0, 0, 1 }, 6)]
        [InlineData(0, -5,
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 }, 40)]
        public void Should_Return_Correct_Number_Of_Elements(int x, int y, byte[] r1, byte[] r2, byte[] r3, byte[] r4, byte[] r5, int result)
        {
            var markup = new[] { r1, r2, r3, r4, r5 };
            var area = Area2D.Create(x, y, markup);
            area.ElementsCount.ShouldBe(result);
        }

        [Theory]
        [InlineData(0, 0, 5, 5, 25)]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(0, 0, 5, 0, 0)]
        [InlineData(0, 0, 0, 5, 0)]
        [InlineData(0, 0, 1, 1, 1)]
        [InlineData(2, 3, 2, 2, 4)]
        [InlineData(2, 3, 1, 1, 1)]
        [InlineData(-5, 2, 2, 1, 2)]
        public void Should_Return_Correct_ElementsCount(int x, int y, int width, int height, int result)
        {
            var area = Area2D.Create(x, y, width, height);
            area.ElementsCount.ShouldBe(result);
        }
    }
}