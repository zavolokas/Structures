using System;
using System.Linq;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMap
{
    [TestFixture]
    public class WhenGetDestPoints
    {
        [Test]
        public void Should_Contain_All_Points_From_Dest_Areas()
        {
            var srcArea = Area2D.Create(0, 0, 3, 3);
            var emptyArea = Area2D.Empty;

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0,0,5, 5), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(4,4,6, 6), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(-3,-3,6, 6), srcArea)
            };
            var areaMap = new Area2DMap(areas, emptyArea);

            for (var i = 0; i < areas.Length; i++)
            {
                var dest = areas[i].Item1;
                foreach (var point in dest.Points)
                {
                    Assert.That(areaMap.DestPoints.Contains(point));
                }
            }
        }

        [Test]
        public void Points_Area_Ordered_From_Left_To_Right_From_Top_To_Bottom()
        {
            var srcArea = Area2D.Create(0, 0, 3, 3);
            var emptyArea = Area2D.Empty;

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0,0,5, 5), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(4,4,6, 6), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(-3,-3,6, 6), srcArea)
            };
            var areaMap = new Area2DMap(areas, emptyArea);

            var prevIndex = int.MinValue;
            foreach (var point in areaMap.DestPoints)
            {
                var index = point.Y * 1000 + point.X;
                Assert.That(index, Is.GreaterThan(prevIndex));

                prevIndex = index;
            }
        }

        [Test]
        public void Should_Not_Contain_Duplicated_Points_Even_When_Dest_Areas_Have_Common_Parts()
        {
            var srcArea = Area2D.Create(0, 0, 3, 3);
            var emptyArea = Area2D.Empty;

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0,0,5, 5), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(4,4,6, 6), srcArea)
            };
            var areaMap = new Area2DMap(areas, emptyArea);

            var result = areaMap.DestPoints;
            var noDuplicates = areaMap.DestPoints.Distinct();

            Assert.That(result.SequenceEqual(noDuplicates));
        }
    }
}
