using System;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMapBuilder
{
    [TestFixture]
    public class WhenReduceDestArea
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
            var reducedArea = Area2D.Create(0, 0, 5, 5);
            Assert.Throws<MapIsNotInitializedException>(() => _mapBuilder.ReduceDestArea(reducedArea));
        }

        [Test]
        public void Should_Throw_EmptyAreaException_When_ReducedArea_IsEmpty()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            var reducedArea = Area2D.Empty;
            Assert.Throws<EmptyAreaException>(() => _mapBuilder.ReduceDestArea(reducedArea));
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_ReducedArea_Is_Null()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            Assert.Throws<ArgumentNullException>(() => _mapBuilder.ReduceDestArea(null));
        }

        [Test]
        public void Should_Throw_AreaRemovedException_When_Intersection_Of_ReducedArea_And_Dest_IsEmpty()
        {
            _destArea = Area2D.Create(0, 0, 10, 10);
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            
            var reducedArea = Area2D.Create(15, 15, 20, 20);
            Assert.Throws<AreaRemovedException>(() => _mapBuilder.ReduceDestArea(reducedArea));
        }

        [Test]
        public void Should_Throw_AreaRemovedException_When_Intersection_Of_ReducedAreas_And_Dest_IsEmpty()
        {
            _destArea = Area2D.Create(0, 0, 10, 10);
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            
            var reducedArea1 = Area2D.Create(0, 0, 5, 5);
            var reducedArea2 = Area2D.Create(5, 5, 5, 5);

            _mapBuilder.ReduceDestArea(reducedArea1);
            Assert.Throws<AreaRemovedException>(() => _mapBuilder.ReduceDestArea(reducedArea2));
        }
    }
}
