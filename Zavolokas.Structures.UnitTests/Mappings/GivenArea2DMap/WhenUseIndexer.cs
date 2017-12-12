using System;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMap
{
    [TestFixture]
    public class WhenUseIndexer
    {
        private Area2D _emptyArea;

        [SetUp]
        public void Setup()
        {
            _emptyArea = Area2D.Empty;
        }

        [Test]
        public void Should_Throw_IndexOutOfRangeException_When_Index_Less_Than_Zero()
        {
            var areas = new[] { new Tuple<Area2D, Area2D>(Area2D.Create(0, 0, 10, 10), Area2D.Create(0, 0, 10, 10)) };
            var map = new Area2DMap(areas, _emptyArea);
            Assert.Throws<IndexOutOfRangeException>(() => { var p = map[-1]; });
        }

        [Test]
        public void Should_Throw_IndexOutOfRangeException_When_Index_Greater_Than_DestElementsCount()
        {
            var areas = new[] { new Tuple<Area2D, Area2D>(Area2D.Create(0, 0, 10, 10), Area2D.Create(0, 0, 10, 10)) };
            var map = new Area2DMap(areas, _emptyArea);
            Assert.Throws<IndexOutOfRangeException>(() => { var p = map[100]; });
        }

        [Test]
        public void Should_Return_Points_That_Contained_In_Dest_Area()
        {
            var srcArea = Area2D.Create(0, 0, 3, 3);

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0,0,5, 5), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(4,4,6, 6), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(-3,-3,6, 6), srcArea)
            };
            var areaMap = new Area2DMap(areas, _emptyArea);

            for (var i = 0; i < areaMap.DestElementsCount; i++)
            {
                var point = areaMap[i];
                var found = false;

                for (var j = 0; j < areas.Length; j++)
                {
                    var dest = areas[j].Item1;

                    if (dest.GetPointIndex(point) > -1)
                    {
                        found = true;
                        break;
                    }
                }

                Assert.That(found);
            }
        }

        [Test]
        public void Should_Contain_Points_From_Dest_Areas()
        {
            var srcArea = Area2D.Create(0, 0, 3, 3);

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0,0,5, 5), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(4,4,6, 6), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(-3,-3,6, 6), srcArea)
            };
            var areaMap = new Area2DMap(areas, _emptyArea);

            for (var j = 0; j < areas.Length; j++)
            {
                var dest = areas[j].Item1;

                foreach (var destPoint in dest.Points)
                {
                    var found = false;

                    for (var i = 0; i < areaMap.DestElementsCount; i++)
                    {
                        var point = areaMap[i];

                        if (destPoint.X == point.X && destPoint.Y == point.Y)
                            found = true;
                    }

                    Assert.That(found);
                }
            }
        }

        [Test]
        public void Should_Return_Ordered_From_Left_To_Right_From_Top_To_Bottom()
        {
            var srcArea = Area2D.Create(0, 0, 3, 3);

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0,0,5, 5), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(4,4,6, 6), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(-3,-3,6, 6), srcArea)
            };
            var areaMap = new Area2DMap(areas, _emptyArea);

            var prevIndex = int.MinValue;
            for (var i = 0; i < areaMap.DestElementsCount; i++)
            {
                var point = areaMap[i];
                var index = point.Y * 1000 + point.X;
                Assert.That(index, Is.GreaterThan(prevIndex));

                prevIndex = index;
            }
        }
    }
}
