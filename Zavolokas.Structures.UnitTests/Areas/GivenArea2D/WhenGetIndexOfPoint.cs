using System.Linq;
using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    public class WhenGetIndexOfPoint
    {
        [Theory]
        [InlineData(0, 0,
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 1, 1, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  1, 1, 0)]
        [InlineData(-2, 0,
                  new byte[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 1, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            -1, 3, 5)]
        [InlineData(-5, 0,
                  new byte[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 1, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            -3, 0, -1)]
        [InlineData(2, 3,
                  new byte[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 1, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            3, 4, 3)]
        [InlineData(3, 4,
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            -2, 1, -1)]
        [InlineData(2, 2,
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            2, 1, -1)]
        [InlineData(2, -2,
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            0, -3, -1)]
        [InlineData(-500, 45,
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            -508, 45, -1)]
        [InlineData(100, -300,
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            100, -305, -1)]
        public void Point_Index_Should_Be_Correct(int x, int y, byte[] r1, byte[] r2, byte[] r3, byte[] r4, byte[] r5, int px, int py, int result)
        {
            var markup = new[] { r1, r2, r3, r4, r5 };
            var area = Area2D.Create(x, y, markup);
            var point = new Point(px, py);
            area.GetPointIndex(point).ShouldBe(result);
        }

        [Theory]
        [InlineData(0, 0, 5, 5, 0, 0, 0)]
        [InlineData(2, 3, 2, 2, 2, 3, 0)]
        [InlineData(5, -2, 2, 3, 5, -2, 0)]
        [InlineData(0, 0, 5, 5, 4, 4, 24)]
        [InlineData(2, 3, 2, 2, 3, 4, 3)]
        [InlineData(5, -5, 2, 3, 6, -3, 5)]
        [InlineData(-1, -1, 3, 3, 1, 1, 8)]
        [InlineData(-1, -1, 3, 3, 10, 10, -1)]
        public void Index_Of_Provided_Point_Should_Be_Correct(int x, int y, int width, int height, int pointX, int pointY, int result)
        {
            var area = Area2D.Create(x, y, width, height);
            var point = new Point(pointX, pointY);
            area.GetPointIndex(point).ShouldBe(result);
        }

        [Fact]
        public static void GetPointIndex_When_Rect_Ctor_Used()
        {
            var isFailed = false;

            var reference = new[]
            {
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,1,1,1,1,1,1,1},
                new byte[] { 0,0,1,1,1,1,1,1,1},
                new byte[] { 0,0,1,1,1,1,1,1,1},
                new byte[] { 0,0,1,1,1,1,1,1,1},
            };

            var constr = new[]
            {
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
            };

            var a = new Area2D(2, 3, 7, 4);

            for (var y = 0; y < 7; y++)
            {
                for (var x = 0; x < 9; x++)
                {
                    if (a.GetPointIndex(new Point(x, y)) > -1)
                    {
                        constr[y][x] = 1;
                    }
                }
            }

            for (var y = 0; y < 7; y++)
            {
                for (var x = 0; x < 9; x++)
                {
                    if (constr[y][x] != reference[y][x])
                    {
                        isFailed = true;
                    }
                }
            }
            isFailed.ShouldBeFalse();
        }

        [Fact]
        public void Area2D_GetPointIndex_When_Markup_Ctor_Used()
        {
            var isFailed = false;

            var reference = new[]
            {
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,1,1,0,1,1,0,0,1},
                new byte[] { 1,1,1,1,0,0,1,1,1},
                new byte[] { 0,1,1,0,0,0,1,1,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,1,1,1,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
            };

            var constr = new[]
            {
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
            };

            var a = new Area2D(0, 0, reference);


            for (var y = 0; y < 7; y++)
            {
                for (var x = 0; x < 9; x++)
                {
                    if (a.GetPointIndex(new Point(x, y)) > -1)
                    {
                        constr[y][x] = 1;
                    }
                }
            }

            for (var y = 0; y < 7; y++)
            {
                for (var x = 0; x < 9; x++)
                {
                    if (constr[y][x] != reference[y][x])
                    {
                        isFailed = true;
                    }
                }
            }
            isFailed.ShouldBeFalse();
        }

        [Fact]
        public void Area2D_GetPointIndex_When_Markup_Ctor_Used_Not_In_Zero_Pos()
        {
            bool isFailed = false;

            var mrkp = new[]
            {
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,1,1,0,1,1,0,0,1},
                new byte[] { 1,1,1,1,0,0,1,1,1},
                new byte[] { 0,1,1,0,0,0,1,1,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,1,1,1,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
            };

            var reference = new[]
            {
                new byte[] {0,0,0,0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,1,1,0,1,1,0,0,1},
                new byte[] {0,0,1,1,1,1,0,0,1,1,1},
                new byte[] {0,0,0,1,1,0,0,0,1,1,0},
                new byte[] {0,0,0,0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,0,0,0,1,1,1,0,0},
            };

            var constr = new[]
            {
                new byte[] {0,0,0,0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,0,0,0,0,0,0,0,0},
            };

            var a = new Area2D(2, 2, mrkp);

            for (var y = 0; y < constr.Length; y++)
            {
                for (var x = 0; x < constr[y].Length; x++)
                {
                    if (a.GetPointIndex(new Point(x, y)) > -1)
                    {
                        constr[y][x] = 1;
                    }
                }
            }

            for (var y = 0; y < constr.Length; y++)
            {
                for (var x = 0; x < constr[y].Length; x++)
                {
                    if (constr[y][x] != reference[y][x])
                    {
                        isFailed = true;
                    }
                }
            }
            isFailed.ShouldBeFalse();
        }

        [Fact]
        public void Area2D_GetPointIndex_When_Markup_Ctor_Used_Not_At_Zero_Pos2()
        {
            var isFailed = false;

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

            var reference = new[]
            {
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,1,0,1,1,0,0,1},
                new byte[] { 0,0,0,0,1,1,0,0,1,1,1},
                new byte[] { 0,0,0,0,1,0,0,0,1,1,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,1,1,1,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0},
            };

            var constr = new[]
            {
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0},
            };

            var a = new Area2D(2, 2, markup);

            for (var y = 0; y < constr.Length; y++)
            {
                for (var x = 0; x < constr[y].Length; x++)
                {
                    if (a.GetPointIndex(new Point(x, y)) > -1)
                    {
                        constr[y][x] = 1;
                    }
                }
            }

            for (var y = 0; y < constr.Length; y++)
            {
                for (var x = 0; x < constr[y].Length; x++)
                {
                    if (constr[y][x] != reference[y][x])
                    {
                        isFailed = true;
                    }
                }
            }

            isFailed.ShouldBeFalse();
        }

        [Fact]
        public void Area2D_GetPointIndex_When_Markup_Ctor_Used_Not_In_Negative_Position()
        {
            var isFailed = false;

            var mrkp = new[]
            {
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,1,1,0,1,1,0,0,1},
                new byte[] { 1,1,1,1,0,0,1,1,1},
                new byte[] { 0,1,1,0,0,0,1,1,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,1,1,1,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0},
            };

            var a = new Area2D(-2, -5, mrkp);

            var points = a.Points.ToArray();

            for (var i = 0; i < points.Length; i++)
            {

                if (i != a.GetPointIndex(points[i]))
                {
                    isFailed = true;
                }
            }

            isFailed.ShouldBeFalse();
        }
    }
}