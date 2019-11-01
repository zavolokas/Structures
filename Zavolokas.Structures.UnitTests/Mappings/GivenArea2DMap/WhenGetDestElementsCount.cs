using System;
using System.Collections.Generic;
using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMap
{
    public class WhenGetDestElementsCount
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Correct_Bounds(int ind)
        {
            var correctPointsCount = GetMapAndCount()[ind].Item1;
            var area2DMap = GetMapAndCount()[ind].Item2;
            area2DMap.DestElementsCount.ShouldBe(correctPointsCount);
        }

        public static Tuple<int, Area2DMap>[] GetMapAndCount()
        {
            var data = new List<Tuple<int, Area2DMap>>();

              var srcArea = Area2D.Create(0, 0, 3, 3);
            var emptyArea = Area2D.Empty;

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0, 0, 5, 5), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(4, 4, 6, 6), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(-3, -3, 6, 6), srcArea)
            };
            var areaMap = new Area2DMap(areas, emptyArea);
            data.Add(new Tuple<int, Area2DMap>(87, areaMap));

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0,0,5, 5), srcArea),
            };
            areaMap = new Area2DMap(areas, emptyArea);
            data.Add(new Tuple<int, Area2DMap>(25, areaMap));

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
            data.Add(new Tuple<int, Area2DMap>(4, areaMap));

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
            data.Add(new Tuple<int, Area2DMap>(402, areaMap));

            return data.ToArray();
        }
    }
}
