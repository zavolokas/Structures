using Shouldly;
using Xunit;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    public class WhenGetBound
    {
        [Fact]
        public void Area2D_Bounds_Test_1()
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

            var expected = new Rectangle(5, 3, 7, 5);
            var a = new Area2D(3, 2, markup);

            var isFailed = a.Bound.X != expected.X ||
                            a.Bound.Y != expected.Y ||
                            a.Bound.Height != expected.Height ||
                            a.Bound.Width != expected.Width;

            isFailed.ShouldBeFalse();
        }

        [Fact]
        public void Area2D_Bounds_Test_2()
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

            var expected = new Rectangle(0, 0, 7, 5);
            var a = new Area2D(0, 0, markup);

            var isFailed = a.Bound.X != expected.X ||
                            a.Bound.Y != expected.Y ||
                            a.Bound.Height != expected.Height ||
                            a.Bound.Width != expected.Width;

            isFailed.ShouldBeFalse();
        }

        [Fact]
        public void Area2D_Bounds_Test_3()
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

            var expected = new Rectangle(1, 1, 7, 5);
            var a = new Area2D(0, 0, markup);

            var isFailed = a.Bound.X != expected.X ||
                            a.Bound.Y != expected.Y ||
                            a.Bound.Height != expected.Height ||
                            a.Bound.Width != expected.Width;

            isFailed.ShouldBeFalse();
        }

        [Fact]
        public void Area2D_Bounds_Test_4()
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

            var expected = new Rectangle(2, 2, 7, 5);
            var a = new Area2D(1, 1, markup);

            var isFailed = a.Bound.X != expected.X ||
                            a.Bound.Y != expected.Y ||
                            a.Bound.Height != expected.Height ||
                            a.Bound.Width != expected.Width;

            isFailed.ShouldBeFalse();
        }

        [Fact]
        public void Area2D_Bounds_Test_5()
        {
            var expected = new Rectangle(1, 1, 5, 3);
            var a = new Area2D(1, 1, 5, 3);

            var isFailed = a.Bound.X != expected.X ||
                            a.Bound.Y != expected.Y ||
                            a.Bound.Height != expected.Height ||
                            a.Bound.Width != expected.Width;

            isFailed.ShouldBeFalse();
        }

        [Fact]
        public void Area2D_Bounds_Test_6()
        {
            var expected = new Rectangle(0, 0, 4, 2);
            var a = new Area2D(0, 0, 4, 2);

            var isFailed = a.Bound.X != expected.X ||
                            a.Bound.Y != expected.Y ||
                            a.Bound.Height != expected.Height ||
                            a.Bound.Width != expected.Width;

            isFailed.ShouldBeFalse();
        }
    }
}
