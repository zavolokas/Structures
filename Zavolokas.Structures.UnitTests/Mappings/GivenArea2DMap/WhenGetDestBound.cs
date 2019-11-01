using System;
using System.Collections.Generic;
using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMap
{
    public class WhenGetDestBound
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Correct_Bounds(int ind)
        {
            var input = GetMapAndRect()[ind];
            var correctBound = input.Item1;
            var area2DMap = input.Item2;

            area2DMap.DestBounds.ShouldBe(correctBound);
        }

        public static Tuple<Rectangle, Area2DMap>[] GetMapAndRect()
        {
            List<Tuple<Rectangle, Area2DMap>> data = new List<Tuple<Rectangle, Area2DMap>>();

            var srcArea = Area2D.Create(0, 0, 3, 3);
            var emptyArea = Area2D.Empty;

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0,0,5, 5), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(4,4,6, 6), srcArea),
                new Tuple<Area2D, Area2D>(Area2D.Create(-3,-3,6, 6), srcArea)
            };
            var areaMap = new Area2DMap(areas, emptyArea);
            data.Add(new Tuple<Rectangle, Area2DMap>(new Rectangle(-3, -3, 13, 13), areaMap));


            areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0,0,5, 5), srcArea),
            };
            areaMap = new Area2DMap(areas, emptyArea);
            data.Add(new Tuple<Rectangle, Area2DMap>(new Rectangle(0, 0, 5, 5), areaMap));

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
            data.Add(new Tuple<Rectangle, Area2DMap>(new Rectangle(-5, 0, 16, 16), areaMap));

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
            data.Add(new Tuple<Rectangle, Area2DMap>(new Rectangle(-5, 0, 29, 24), areaMap));

            return data.ToArray();
        }
    }
}
