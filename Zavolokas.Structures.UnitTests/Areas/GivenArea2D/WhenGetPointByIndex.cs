using System;
using System.Linq;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    [TestFixture]
    public class WhenGetPointByIndex
    {
        [Test]
        public void Should_Throw_OutOfRangeException_When_Negative_Index2()
        {
            var area = Area2D.Create(0, 0, 5, 5);
            Assert.Throws<IndexOutOfRangeException>(() => { var p = area[-1]; });
        }

        [Test]
        public void Should_Throw_OutOfRangeException_When_Index_Greater_Than_ElementsCount2()
        {
            var area = Area2D.Create(0, 0, 5, 5);
            Assert.Throws<IndexOutOfRangeException>(() => { var p = area[30]; });
        }

        [TestCase(0, 0, 5, 5, 0, ExpectedResult = 0)]
        [TestCase(2, 3, 2, 2, 0, ExpectedResult = 2)]
        [TestCase(-5, 2, 2, 3, 0, ExpectedResult = -5)]
        [TestCase(0, 0, 5, 5, 24, ExpectedResult = 4)]
        [TestCase(2, 3, 2, 2, 3, ExpectedResult = 3)]
        [TestCase(-5, 2, 2, 3, 5, ExpectedResult = -4)]
        [TestCase(-1, 2, 3, 3, 8, ExpectedResult = 1)]
        public int X_Coordinate_Of_Point_Should_Be_Correct(int x, int y, int width, int height, int index)
        {
            var area = Area2D.Create(x, y, width, height);
            return area[index].X;
        }

        [TestCase(0, 0, 5, 5, 0, ExpectedResult = 0)]
        [TestCase(2, 3, 2, 2, 0, ExpectedResult = 3)]
        [TestCase(5, -2, 2, 3, 0, ExpectedResult = -2)]
        [TestCase(0, 0, 5, 5, 24, ExpectedResult = 4)]
        [TestCase(2, 3, 2, 2, 3, ExpectedResult = 4)]
        [TestCase(5, -5, 2, 3, 5, ExpectedResult = -3)]
        [TestCase(1, -1, 3, 3, 8, ExpectedResult = 1)]
        public int Y_Coordinate_Of_Point_Should_Be_Correct(int x, int y, int width, int height, int index)
        {
            var area = Area2D.Create(x, y, width, height);
            return area[index].Y;
        }

        [Test]
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

        [Test]
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

        [TestCase(0, 0,
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 1, 1, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  0,
            ExpectedResult = 1)]
        [TestCase(0, 0,
                  new byte[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 1, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            4,
            ExpectedResult = 2)]
        [TestCase(0, 0,
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            11,
            ExpectedResult = 3)]
        [TestCase(-2, -3,
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 1, 1, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  0,
            ExpectedResult = -1)]
        [TestCase(3, 3,
                  new byte[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 1, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            4,
            ExpectedResult = 5)]
        [TestCase(-1, 0,
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            11,
            ExpectedResult = 2)]
        public int X_Coordinate_Of_Point_Should_Be_Correct(int x, int y, byte[] r1, byte[] r2, byte[] r3, byte[] r4, byte[] r5, int index)
        {
            var markup = new[] { r1, r2, r3, r4, r5 };
            var area = Area2D.Create(x, y, markup);
            return area[index].X;
        }

        [TestCase(0, 0,
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 1, 1, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  0,
            ExpectedResult = 1)]
        [TestCase(0, 0,
                  new byte[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 1, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            5,
            ExpectedResult = 3)]
        [TestCase(0, 0,
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            11,
            ExpectedResult = 1)]
        [TestCase(-2, -1,
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 1, 1, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  0,
            ExpectedResult = 0)]
        [TestCase(1, 3,
                  new byte[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 1, 1, 1, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            5,
            ExpectedResult = 6)]
        [TestCase(0, 2,
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            11,
            ExpectedResult = 3)]

        public int Y_Coordinate_Of_Point_Should_Be_Correct(int x, int y, byte[] r1, byte[] r2, byte[] r3, byte[] r4, byte[] r5, int index)
        {
            var markup = new[] { r1, r2, r3, r4, r5 };
            var area = Area2D.Create(x, y, markup);
            return area[index].Y;
        }

        [Test]
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

            Assert.That(!isFailed);
        }

        [Test]
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

            Assert.That(!isFailed);
        }

        [Test]
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

            Assert.That(!isFailed);
        }

        [Test]
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

            Assert.That(!isFailed);
        }

        [Test]
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

            Assert.That(!isFailed);
        }
    }
}