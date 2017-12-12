using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Primitives.GivenVector2D
{
   [TestFixture]
   public class WhenCompareVectors
   {
      [Test]
      public void RightShouldBeGreater()
      {
         var v1 = new Vector2D(1, 1);
         var v2 = new Vector2D(2, 2);
         
         Assert.IsTrue(v1 < v2);
      }


      [Test]
      public void LeftShouldBeGreater()
      {
         var v1 = new Vector2D(1, 5);
         var v2 = new Vector2D(2, 2);

         Assert.IsTrue(v1 > v2);
      }

      [Test]
      public void ShouldBeEqual()
      {
         var v1 = new Vector2D(1, 5);
         var v2 = new Vector2D(1, 5);

         Assert.IsTrue(v1 == v2);
      }

      [Test]
      public void ShouldBeNotEqual()
      {
         var v1 = new Vector2D(2, 5);
         var v2 = new Vector2D(1, 2);

         Assert.IsFalse(v1 == v2);
      }

      [Test]
      public void ShouldBeNotEqual2()
      {
         var v1 = new Vector2D(2, 5);
         var v2 = new Vector2D(1, 5);

         Assert.IsTrue(v1 != v2);
      }

      [Test]
      public void ShouldBeEqual2()
      {
         var v1 = new Vector2D(2, 5);
         var v2 = new Vector2D(2, 5);

         Assert.IsFalse(v1 != v2);
      }
   }
}
