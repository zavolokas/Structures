using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Primitives.GivenInterval
{
    public class WhenJoin
    {
        [Fact]
        public void Should_Return_Empty_When_Both_Left_And_Right_Interval_Empty()
        {
            var left = Interval.Empty;
            var right = Interval.Empty;

            var result = left.Join(right);

            result.Length.ShouldBe(1);
            result[0].ShouldBe(Interval.Empty);
        }

        [Fact]
        public void Should_Return_Right_When_Left_Interval_Empty()
        {
            var left = Interval.Empty;
            var right = new Interval { Start = 3, End = 6 };

            var result = left.Join(right);

            result.Length.ShouldBe(1);
            result[0].ShouldBe(right);
        }

        [Fact]
        public void Should_Return_Left_When_Rigth_Interval_Empty()
        {
            var left = new Interval { Start = 3, End = 6 };
            var right = Interval.Empty;

            var result = left.Join(right);

            result.Length.ShouldBe(1);
            result[0].ShouldBe(left);
        }

        [Fact]
        public void Should_Return_Both_Left_And_Right_When_Intervals_Dont_Intersect()
        {
            var left = new Interval { Start = 4, End = 7 };
            var right = new Interval { Start = 0, End = 3 };

            var result = left.Join(right);

            result.Length.ShouldBe(2);
            result[0].ShouldBe(right);
            result[1].ShouldBe(left);
        }

        [Fact]
        public void Should_Return_Both_Left_And_Right_When_Intervals_Dont_Intersect2()
        {
            var left = new Interval { Start = 0, End = 2 };
            var right = new Interval { Start = 4, End = 8 };

            var result = left.Join(right);

            result.Length.ShouldBe(2);
            result[0].ShouldBe(left);
            result[1].ShouldBe(right);
        }

        [Fact]
        public void Should_Return_Same_Interval_When_Intervals_Identic()
        {
            var left = new Interval { Start = 2, End = 5 };
            var right = new Interval { Start = 2, End = 5 };

            var result = left.Join(right);

            result.Length.ShouldBe(1);
            result[0].ShouldBe(left);
        }

        [Fact]
        public void Should_Return_Left_When_Left_Includes_Right()
        {
            var left = new Interval { Start = 1, End = 7 };
            var right = new Interval { Start = 2, End = 5 };

            var result = left.Join(right);

            result.Length.ShouldBe(1);
            result[0].ShouldBe(left);
        }

        [Fact]
        public void Should_Return_Left_When_Left_Includes_Right_And_Starts_At_Same_Point()
        {
            var left = new Interval { Start = 1, End = 7 };
            var right = new Interval { Start = 1, End = 4 };

            var result = left.Join(right);

            result.Length.ShouldBe(1);
            result[0].ShouldBe(left);
        }

        [Fact]
        public void Should_Return_Left_When_Left_Includes_Right_And_Ends_At_Same_Point()
        {
            var left = new Interval { Start = 1, End = 7 };
            var right = new Interval { Start = 4, End = 7 };

            var result = left.Join(right);

            result.Length.ShouldBe(1);
            result[0].ShouldBe(left);
        }

        [Fact]
        public void Should_Return_Right_When_Right_Includes_Left()
        {
            var left = new Interval { Start = 3, End = 5 };
            var right = new Interval { Start = 1, End = 7 };

            var result = left.Join(right);

            result.Length.ShouldBe(1);
            result[0].ShouldBe(right);
        }

        [Fact]
        public void Should_Return_Right_When_Right_Includes_Left_And_Starts_At_Same_Point()
        {
            var left = new Interval { Start = 1, End = 4 };
            var right = new Interval { Start = 1, End = 7 };

            var result = left.Join(right);

            result.Length.ShouldBe(1);
            result[0].ShouldBe(right);
        }

        [Fact]
        public void Should_Return_Right_When_Right_Includes_Left_And_Ends_At_Same_Point()
        {
            var left = new Interval { Start = 4, End = 7 };
            var right = new Interval { Start = 1, End = 7 };

            var result = left.Join(right);

            result.Length.ShouldBe(1);
            result[0].ShouldBe(right);
        }

        [Fact]
        public void Should_Return_Joined_Interval_When_Overlap1()
        {
            var left = new Interval { Start = 2, End = 5 };
            var right = new Interval { Start = 4, End = 7 };
            var r = new Interval { Start = 2, End = 7 };

            var result = left.Join(right);

            result.Length.ShouldBe(1);
            result[0].ShouldBe(r);
        }

        [Fact]
        public void Should_Return_Joined_Interval_When_Overlap2()
        {
            var left = new Interval { Start = 5, End = 12 };
            var right = new Interval { Start = 0, End = 9 };
            var r = new Interval { Start = 0, End = 12 };

            var result = left.Join(right);

            result.Length.ShouldBe(1);
            result[0].ShouldBe(r);
        }

        [Fact]
        public void Should_Return_Joined_Interval_When_Overlap3()
        {
            var left = new Interval { Start = -5, End = 5 };
            var right = new Interval { Start = -1, End = 8 };
            var r = new Interval { Start = -5, End = 8 };

            var result = left.Join(right);

            result.Length.ShouldBe(1);
            result[0].ShouldBe(r);
        }
    }
}
