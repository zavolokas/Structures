using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Primitives.GivenVector2D
{
   [TestFixture]
   public class WhenCreateVector2D
   {
      [Test]
      public void ShouldSetValues()
      {
         const double x = 23;
         const double y = 1.5;
         var vector = new Vector2D(x,y);

         Assert.AreEqual(x, vector.X);
         Assert.AreEqual(y, vector.Y);
      }
   }
}