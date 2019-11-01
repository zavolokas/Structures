using System;
using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMapBuilder
{
    public class WhenAddAssociatedAreas
    {
        private Area2D _destArea;
        private Area2D _sourceArea;
        private Area2DMapBuilder _mapBuilder;

        public WhenAddAssociatedAreas()
        {
            _mapBuilder = new Area2DMapBuilder();

            _sourceArea = Area2D.Create(0, 0, 10, 10);
            _destArea = Area2D.Create(0, 0, 10, 10);
        }

        [Fact]
        public void Should_Throw_MapIsNotInitializedException_When_Called_Before_InitMap_Call()
        {
            var areaOnDest = Area2D.Create(0, 0, 5, 5);
            var areaOnSource = Area2D.Create(0, 0, 5, 5);

            Should.Throw<MapIsNotInitializedException>(() => _mapBuilder.AddAssociatedAreas(areaOnDest, areaOnSource));
        }

        [Fact]
        public void Should_Throw_EmptyAreaException_When_AreaOnDest_IsEmpty()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            var areaOnDest = Area2D.Empty;
            var areaOnSource = Area2D.Create(0, 0, 5, 5);

            Should.Throw<EmptyAreaException>(() => _mapBuilder.AddAssociatedAreas(areaOnDest, areaOnSource));
        }

        [Fact]
        public void Should_Throw_EmptyAreaException_When_AreaOnSource_IsEmpty()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            var areaOnDest = Area2D.Create(0, 0, 5, 5);
            var areaOnSource = Area2D.Empty;
            Should.Throw<EmptyAreaException>(() => _mapBuilder.AddAssociatedAreas(areaOnDest, areaOnSource));
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_AreaOnSource_Is_Null()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            Area2D areaOnDest = Area2D.Create(0, 0, 5, 5);
            Area2D areaOnSource = null;
            Should.Throw<ArgumentNullException>(() => _mapBuilder.AddAssociatedAreas(areaOnDest, areaOnSource));
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_AreaOnDest_Is_Null()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            Area2D areaOnDest = null;
            Area2D areaOnSource = Area2D.Create(0, 0, 5, 5);
            Should.Throw<ArgumentNullException>(() => _mapBuilder.AddAssociatedAreas(areaOnDest, areaOnSource));
        }
    }
}