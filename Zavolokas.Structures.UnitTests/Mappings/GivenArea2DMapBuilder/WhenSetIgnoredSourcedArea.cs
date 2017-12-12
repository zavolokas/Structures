using System;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMapBuilder
{
    [TestFixture]
    public class WhenSetIgnoredSourcedArea
    {
        private Area2D _destArea;
        private Area2D _sourceArea;
        private Area2DMapBuilder _mapBuilder;

        [SetUp]
        public void Setup()
        {
            _mapBuilder = new Area2DMapBuilder();

            _sourceArea = Area2D.Create(0, 0, 10, 10);
            _destArea = Area2D.Create(0, 0, 10, 10);
        }

        [Test]
        public void Should_Throw_MapIsNotInitializedException_When_Called_Before_InitMap_Call()
        {
            var ignoreArea = Area2D.Create(0, 0, 5, 5);
            Assert.Throws<MapIsNotInitializedException>(() => _mapBuilder.SetIgnoredSourcedArea(ignoreArea));
        }

        [Test]
        public void Should_Throw_EmptyAreaException_When_IgnoreArea_IsEmpty()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            var ignoreArea = Area2D.Empty;
            Assert.Throws<EmptyAreaException>(() => _mapBuilder.SetIgnoredSourcedArea(ignoreArea));
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_IgnoreArea_Is_Null()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            Area2D ignoreArea = null;
            Assert.Throws<ArgumentNullException>(() => _mapBuilder.SetIgnoredSourcedArea(ignoreArea));
        }

        [Test]
        public void Should_Throw_AreaRemovedException_When_IgnoreArea_Is_Equal_To_Source_Area()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            Assert.Throws<AreaRemovedException>(() => _mapBuilder.SetIgnoredSourcedArea(_sourceArea));
        }

        [Test]
        public void Should_Throw_AreaRemovedException_When_IgnoreArea_Bigger_Than_Source_Area()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            var ignoreArea = Area2D.Create(-5, -5, 50, 50);
            Assert.Throws<AreaRemovedException>(() => _mapBuilder.SetIgnoredSourcedArea(ignoreArea));
        }
    }
}
