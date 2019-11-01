using System;
using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMapBuilder
{
    public class WhenInitNewMap
    {
        private Area2D _destArea;
        private Area2D _sourceArea;
        private Area2D _emptyArea;
        private Area2DMapBuilder _mapBuilder;

        public WhenInitNewMap()
        {
            _mapBuilder = new Area2DMapBuilder();

            _emptyArea = Area2D.Empty;
            _sourceArea = Area2D.Create(0, 0, 10, 10);
            _destArea = Area2D.Create(0, 0, 10, 10);
        }

        [Fact]
        public void Should_Throw_EmptyAreaException_When_Dest_Is_Empty()
        {
            Should.Throw<EmptyAreaException>(() => _mapBuilder.InitNewMap(_emptyArea, _sourceArea));
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_Dest_Is_Null()
        {
            Should.Throw<ArgumentNullException>(() => _mapBuilder.InitNewMap(null, _sourceArea));
        }

        [Fact]
        public void Should_Throw_EmptyAreaException_When_Source_Is_Empty()
        {
            Should.Throw<EmptyAreaException>(() => _mapBuilder.InitNewMap(_destArea, _emptyArea));
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_Source_Is_Null()
        {
            Should.Throw<ArgumentNullException>(() => _mapBuilder.InitNewMap(_destArea, null));
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_Mapping_Is_Null()
        {
            IAreasMapping mapping = null;
            Should.Throw<ArgumentNullException>(() => _mapBuilder.InitNewMap(mapping));
        }
    }
}