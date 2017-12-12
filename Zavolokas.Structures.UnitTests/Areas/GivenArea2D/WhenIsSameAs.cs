using System;
using NUnit.Framework;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    [TestFixture]
    public class WhenIsSameAs
    {
        [Test]
        public void Should_Throw_ArgumentNullException_When_Area2_Is_Null()
        {
            var a1 = Area2D.Create(0, 0, 0, 0);
            Area2D a2 = null;

            Assert.Throws<ArgumentNullException>(() => a1.IsSameAs(a2));
        }

        [Test]
        public void Should_Return_True_When_Both_Are_Empty()
        {
            var a1 = Area2D.Create(0, 0, 0, 0);
            var a2 = Area2D.Create(12, 2, 0, 10);

            Assert.That(a1.IsSameAs(a2), Is.True);
        }

        [Test]
        public void Should_Return_False_When_Area1_Is_Empty()
        {
            var a1 = Area2D.Create(0, 0, 0, 0);
            var a2 = Area2D.Create(12, 2, 10, 10);

            Assert.That(a1.IsSameAs(a2), Is.False);
        }

        [Test]
        public void Should_Return_False_When_Area2_Is_Empty()
        {
            var a1 = Area2D.Create(12, 2, 10, 10);
            var a2 = Area2D.Create(0, 0, 0, 0);

            Assert.That(a1.IsSameAs(a2), Is.False);
        }

        [Test]
        public void Should_Return_False_When_Area1_Contains_Additional_Points()
        {
            var a1 = Area2D.Create(0, 0,
                new[]
                {
                    new byte[]{0,0,0,0,0,0,1 },
                    new byte[]{0,1,1,0,0,0,0 },
                    new byte[]{0,1,0,1,0,0,0 },
                    new byte[]{0,1,0,0,0,0,0 },
                    new byte[]{0,0,0,0,0,0,0 },
                    new byte[]{0,0,0,0,0,0,0 },
                    new byte[]{0,1,1,1,0,1,0 },
                    new byte[]{0,0,0,0,0,1,0 },
                    new byte[]{0,0,0,0,0,0,0 },
                });

            var a2 = Area2D.Create(0, 0,
                new[]
                {
                    new byte[]{0,0,0,0,0,0,0 },
                    new byte[]{0,1,1,0,0,0,0 },
                    new byte[]{0,1,0,1,0,0,0 },
                    new byte[]{0,1,0,0,0,0,0 },
                    new byte[]{0,0,0,0,0,0,0 },
                    new byte[]{0,0,0,0,0,0,0 },
                    new byte[]{0,1,1,1,0,1,0 },
                    new byte[]{0,0,0,0,0,1,0 },
                    new byte[]{0,0,0,0,0,0,0 },
                });


            Assert.That(a1.IsSameAs(a2), Is.False);
        }

        [Test]
        public void Should_Return_False_When_Area2_Contains_Additional_Points()
        {
            var a1 = Area2D.Create(0, 0,
                new[]
                {
                    new byte[]{0,0,0,0,0,0,0 },
                    new byte[]{0,1,1,0,0,0,0 },
                    new byte[]{0,1,0,1,0,0,0 },
                    new byte[]{0,1,0,0,0,0,0 },
                    new byte[]{0,0,0,0,0,0,0 },
                    new byte[]{0,0,0,0,0,0,0 },
                    new byte[]{0,1,1,1,0,1,0 },
                    new byte[]{0,0,0,0,0,1,0 },
                    new byte[]{0,0,0,0,0,0,0 },
                });

            var a2 = Area2D.Create(0, 0,
                new[]
                {
                    new byte[]{0,0,0,0,0,0,0 },
                    new byte[]{0,1,1,0,0,0,0 },
                    new byte[]{0,1,0,1,0,0,0 },
                    new byte[]{0,1,0,0,0,1,0 },
                    new byte[]{0,0,0,0,0,0,0 },
                    new byte[]{0,0,0,0,0,0,0 },
                    new byte[]{0,1,1,1,0,1,0 },
                    new byte[]{0,0,0,0,0,1,0 },
                    new byte[]{0,0,0,0,0,0,0 },
                });


            Assert.That(a1.IsSameAs(a2), Is.False);
        }

        [Test]
        public void Should_Return_True_When_All_Points_Are_Same()
        {
            var points = new[]
            {
                new byte[]{1,1,1},
                new byte[]{1,1,1},
                new byte[]{1,1,1}
            };

            var a1 = Area2D.Create(-1, -1, points);

            var a2 = Area2D.Create(-1, -1, 3, 3);

            Assert.That(a1.IsSameAs(a2), Is.True);
        }
    }
}
