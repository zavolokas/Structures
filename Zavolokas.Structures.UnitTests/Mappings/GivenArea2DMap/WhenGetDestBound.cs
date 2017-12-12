using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMap
{
    [TestFixture]
    public class WhenGetDestBound
    {
        [TestCaseSource(nameof(GetMapAndRect))]
        public void Should_Return_Correct_Bounds(Rectangle correctBound, Area2DMap area2DMap)
        {
            Assert.That(area2DMap.DestBounds, Is.EqualTo(correctBound));
        }

        public static IEnumerable<TestCaseData> GetMapAndRect()
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
            yield return new TestCaseData(new Rectangle(-3, -3, 13, 13), areaMap);


            areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0,0,5, 5), srcArea),
            };
            areaMap = new Area2DMap(areas, emptyArea);
            yield return new TestCaseData(new Rectangle(0, 0, 5, 5), areaMap);

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(
                    Area2D.Create(-5, 0, new []
                    {
                            new byte[]{0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0},
                        }),
                    srcArea),
            };
            areaMap = new Area2DMap(areas, emptyArea);
            yield return new TestCaseData(new Rectangle(-5, 0, 16, 16), areaMap);

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(
                    Area2D.Create(-5, 0, new []
                    {
                            new byte[]{0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            new byte[]{0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0},
                        }),
                    srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(4,4,20,20), srcArea),
            };
            areaMap = new Area2DMap(areas, emptyArea);
            yield return new TestCaseData(new Rectangle(-5, 0, 29, 24), areaMap);
        }
    }
}
