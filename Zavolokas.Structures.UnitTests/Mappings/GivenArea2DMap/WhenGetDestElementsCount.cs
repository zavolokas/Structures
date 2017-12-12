using System;
using System.Collections.Generic;
using NUnit.Framework;
using Is = NUnit.Framework.Is;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMap
{
    [TestFixture]
    public class WhenGetDestElementsCount
    {
        [TestCaseSource(nameof(GetMapAndCount))]
        public void Should_Return_Correct_Bounds(int correctPointsCount, Area2DMap area2DMap)
        {
            Assert.That(area2DMap.DestElementsCount, Is.EqualTo(correctPointsCount));
        }

        public static IEnumerable<TestCaseData> GetMapAndCount()
        {
            var srcArea = Area2D.Create(0, 0, 3, 3);
            var emptyArea = Area2D.Empty;

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0, 0, 5, 5), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(4, 4, 6, 6), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(-3, -3, 6, 6), srcArea)
            };
            var areaMap = new Area2DMap(areas, emptyArea);
            yield return new TestCaseData(87, areaMap);

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0,0,5, 5), srcArea),
            };
            areaMap = new Area2DMap(areas, emptyArea);
            yield return new TestCaseData(25, areaMap);

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
            yield return new TestCaseData(4, areaMap);

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
            yield return new TestCaseData(402, areaMap);
        }
    }
}
