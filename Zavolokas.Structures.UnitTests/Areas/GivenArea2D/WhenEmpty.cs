using Shouldly;
using System.Linq;
using Xunit;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    public class WhenEmpty
    {
        [Fact]
        public void Should_Return_Area_With_Zero_ElementsCount()
        {
            var area = Area2D.Empty;
            area.ElementsCount.ShouldBe(0);
        }

        [Fact]
        public void Should_Return_Area_IsEmpty_Set_To_True()
        {
            var area = Area2D.Empty;
            area.IsEmpty.ShouldBeTrue();
        }

        [Fact]
        public void Should_Return_Area_With_Empty_Points_Enumerator()
        {
            var area = Area2D.Empty;
            var pe = area.Points;
            pe.Count().ShouldBe(0);
        }

        [Fact]
        public void Should_Return_Area_With_Zero_Bound_Size()
        {
            var area = Area2D.Empty;
            area.Bound.Width.ShouldBe(0);
            area.Bound.Height.ShouldBe(0);
        }
    }
}
