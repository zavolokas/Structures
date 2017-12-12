using System;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMap
{
    [TestFixture]
    public class WhenGetPointSourceArea
    {
        [Test]
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
            Assert.That(result.IsEmpty);
        }

        [Test]
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

            Assert.That(result.IsSameAs(srcArea2));
        }

        [TestCase(0, 0, ExpectedResult = 3)]
        [TestCase(0, 4, ExpectedResult = 1)]
        [TestCase(4, 0, ExpectedResult = 1)]
        [TestCase(4, 4, ExpectedResult = 2)]
        [TestCase(4, 2, ExpectedResult = 1)]
        [TestCase(2, 2, ExpectedResult = 3)]
        [TestCase(5, 5, ExpectedResult = 2)]
        [TestCase(5, 9, ExpectedResult = 2)]
        [TestCase(9, 5, ExpectedResult = 2)]
        [TestCase(9, 9, ExpectedResult = 2)]
        [TestCase(7, 7, ExpectedResult = 2)]
        [TestCase(-3, -3, ExpectedResult = 3)]
        [TestCase(-3, -1, ExpectedResult = 3)]
        [TestCase(-1, -3, ExpectedResult = 3)]
        [TestCase(-1, -1, ExpectedResult = 3)]
        [TestCase(-2, -2, ExpectedResult = 3)]
        public byte Should_Return_Related_Source_Area(int x, int y)
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

            if (result.IsSameAs(srcArea1)) return 1;
            if (result.IsSameAs(srcArea2)) return 2;
            if (result.IsSameAs(srcArea3)) return 3;
            return 0;
        }
    }
}
