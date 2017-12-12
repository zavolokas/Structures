using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMap
{
    [TestFixture]
    public class WhenCtor
    {
        [Test]
        public void Should_Throw_ArgumentNullException_Areas_Are_Null()
        {
            Tuple<Area2D, Area2D>[] areas = null;
            Assert.Throws<ArgumentNullException>(() => new Area2DMap(areas, Area2D.Empty));
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_OutsideArea_Is_Null()
        {
            Area2D srcArea = Area2D.Create(5, 5, 10, 10);
            Area2D destArea = Area2D.Create(0, 0, 3, 3);
            var areas = new[] { new Tuple<Area2D, Area2D>(destArea, srcArea) };

            Assert.Throws<ArgumentNullException>(() => new Area2DMap(areas, null));
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_Dest_Area_Is_Null()
        {
            var srcArea = Area2D.Create(0, 0, 3, 3);
            Area2D destArea = null;
            var areas = new[] { new Tuple<Area2D, Area2D>(destArea, srcArea) };
            Assert.Throws<ArgumentNullException>(() => new Area2DMap(areas, Area2D.Empty));
        }

        [TestCaseSource(nameof(GetAreasWithNullDestArea))]
        [TestCaseSource(nameof(GetAreasWithNullSourceArea))]
        public void Should_Throw_ArgumentNullException_When_Source_Or_Dest_Area_Is_Null(int a, Tuple<Area2D, Area2D>[] areas, Area2D emptyArea)
        {
            Assert.Throws<ArgumentNullException>(() => new Area2DMap(areas, emptyArea));
        }

        public static IEnumerable<TestCaseData> GetAreasWithNullSourceArea()
        {
            Area2D srcArea = Area2D.Create(5, 5, 10, 10);
            Area2D destArea = Area2D.Create(0, 0, 3, 3);
            Area2D emptyArea = Area2D.Empty;

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(destArea, null),
            };
            yield return new TestCaseData(1, areas, emptyArea);

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(destArea, srcArea),
                new Tuple<Area2D, Area2D>(destArea, null)
            };
            yield return new TestCaseData(1, areas, emptyArea);

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(destArea, srcArea),
                new Tuple<Area2D, Area2D>(destArea, null),
                new Tuple<Area2D, Area2D>(destArea, srcArea),
            };
            yield return new TestCaseData(1, areas, emptyArea);
        }

        public static IEnumerable<TestCaseData> GetAreasWithEmptySourceArea()
        {
            Area2D emptyArea = Area2D.Empty;
            Area2D srcArea = Area2D.Create(5, 5, 10, 10);
            Area2D destArea = Area2D.Create(0, 0, 3, 3);

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(destArea, emptyArea),
            };
            yield return new TestCaseData(1, areas, emptyArea);

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(destArea, srcArea),
                new Tuple<Area2D, Area2D>(destArea, emptyArea)
            };
            yield return new TestCaseData(1, areas, emptyArea);

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(destArea, srcArea),
                new Tuple<Area2D, Area2D>(destArea, emptyArea),
                new Tuple<Area2D, Area2D>(destArea, srcArea),
            };
            yield return new TestCaseData(1, areas, emptyArea);
        }

        public static IEnumerable<TestCaseData> GetAreasWithNullDestArea()
        {
            Area2D srcArea = Area2D.Create(5, 5, 10, 10);
            Area2D destArea = Area2D.Create(0, 0, 3, 3);
            Area2D emptyArea = Area2D.Empty;

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(null, srcArea),
            };
            yield return new TestCaseData(1, areas, emptyArea);

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(destArea, srcArea),
                new Tuple<Area2D, Area2D>(null, srcArea)
            };
            yield return new TestCaseData(1, areas, emptyArea);

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(destArea, srcArea),
                new Tuple<Area2D, Area2D>(null, srcArea),
                new Tuple<Area2D, Area2D>(destArea, srcArea),
            };
            yield return new TestCaseData(1, areas, emptyArea);
        }

        public static IEnumerable<TestCaseData> GetAreasWithEmptyDestArea()
        {
            Area2D srcArea = Area2D.Create(5, 5, 10, 10);
            Area2D destArea = Area2D.Create(0, 0, 3, 3);
            Area2D emptyArea = Area2D.Empty;

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(emptyArea, srcArea),
            };
            yield return new TestCaseData(1, areas, emptyArea);

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(destArea, srcArea),
                new Tuple<Area2D, Area2D>(emptyArea, srcArea)
            };
            yield return new TestCaseData(1, areas, emptyArea);

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(destArea, srcArea),
                new Tuple<Area2D, Area2D>(emptyArea, srcArea),
                new Tuple<Area2D, Area2D>(destArea, srcArea),
            };
            yield return new TestCaseData(1, areas, emptyArea);
        }
    }
}
