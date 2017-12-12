using System;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMapBuilder
{
    [TestFixture]
    public class WhenInitNewMap
    {
        private Area2D _destArea;
        private Area2D _sourceArea;
        private Area2D _emptyArea;
        private Area2DMapBuilder _mapBuilder;

        [SetUp]
        public void Setup()
        {
            _mapBuilder = new Area2DMapBuilder();

            _emptyArea = Area2D.Empty;
            _sourceArea = Area2D.Create(0, 0, 10, 10);
            _destArea = Area2D.Create(0, 0, 10, 10);
        }

        [Test]
        public void Should_Throw_EmptyAreaException_When_Dest_Is_Empty()
        {
            Assert.Throws<EmptyAreaException>(() => _mapBuilder.InitNewMap(_emptyArea, _sourceArea));
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_Dest_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => _mapBuilder.InitNewMap(null, _sourceArea));
        }

        [Test]
        public void Should_Throw_EmptyAreaException_When_Source_Is_Empty()
        {
            Assert.Throws<EmptyAreaException>(() => _mapBuilder.InitNewMap(_destArea, _emptyArea));
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_Source_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => _mapBuilder.InitNewMap(_destArea, null));
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_Mapping_Is_Null()
        {
            IAreasMapping mapping = null;
            Assert.Throws<ArgumentNullException>(() => _mapBuilder.InitNewMap(mapping));
        }
    }
}