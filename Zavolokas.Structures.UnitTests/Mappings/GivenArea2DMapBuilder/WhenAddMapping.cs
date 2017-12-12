using System;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMapBuilder
{
    [TestFixture]
    public class WhenAddMapping
    {
        private Area2D _destArea;
        private Area2D _sourceArea;
        private Area2DMap _mapping;
        private Area2DMapBuilder _mapBuilder;

        [SetUp]
        public void Setup()
        {
            _mapBuilder = new Area2DMapBuilder();

            _sourceArea = Area2D.Create(0, 0, 10, 10);
            _destArea = Area2D.Create(0, 0, 10, 10);

            _mapping = _mapBuilder.InitNewMap(_destArea, _sourceArea)
                .Build();

            _mapBuilder = new Area2DMapBuilder();
        }

        [Test]
        public void Should_Throw_MapIsNotInitializedException_When_Called_Before_InitMap_Call()
        {
            Assert.Throws<MapIsNotInitializedException>(() => _mapBuilder.AddMapping(_mapping));
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_Mapping_Is_Null()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            Assert.Throws<ArgumentNullException>(() => _mapBuilder.AddMapping(null));
        }
    }
}