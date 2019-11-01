using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    public class WhenTranslate
    {
        [Fact]
        public void Should_Move_Area_To_Origin()
        {
            var markup = new[]
            {
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,1,0,1,1,0,0,1},
                new byte[] { 0,0,1,1,0,0,1,1,1},
                new byte[] { 0,0,1,0,0,0,1,1,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,1,1,1,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
            };

            var expected = new Rectangle(0, 0, 7, 5);
            var a = new Area2D(3, 2, markup);

            a = a.Translate(-5, -3);

            var isFailed = a.Bound.X != expected.X ||
                            a.Bound.Y != expected.Y ||
                            a.Bound.Height != expected.Height ||
                            a.Bound.Width != expected.Width;

            isFailed.ShouldBeFalse();
        }

        [Fact]
        public void Should_Move_Area_To_4_3()
        {
            var markup = new[]
            {
                new byte[] { 1,0,1,1,0,0,1},
                new byte[] { 1,1,0,0,1,1,1},
                new byte[] { 1,0,0,0,1,1,0},
                new byte[] { 0,0,0,0,0,0,0},
                new byte[] { 0,0,1,1,1,0,0},
                new byte[] { 0,0,0,0,0,0,0},
            };

            var expected = new Rectangle(4, 3, 7, 5);
            var a = new Area2D(0, 0, markup);
            a = a.Translate(4, 3);

            var isFailed = a.Bound.X != expected.X ||
                            a.Bound.Y != expected.Y ||
                            a.Bound.Height != expected.Height ||
                            a.Bound.Width != expected.Width;

            isFailed.ShouldBeFalse();
        }

        [Fact]
        public void Should_Move_Area_To_min5_3()
        {
            var markup = new[]
            {
                new byte[] {0,0,0,0,0,0,0,0},
                new byte[] {0,1,0,1,1,0,0,1},
                new byte[] {0,1,1,0,0,1,1,1},
                new byte[] {0,1,0,0,0,1,1,0},
                new byte[] {0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,1,1,1,0,1},
            };

            var expected = new Rectangle(-5, 3, 7, 5);
            var a = new Area2D(0, 0, markup);
            a = a.Translate(-6, 2);

            var isFailed = a.Bound.X != expected.X ||
                            a.Bound.Y != expected.Y ||
                            a.Bound.Height != expected.Height ||
                            a.Bound.Width != expected.Width;

            isFailed.ShouldBeFalse();
        }

        [Fact]
        public void Should_Result_In_Empty_Area()
        {
            var markup = new[]
            {
                new byte[] {0,0,0,0,0,0,0,0},
                new byte[] {0,1,0,1,1,0,0,1},
                new byte[] {0,1,1,0,0,1,1,1},
                new byte[] {0,1,0,0,0,1,1,0},
                new byte[] {0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,1,1,1,0,1},
            };

            var a = new Area2D(-2, 3, markup); // real pos - (-1, 4)
            var b = new Area2D(5, -7, markup); // real pos - (6, -6)

            //  Move both areas to the position (3 , -1)
            a = a.Translate(5, -4);
            b = b.Translate(-2, 6);

            a.Substract(b).IsEmpty.ShouldBeTrue();
        }
    }
}
