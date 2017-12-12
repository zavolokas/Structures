using System.Linq;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    [TestFixture]
    public class WhenEmpty
    {
        [Test]
        public void Should_Return_Area_With_Zero_ElementsCount()
        {
            var area = Area2D.Empty;
            Assert.That(area.ElementsCount, Is.EqualTo(0));
        }

        [Test]
        public void Should_Return_Area_IsEmpty_Set_To_True()
        {
            var area = Area2D.Empty;
            Assert.That(area.IsEmpty, Is.True);
        }

        [Test]
        public void Should_Return_Area_With_Empty_Points_Enumerator()
        {
            var area = Area2D.Empty;
            var pe = area.Points;
            Assert.That(pe.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Should_Return_Area_With_Zero_Bound_Size()
        {
            var area = Area2D.Empty;
            Assert.That(area.Bound.Width, Is.EqualTo(0));
            Assert.That(area.Bound.Height, Is.EqualTo(0));
        }
    }
}
