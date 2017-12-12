using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    [TestFixture]
    public class WhenGetElementsCount
    {
        [TestCase(0, 0,
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            ExpectedResult = 0)]
        [TestCase(-2, 0,
                  new byte[] { 1, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 1, 0, 1, 0, 0, 0 },
                  new byte[] { 0, 1, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                  new byte[] { 0, 0, 1, 0, 0, 0, 0, 1 },
            ExpectedResult = 6)]
        [TestCase(0, -5,
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
                  new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 },
            ExpectedResult = 40)]
        public int Should_Return_Correct_Number_Of_Elements(int x, int y, byte[] r1, byte[] r2, byte[] r3, byte[] r4, byte[] r5)
        {
            var markup = new [] { r1, r2, r3, r4, r5 };
            var area = Area2D.Create(x, y, markup);
            return area.ElementsCount;
        }

        [TestCase(0, 0, 5, 5, ExpectedResult = 25)]
        [TestCase(0, 0, 0, 0, ExpectedResult = 0)]
        [TestCase(0, 0, 5, 0, ExpectedResult = 0)]
        [TestCase(0, 0, 0, 5, ExpectedResult = 0)]
        [TestCase(0, 0, 1, 1, ExpectedResult = 1)]
        [TestCase(2, 3, 2, 2, ExpectedResult = 4)]
        [TestCase(2, 3, 1, 1, ExpectedResult = 1)]
        [TestCase(-5, 2, 2, 1, ExpectedResult = 2)]
        public int Should_Return_Correct_ElementsCount(int x, int y, int width, int height)
        {
            var area = Area2D.Create(x, y, width, height);
            return area.ElementsCount;
        }
    }
}