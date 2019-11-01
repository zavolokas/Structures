using System;
using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMap
{
    public class WhenGetPointSourceArea
    {
        [Fact]
        public void Should_Return_Empty_Area_When_Dest_Not_Contain_Point()
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

            var result = areaMap.GetPointSourceArea(new Point(-10, 0));
            result.IsEmpty.ShouldBeTrue();
        }

        [Fact]
        public void Should_Return_Source_That_Relates_To_Last_Dest_When_Point_Belongs_To_Several_Dest_Areas()
        {
            var srcArea1 = Area2D.Create(0, 0, 3, 3);
            var srcArea2 = Area2D.Create(3, 3, 3, 3);
            var srcArea3 = Area2D.Create(6, 6, 3, 3);
            var emptyArea = Area2D.Empty;

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0,0,5, 5), srcArea1),
                new Tuple<Area2D, Area2D>(Area2D.Create(4,4,6, 6), srcArea2),
                new Tuple<Area2D, Area2D>(Area2D.Create(-3,-3,6, 6), srcArea3)
            };
            var areaMap = new Area2DMap(areas, emptyArea);

            var result = areaMap.GetPointSourceArea(new Point(4, 4));

            result.IsSameAs(srcArea2).ShouldBeTrue();
        }

        [Theory]
        [InlineData(0, 0, 3)]
        [InlineData(0, 4, 1)]
        [InlineData(4, 0, 1)]
        [InlineData(4, 4, 2)]
        [InlineData(4, 2, 1)]
        [InlineData(2, 2, 3)]
        [InlineData(5, 5, 2)]
        [InlineData(5, 9, 2)]
        [InlineData(9, 5, 2)]
        [InlineData(9, 9, 2)]
        [InlineData(7, 7, 2)]
        [InlineData(-3, -3, 3)]
        [InlineData(-3, -1, 3)]
        [InlineData(-1, -3, 3)]
        [InlineData(-1, -1, 3)]
        [InlineData(-2, -2, 3)]
        public void Should_Return_Related_Source_Area(int x, int y, byte option)
        {
            var srcArea1 = Area2D.Create(0, 0, 3, 3);
            var srcArea2 = Area2D.Create(3, 3, 3, 3);
            var srcArea3 = Area2D.Create(6, 6, 3, 3);
            var emptyArea = Area2D.Empty;

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0,0,5, 5), srcArea1),
                new Tuple<Area2D, Area2D>(Area2D.Create(4,4,6, 6), srcArea2),
                new Tuple<Area2D, Area2D>(Area2D.Create(-3,-3,6, 6), srcArea3)
            };
            var areaMap = new Area2DMap(areas, emptyArea);

            var result = areaMap.GetPointSourceArea(new Point(x, y));

            if (option == 1)
                result.IsSameAs(srcArea1).ShouldBeTrue();
            else if (option == 2)
                result.IsSameAs(srcArea2).ShouldBeTrue();
            else if (option == 3)
                result.IsSameAs(srcArea3).ShouldBeTrue();

        }
    }
}
