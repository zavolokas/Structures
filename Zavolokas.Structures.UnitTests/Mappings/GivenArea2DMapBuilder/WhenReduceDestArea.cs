using System;
using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMapBuilder
{
    public class WhenReduceDestArea
    {
        private Area2D _destArea;
        private Area2D _sourceArea;
        private Area2DMapBuilder _mapBuilder;

        public WhenReduceDestArea()
        {
            _mapBuilder = new Area2DMapBuilder();

            _sourceArea = Area2D.Create(0, 0, 10, 10);
            _destArea = Area2D.Create(0, 0, 10, 10);
        }

        [Fact]
        public void Should_Throw_MapIsNotInitializedException_When_Called_Before_InitMap_Call()
        {
            var reducedArea = Area2D.Create(0, 0, 5, 5);
            Should.Throw<MapIsNotInitializedException>(() => _mapBuilder.ReduceDestArea(reducedArea));
        }

        [Fact]
        public void Should_Throw_EmptyAreaException_When_ReducedArea_IsEmpty()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            var reducedArea = Area2D.Empty;
            Should.Throw<EmptyAreaException>(() => _mapBuilder.ReduceDestArea(reducedArea));
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_ReducedArea_Is_Null()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            Should.Throw<ArgumentNullException>(() => _mapBuilder.ReduceDestArea(null));
        }

        [Fact]
        public void Should_Throw_AreaRemovedException_When_Intersection_Of_ReducedArea_And_Dest_IsEmpty()
        {
            _destArea = Area2D.Create(0, 0, 10, 10);
            _mapBuilder.InitNewMap(_destArea, _sourceArea);

            var reducedArea = Area2D.Create(15, 15, 20, 20);
            Should.Throw<AreaRemovedException>(() => _mapBuilder.ReduceDestArea(reducedArea));
        }

        [Fact]
        public void Should_Throw_AreaRemovedException_When_Intersection_Of_ReducedAreas_And_Dest_IsEmpty()
        {
            _destArea = Area2D.Create(0, 0, 10, 10);
            _mapBuilder.InitNewMap(_destArea, _sourceArea);

            var reducedArea1 = Area2D.Create(0, 0, 5, 5);
            var reducedArea2 = Area2D.Create(5, 5, 5, 5);

            _mapBuilder.ReduceDestArea(reducedArea1);
            Should.Throw<AreaRemovedException>(() => _mapBuilder.ReduceDestArea(reducedArea2));
        }
    }
}
