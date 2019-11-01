using System;
using System.Linq;
using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    public class WhenGetPointByIndex
    {
        [Fact]
        public void Should_Throw_OutOfRangeException_When_Negative_Index2()
        {
            var area = Area2D.Create(0, 0, 5, 5);
            Assert.Throws<IndexOutOfRangeException>(() => { var p = area[-1]; });
        }

        [Fact]
        public void Should_Throw_OutOfRangeException_When_Index_Greater_Than_ElementsCount2()
        {
            var area = Area2D.Create(0, 0, 5, 5);
            Assert.Throws<IndexOutOfRangeException>(() => { var p = area[30]; });
        }

        [Theory]
        [InlineData(0, 0, 5, 5, 0,  0)]
        [InlineData(2, 3, 2, 2, 0,  2)]
        [InlineData(-5, 2, 2, 3, 0,  -5)]
        [InlineData(0, 0, 5, 5, 24,  4)]
        [InlineData(2, 3, 2, 2, 3,  3)]
        [InlineData(-5, 2, 2, 3, 5,  -4)]
        [InlineData(-1, 2, 3, 3, 8,  1)]
        public void X_Coordinate_Of_Point_Should_Be_Correct(int x, int y, int width, int height, int index, int result)
        {
            var area = Area2D.Create(x, y, width, height);
            area[index].X.ShouldBe(result);
        }

        [Theory]
        [InlineData(0, 0, 5, 5, 0,  0)]
        [InlineData(2, 3, 2, 2, 0,  3)]
        [InlineData(5, -2, 2, 3, 0,  -2)]
        [InlineData(0, 0, 5, 5, 24,  4)]
        [InlineData(2, 3, 2, 2, 3,  4)]
        [InlineData(5, -5, 2, 3, 5,  -3)]
        [InlineData(1, -1, 3, 3, 8,  1)]
        public void Y_Coordinate_Of_Point_Should_Be_Correct(int x, int y, int width, int height, int index, int result)
        {
            var area = Area2D.Create(x, y, width, height);
            area[index].Y.ShouldBe(result);
        }

        [Fact]
        public void Should_Throw_OutOfRangeException_When_Negative_Index()
        {
            var markup = new[]
            {
                new byte[] {1, 1, 1, 1, 1, 1, 1, 1},
                new byte[] {1, 1, 1, 1, 1, 1, 1, 1},
                new byte[] {1, 1, 1, 1, 1, 1, 1, 1},
                new byte[] {1, 1, 1, 1, 1, 1, 1, 1},
                new byte[] {1, 1, 1, 1, 1, 1, 1, 1}
            };
            var area = Area2D.Create(0, 0, markup);
            Assert.Throws<IndexOutOfRangeException>(() => { var p = area[-1]; });
        }

        [Fact]
        public void Should_Throw_OutOfRangeException_When_Index_Greater_Than_ElementsCount()
        {
            var markup = new[]
            {
                new byte[] {1, 1, 1, 1, 1, 1, 1, 1},
                new byte[] {1, 1, 1, 1, 1, 1, 1, 1},
                new byte[] {1, 1, 1, 1, 1, 1, 1, 1},
                new byte[] {1, 1, 1, 1, 1, 1, 1, 1},
                new byte[] {1, 1, 1, 1, 1, 1, 1, 1}
            };
            var area = Area2D.Create(0, 0, markup);

            Assert.Throws<IndexOutOfRangeException>(() => { var p = area[40]; });
        }

        [Theory]
        [InlineData(0, 0,
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 1, 1, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  0,
             1)]
        [InlineData(0, 0,
                  new byte[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 1, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            4,
             2)]
        [InlineData(0, 0,
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            11,
             3)]
        [InlineData(-2, -3,
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 1, 1, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  0,
             -1)]
        [InlineData(3, 3,
                  new byte[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 1, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            4,
             5)]
        [InlineData(-1, 0,
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            11,
             2)]
        public void X_Coordinate_Of_Point_Should_Be_Correct2(int x, int y, byte[] r1, byte[] r2, byte[] r3, byte[] r4, byte[] r5, int index, int result)
        {
            var markup = new[] { r1, r2, r3, r4, r5 };
            var area = Area2D.Create(x, y, markup);
            area[index].X.ShouldBe(result);
        }

        [Theory]
        [InlineData(0, 0,
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 1, 1, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  0,
             1)]
        [InlineData(0, 0,
                  new byte[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 1, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            5,
             3)]
        [InlineData(0, 0,
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            11,
             1)]
        [InlineData(-2, -1,
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 1, 1, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  0,
             0)]
        [InlineData(1, 3,
                  new byte[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 1, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            5,
             6)]
        [InlineData(0, 2,
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            11,
             3)]

        public void Y_Coordinate_Of_Point_Should_Be_Correct2(int x, int y, byte[] r1, byte[] r2, byte[] r3, byte[] r4, byte[] r5, int index, int result)
        {
            var markup = new[] { r1, r2, r3, r4, r5 };
            var area = Area2D.Create(x, y, markup);
            area[index].Y.ShouldBe(result);
        }

        [Fact]
        public void Area2D_Indexer_When_Rect_Ctor_Used()
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

            for (var i = 0; i < a.ElementsCount; i++)
            {
                var p = a[i];
                constr[p.Y][p.X] = 1;
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
        public void Area2D_Indexer_When_Markup_Ctor_Used()
        {
            bool isFailed = false;

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
            var dd = a[7];

            for (var i = 0; i < a.ElementsCount; i++)
            {
                var p = a[i];
                constr[p.Y][p.X] = 1;
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
        public void Area2D_Indexer_When_Markup_Ctor_Used_Not_In_Zero_Pos()
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
            var pp = a[0];

            for (int i = 0; i < a.ElementsCount; i++)
            {
                var p = a[i];
                constr[p.Y][p.X] = 1;
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
        public void Area2D_Indexer_When_Markup_Ctor_Used_Not_At_Zero_Pos2()
        {
            bool isFailed = false;

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
                new byte[] {0,0,0,0,0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,0,0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,0,0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,0,0,1,0,1,1,0,0,1},
                new byte[] {0,0,0,0,0,1,1,0,0,1,1,1},
                new byte[] {0,0,0,0,0,1,0,0,0,1,1,0},
                new byte[] {0,0,0,0,0,0,0,0,0,0,0,0},
                new byte[] {0,0,0,0,0,0,0,1,1,1,0,0},
                new byte[] {0,0,0,0,0,0,0,0,0,0,0,0},
            };

            var constr = new[]
            {
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0,0},
                new byte[] { 0,0,0,0,0,0,0,0,0,0,0,0},
            };

            var a = new Area2D(3, 2, markup);

            for (var i = 0; i < a.ElementsCount; i++)
            {
                var p = a[i];
                constr[p.Y][p.X] = 1;
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
        public void Area2D_Indexer_When_Markup_Ctor_Used_Not_In_Negative_Position()
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
            var refPoints = new[]{ new Point(-1, -4), new Point(0, -4), new Point(2, -4), new Point(3, -4), new Point(6, -4),
                new Point(-2, -3), new Point(-1, -3), new Point(0, -3), new Point(1, -3), new Point(4, -3), new Point(5, -3), new Point(6, -3),
                new Point(-1, -2), new Point(0, -2), new Point(4, -2), new Point(5, -2),
                new Point(2, 0), new Point(3, 0), new Point(4, 0)};

            var points = a.Points.ToArray();

            for (var i = 0; i < points.Length; i++)
            {
                var point = points[i];
                var refPoint = refPoints[i];
                if (point.X != refPoint.X || point.Y != refPoint.Y)
                {
                    isFailed = true;
                }
            }

            isFailed.ShouldBeFalse();
        }
    }
}