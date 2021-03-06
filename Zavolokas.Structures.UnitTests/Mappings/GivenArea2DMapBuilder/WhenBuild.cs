﻿using System;
using System.Linq;
using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Mappings.GivenArea2DMapBuilder
{
    public class WhenBuild
    {
        private Area2D _destArea;
        private Area2D _sourceArea;
        private Area2D _emptyArea;
        private Area2DMapBuilder _mapBuilder;
        private Area2DMap _initialMapping;

        public WhenBuild()
        {
            _mapBuilder = new Area2DMapBuilder();

            _emptyArea = Area2D.Empty;
            _sourceArea = Area2D.Create(10, 10, 10, 10);
            _destArea = Area2D.Create(0, 0, 10, 10);

            var areas = new[]
            {
                new Tuple<Area2D, Area2D>(Area2D.Create(0, 0, 5, 5), Area2D.Create(3, 3, 5, 5)),
                new Tuple<Area2D, Area2D>(Area2D.Create(5, 0, 5, 5), Area2D.Create(0, 0, 10, 10)),
                new Tuple<Area2D, Area2D>(Area2D.Create(0, 5, 5, 5), Area2D.Create(5, 2, 6, 6)),
                new Tuple<Area2D, Area2D>(Area2D.Create(5, 5, 5, 5), Area2D.Create(0, 2, 8, 6)),
            };

            _initialMapping = new Area2DMap(areas, _emptyArea);
        }

        [Fact]
        public void Should_Throw_MapIsNotInitializedException_When_Called_Before_InitMap_Call()
        {
            Assert.Throws<MapIsNotInitializedException>(() => _mapBuilder.Build());
        }

        [Fact]
        public void Every_Dest_Point_From_Result_Should_Present_In_DestArea()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            var mapping = _mapBuilder.Build();

            foreach (var destPoint in mapping.DestPoints)
            {
                _destArea.Points.Contains(destPoint).ShouldBeTrue();
            }
        }

        [Fact]
        public void Result_Should_Contain_Every_Point_From_Dest_Area()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            var mapping = _mapBuilder.Build();

            foreach (var point in _destArea.Points)
            {
                mapping.DestPoints.Contains(point).ShouldBeTrue();
            }
        }

        [Fact]
        public void Every_Dest_Point_Of_Result_Should_Be_Associated_With_Source_Area()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            var mapping = _mapBuilder.Build();

            foreach (var destPoint in mapping.DestPoints)
            {
                mapping.GetPointSourceArea(destPoint).IsSameAs(_sourceArea).ShouldBeTrue();
            }
        }

        [Fact]
        public void New_Mapping_Should_Contain_DestPoints_That_Present_In_The_Initialization_Mapping()
        {
            _mapBuilder.InitNewMap(_initialMapping);
            var newMapping = _mapBuilder.Build();

            foreach (var destPoint in newMapping.DestPoints)
            {
                _initialMapping.DestPoints.Contains(destPoint).ShouldBeTrue();
            }
        }

        [Fact]
        public void Every_Point_From_InitMapping_Should_Present_In_Result_Mapping()
        {
            _mapBuilder.InitNewMap(_initialMapping);
            var newMapping = _mapBuilder.Build();

            foreach (var destPoint in _initialMapping.DestPoints)
            {
                newMapping.DestPoints.Contains(destPoint).ShouldBeTrue();
            }
        }

        [Fact]
        public void DestPoints_Of_Result_Mapping_Should_Point_To_The_Same_Area_As_The_Initialization_Mapping()
        {
            _mapBuilder.InitNewMap(_initialMapping);
            var newMapping = _mapBuilder.Build();

            foreach (var destPoint in newMapping.DestPoints)
            {
                var newMappingDestPointSourceArea = newMapping.GetPointSourceArea(destPoint);
                var initMappingDestPointSourceArea = _initialMapping.GetPointSourceArea(destPoint);
                newMappingDestPointSourceArea.IsSameAs(initMappingDestPointSourceArea).ShouldBeTrue();
            }
        }

        [Fact]
        public void Should_Discard_All_The_Settings_That_Were_Made_Before_Init()
        {
            _mapBuilder.InitNewMap(_initialMapping);
            _mapBuilder.SetIgnoredSourcedArea(Area2D.Create(0, 0, 5, 5));
            _mapBuilder.ReduceDestArea(Area2D.Create(0, 0, 5, 5));
            _mapBuilder.AddAssociatedAreas(Area2D.Create(0, 0, 2, 2), Area2D.Create(3, 0, 2, 2));
            _mapBuilder.AddAssociatedAreas(Area2D.Create(1, 1, 2, 2), Area2D.Create(1, 3, 2, 2));

            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            var mapping = _mapBuilder.Build();

            foreach (var destPoint in mapping.DestPoints)
            {
                _destArea.GetPointIndex(destPoint).ShouldBeGreaterThan(-1);
                mapping.GetPointSourceArea(destPoint).IsSameAs(_sourceArea).ShouldBeTrue();
            }

            foreach (var point in _destArea.Points)
            {
                mapping.DestPoints.Contains(point).ShouldBeTrue();
                mapping.GetPointSourceArea(point).IsSameAs(_sourceArea).ShouldBeTrue();
            }
        }

        [Fact]
        public void Should_Return_Mapping_Where_Source_Areas_Contain_No_IgnoreArea()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            _mapBuilder.AddAssociatedAreas(Area2D.Create(0, 0, 2, 2), Area2D.Create(0, 0, 5, 5));
            _mapBuilder.AddAssociatedAreas(Area2D.Create(2, 2, 2, 2), Area2D.Create(5, 5, 5, 5));
            var ignoreArea = Area2D.Create(3, 3, 5, 5);
            _mapBuilder.SetIgnoredSourcedArea(ignoreArea);
            var mapping = _mapBuilder.Build();

            foreach (var destPoint in mapping.DestPoints)
            {
                var pointSourceArea = mapping.GetPointSourceArea(destPoint);
                var intersection = pointSourceArea.Intersect(ignoreArea);
                intersection.IsEmpty.ShouldBeTrue();
            }
        }

        [Fact]
        public void Should_Return_Mapping_Where_Points_Outside_Mapping_Mapped_To_EmptyArea()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);
            var mapping = _mapBuilder.Build();

            mapping.GetPointSourceArea(new Point(-10, -10)).IsEmpty.ShouldBeTrue();
        }

        [Fact]
        public void Should_Not_Take_Into_Account_Donors_That_Point_To_Ignored_Area()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);

            var donor1Dest = Area2D.Create(0, 0, 2, 2);
            var donor1Source = Area2D.Create(3, 3, 3, 2);
            _mapBuilder.AddAssociatedAreas(donor1Dest, donor1Source);

            var donor2Dest = Area2D.Create(2, 2, 2, 2);
            var donor2Source = Area2D.Create(3, 5, 3, 2);
            _mapBuilder.AddAssociatedAreas(donor2Dest, donor2Source);

            var ignoreArea = Area2D.Create(2, 2, 6, 6);
            _mapBuilder.SetIgnoredSourcedArea(ignoreArea);

            var mapping = _mapBuilder.Build();

            foreach (var destPoint in donor1Dest.Points)
            {
                var pointSourceArea = mapping.GetPointSourceArea(destPoint);
                pointSourceArea.IsSameAs(donor1Source).ShouldBeFalse();
            }

            foreach (var destPoint in donor2Dest.Points)
            {
                var pointSourceArea = mapping.GetPointSourceArea(destPoint);
                pointSourceArea.IsSameAs(donor2Source).ShouldBeFalse();
            }
        }

        [Fact]
        public void Should_Use_Only_Last_Set_IgnoreArea()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);

            var donor1Dest = Area2D.Create(0, 0, 2, 2);
            var donor1Source = Area2D.Create(3, 3, 3, 2);
            _mapBuilder.AddAssociatedAreas(donor1Dest, donor1Source);

            var donor2Dest = Area2D.Create(2, 2, 2, 2);
            var donor2Source = Area2D.Create(3, 5, 3, 2);
            _mapBuilder.AddAssociatedAreas(donor2Dest, donor2Source);

            var ignoreArea1 = Area2D.Create(2, 2, 6, 6);
            _mapBuilder.SetIgnoredSourcedArea(ignoreArea1);

            var ignoreArea2 = Area2D.Create(3, 5, 3, 2);
            _mapBuilder.SetIgnoredSourcedArea(ignoreArea2);

            var mapping = _mapBuilder.Build();

            foreach (var destPoint in donor1Dest.Points)
            {
                var pointSourceArea = mapping.GetPointSourceArea(destPoint);
                pointSourceArea.IsSameAs(donor1Source).ShouldBeTrue();
            }

            foreach (var destPoint in donor2Dest.Points)
            {
                var pointSourceArea = mapping.GetPointSourceArea(destPoint);
                pointSourceArea.IsSameAs(donor2Source).ShouldBeFalse();
            }
        }

        [Fact]
        public void Mapping_Source_Area_Should_Be_Same_As_Source_Area_But_Without_Last_Set_IgnoreArea()
        {
            _sourceArea = Area2D.Create(0, 0, 10, 10);
            _mapBuilder.InitNewMap(_destArea, _sourceArea);

            var donor1Dest = Area2D.Create(0, 0, 2, 2);
            var donor1Source = Area2D.Create(3, 3, 3, 2);
            _mapBuilder.AddAssociatedAreas(donor1Dest, donor1Source);

            var donor2Dest = Area2D.Create(2, 2, 2, 2);
            var donor2Source = Area2D.Create(3, 5, 3, 2);
            _mapBuilder.AddAssociatedAreas(donor2Dest, donor2Source);

            var ignoreArea1 = Area2D.Create(2, 2, 6, 6);
            _mapBuilder.SetIgnoredSourcedArea(ignoreArea1);

            var ignoreArea2 = Area2D.Create(3, 5, 3, 2);
            _mapBuilder.SetIgnoredSourcedArea(ignoreArea2);

            var mapping = _mapBuilder.Build();

            var source = Area2D.Empty;
            foreach (var destPoint in mapping.DestPoints)
            {
                var pointSourceArea = mapping.GetPointSourceArea(destPoint);
                source = source.Join(pointSourceArea);
            }

            var expectedSource = _sourceArea.Substract(ignoreArea2);

            source.IsSameAs(expectedSource).ShouldBeTrue();
        }

        [Fact]
        public void All_Dest_Points_Of_Result_Must_Be_In_ReducedArea()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);

            var donor1Dest = Area2D.Create(2, 2, 2, 2);
            var donor1Source = Area2D.Create(3, 3, 3, 2);
            _mapBuilder.AddAssociatedAreas(donor1Dest, donor1Source);

            var donor2Dest = Area2D.Create(4, 2, 2, 2);
            var donor2Source = Area2D.Create(3, 5, 3, 2);
            _mapBuilder.AddAssociatedAreas(donor2Dest, donor2Source);

            var reduceArea = Area2D.Create(1, 1, 6, 6);
            _mapBuilder.ReduceDestArea(reduceArea);

            var mapping = _mapBuilder.Build();

            foreach (var destPoint in mapping.DestPoints)
            {
                reduceArea.Points.Contains(destPoint).ShouldBeTrue();
            }
        }

        [Fact]
        public void Dest_Points_Of_Result_Must_Be_In_Intersection_Of_ReducedAreas_And_Dest()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);

            var donor1Dest = Area2D.Create(2, 2, 2, 2);
            var donor1Source = Area2D.Create(3, 3, 3, 2);
            _mapBuilder.AddAssociatedAreas(donor1Dest, donor1Source);

            var donor2Dest = Area2D.Create(4, 2, 2, 2);
            var donor2Source = Area2D.Create(3, 5, 3, 2);
            _mapBuilder.AddAssociatedAreas(donor2Dest, donor2Source);

            var reduceArea1 = Area2D.Create(-5, -5, 13, 13);
            _mapBuilder.ReduceDestArea(reduceArea1);

            var reduceArea2 = Area2D.Create(-1, -1, 16, 16);
            _mapBuilder.ReduceDestArea(reduceArea2);

            var mapping = _mapBuilder.Build();

            var finalReduceArea = reduceArea1
                .Intersect(reduceArea2)
                .Intersect(_destArea);

            foreach (var destPoint in mapping.DestPoints)
            {
                finalReduceArea.Points.Contains(destPoint).ShouldBeTrue();
            }
        }

        [Fact]
        public void All_Points_Of_Intersection_Of_ReducedAreas_And_Dest_Must_Be_In_Result()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);

            var donor1Dest = Area2D.Create(2, 2, 2, 2);
            var donor1Source = Area2D.Create(3, 3, 3, 2);
            _mapBuilder.AddAssociatedAreas(donor1Dest, donor1Source);

            var donor2Dest = Area2D.Create(4, 2, 2, 2);
            var donor2Source = Area2D.Create(3, 5, 3, 2);
            _mapBuilder.AddAssociatedAreas(donor2Dest, donor2Source);

            var reduceArea1 = Area2D.Create(-5, -5, 13, 13);
            _mapBuilder.ReduceDestArea(reduceArea1);

            var reduceArea2 = Area2D.Create(-1, -1, 16, 16);
            _mapBuilder.ReduceDestArea(reduceArea2);

            var mapping = _mapBuilder.Build();

            var finalReduceArea = reduceArea1
                .Intersect(reduceArea2)
                .Intersect(_destArea);

            foreach (var point in finalReduceArea.Points)
            {
                mapping.DestPoints.Contains(point).ShouldBeTrue();
            }
        }

        [Fact]
        public void DestPoints_That_Reside_In_Multiple_AssociatedAreas_Should_Point_To_The_Last_Added_Source_Area()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);

            var donor1Dest = Area2D.Create(0, 0, 3, 3);
            var donor1Source = Area2D.Create(0, 0, 3, 3);
            _mapBuilder.AddAssociatedAreas(donor1Dest, donor1Source);

            var donor2Dest = Area2D.Create(2, 2, 3, 3);
            var donor2Source = Area2D.Create(3, 3, 3, 3);
            _mapBuilder.AddAssociatedAreas(donor2Dest, donor2Source);

            var commonDest = donor1Dest.Intersect(donor2Dest);

            var mapping = _mapBuilder.Build();

            foreach (var point in commonDest.Points)
            {
                var source = mapping.GetPointSourceArea(point);
                source.IsSameAs(donor2Source).ShouldBeTrue();
            }
        }

        [Fact(Skip = "Wrong assumption, this area will point to the last added")]
        public void DestPoints_That_Reside_In_Multiple_AssociatedAreas_Should_Point_To_Joint_Source_Area()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);

            var donor1Dest = Area2D.Create(0, 0, 3, 3);
            var donor1Source = Area2D.Create(0, 0, 3, 3);
            _mapBuilder.AddAssociatedAreas(donor1Dest, donor1Source);

            var donor2Dest = Area2D.Create(2, 2, 3, 3);
            var donor2Source = Area2D.Create(3, 3, 3, 3);
            _mapBuilder.AddAssociatedAreas(donor2Dest, donor2Source);

            var jointSource = donor1Source.Join(donor2Source);
            var commonDest = donor1Dest.Intersect(donor2Dest);

            var mapping = _mapBuilder.Build();

            foreach (var point in commonDest.Points)
            {
                var source = mapping.GetPointSourceArea(point);
                source.IsSameAs(jointSource).ShouldBeTrue();
            }
        }

        [Fact]
        public void DestPoints_That_Reside_Only_In_One_AssociatedAreas_Should_Point_To_Its_SourceArea()
        {
            _mapBuilder.InitNewMap(_destArea, _sourceArea);

            var donor1Dest = Area2D.Create(0, 0, 3, 3);
            var donor1Source = Area2D.Create(0, 0, 3, 3);
            _mapBuilder.AddAssociatedAreas(donor1Dest, donor1Source);

            var donor2Dest = Area2D.Create(2, 2, 3, 3);
            var donor2Source = Area2D.Create(3, 3, 3, 3);
            _mapBuilder.AddAssociatedAreas(donor2Dest, donor2Source);

            var commonDest = donor1Dest.Intersect(donor2Dest);

            var mapping = _mapBuilder.Build();

            var uniqueDest1 = commonDest.Substract(donor2Dest);
            foreach (var point in uniqueDest1.Points)
            {
                var source = mapping.GetPointSourceArea(point);
                source.IsSameAs(donor1Source).ShouldBeTrue();
            }

            var uniqueDest2 = commonDest.Substract(donor1Dest);
            foreach (var point in uniqueDest2.Points)
            {
                var source = mapping.GetPointSourceArea(point);
                source.IsSameAs(donor2Source).ShouldBeTrue();
            }
        }
    }
}