using System;
using System.Collections.Generic;
using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMap
{
    public class WhenCtor
    {
        [Fact]
        public void Should_Throw_ArgumentNullException_Areas_Are_Null()
        {
            Tuple<Area2D, Area2D>[] areas = null;
            Should.Throw<ArgumentNullException>(() => new Area2DMap(areas, Area2D.Empty));
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_OutsideArea_Is_Null()
        {
            Area2D srcArea = Area2D.Create(5, 5, 10, 10);
            Area2D destArea = Area2D.Create(0, 0, 3, 3);
            var areas = new[] { new Tuple<Area2D, Area2D>(destArea, srcArea) };

            Should.Throw<ArgumentNullException>(() => new Area2DMap(areas, null));
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_Dest_Area_Is_Null()
        {
            var srcArea = Area2D.Create(0, 0, 3, 3);
            Area2D destArea = null;
            var areas = new[] { new Tuple<Area2D, Area2D>(destArea, srcArea) };
            Should.Throw<ArgumentNullException>(() => new Area2DMap(areas, Area2D.Empty));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Should_Throw_ArgumentNullException_When_Source_Or_Dest_Area_Is_Null(int ind)
        {
            var data = GetAreasWithNullSourceArea()[ind];
            int a = data.Item1;
            Tuple<Area2D, Area2D>[] areas = data.Item2;
            Area2D emptyArea = data.Item3;

            Should.Throw<ArgumentNullException>(() => new Area2DMap(areas, emptyArea));
        }

        public static Tuple<int, Tuple<Area2D,Area2D>[],Area2D>[] GetAreasWithNullSourceArea()
        {
            var data = new List<Tuple<int, Tuple<Area2D, Area2D>[], Area2D>>();

            Area2D srcArea = Area2D.Create(5, 5, 10, 10);
            Area2D destArea = Area2D.Create(0, 0, 3, 3);
            Area2D emptyArea = Area2D.Empty;

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(destArea, null),
            };
            data.Add(new Tuple<int, Tuple<Area2D, Area2D>[], Area2D>(1, areas, emptyArea));

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(destArea, srcArea),
                new Tuple<Area2D, Area2D>(destArea, null)
            };
            data.Add(new Tuple<int, Tuple<Area2D, Area2D>[], Area2D>(1, areas, emptyArea));

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(destArea, srcArea),
                new Tuple<Area2D, Area2D>(destArea, null),
                new Tuple<Area2D, Area2D>(destArea, srcArea),
            };
            data.Add(new Tuple<int, Tuple<Area2D, Area2D>[], Area2D>(1, areas, emptyArea));


            srcArea = Area2D.Create(5, 5, 10, 10);
            destArea = Area2D.Create(0, 0, 3, 3);
            emptyArea = Area2D.Empty;

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(null, srcArea),
            };
            data.Add(new Tuple<int, Tuple<Area2D, Area2D>[], Area2D>(1, areas, emptyArea));

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(destArea, srcArea),
                new Tuple<Area2D, Area2D>(null, srcArea)
            };
            data.Add(new Tuple<int, Tuple<Area2D, Area2D>[], Area2D>(1, areas, emptyArea));

            areas = new[]
            {
                new Tuple<Area2D, Area2D>(destArea, srcArea),
                new Tuple<Area2D, Area2D>(null, srcArea),
                new Tuple<Area2D, Area2D>(destArea, srcArea),
            };
            data.Add(new Tuple<int, Tuple<Area2D, Area2D>[], Area2D>(1, areas, emptyArea));

            return data.ToArray();
        }
    }
}
