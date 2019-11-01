using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Zavolokas.Math.Combinatorics;
using Xunit;
using Shouldly;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    public class WhenSubstract
    {
        [Fact]
        public void Should_Return_Empty_Area_When_Substract_From_Empty_Area()
        {
            var areaA = Area2D.Empty;
            var areaB = Area2D.Create(0, 0, 10, 10);

            var a = areaA.Substract(areaB);

            a.IsEmpty.ShouldBeTrue();
        }

        [Fact]
        public void Should_Return_Same_Area_When_Substract_Empty_Area()
        {
            var areaA = Area2D.Create(0, 0, 10, 10);
            var areaB = Area2D.Empty;

            var a = areaA.Substract(areaB);

            a.IsSameAs(areaA).ShouldBeTrue();
        }

        [Fact]
        public void Should_Return_Same_Area_When_Areas_Dont_Intersect()
        {
            var areaA = Area2D.Create(-2, -2, 2, 2);
            var areaB = Area2D.Create(2, 2, 3, 2);

            var a = areaA.Substract(areaB);

            a.IsSameAs(areaA).ShouldBeTrue();
        }

        [Fact]
        public void Should_Return_Intersection()
        {
            var areaA = Area2D.Create(0, 0, 3, 3);
            var areaB = Area2D.Create(1, 1, 10, 10);

            var a = areaA.Substract(areaB);

            var points = new[]
            {
                new byte[] {1, 1, 1},
                new byte[] {1, 0, 0},
                new byte[] {1, 0, 0},
            };

            var b = Area2D.Create(0, 0, points);

            a.IsSameAs(b).ShouldBeTrue();
        }

        [Fact]
        public void Should_Return_Empty_Area_When_Areas_Are_Same()
        {
            var areaA = Area2D.Create(3, 3, 100, 100);
            var areaB = Area2D.Create(3, 3, 100, 100);

            var a = areaA.Substract(areaB);

            a.IsEmpty.ShouldBeTrue();
        }

        [Fact]
        public void Should_Return_Empty_Area_When_Substracted_Area_Is_Subset()
        {
            var areaA = Area2D.Create(3, 3, 100, 100);
            var areaB = Area2D.Create(2, 2, 150, 150);

            var a = areaA.Substract(areaB);

            a.IsEmpty.ShouldBeTrue();
        }

        [Fact]
        public void Result_Should_Throw_ArgumentNullException_When_Area_Is_Null()
        {
            var areaA = Area2D.Create(14, 10, 44, 15);
            Assert.Throws<ArgumentNullException>(() => areaA.Intersect(null));
        }

        [Theory]
        [InlineData("256x128", "AreaSubstractTest", true)]
        //[Ignore("Don't know yet how to handle this properly on Travis CI")]
        public void Should_Intersect_Areas(string size, string testName, bool result)
        {
            var noDiffs = true;
            var ts = TestSet.Init(size);

            var areas = new List<Area2D>
            {
                Area2D.Create(0, 0, ts.Picture.Width, ts.Picture.Height),
                ts.Donors[0].ToArea(),
                ts.Donors[3].ToArea(),
                ts.Donors[4].ToArea(),
                ts.RemoveMarkup.ToArea()
            };

            var testCases = areas.GetAllCombinations()
                .OrderBy(x => x.Count())
                .ToArray();

            for (int i = 0; i < testCases.Length; i++)
            {
                var testCase = testCases[i].ToArray();
                var resultArea = testCase[0];
                for (var j = 1; j < testCase.Length; j++)
                {
                    var area = testCase[j];
                    resultArea = resultArea.Substract(area);
                }

                if (!resultArea.IsEmpty)
                {
                    SaveToOutput(resultArea, $"substraction{i}", testName, ts.Path);
                }
            }

            var reffiles = Directory.GetFiles($"{ts.Path}\\{testName}\\refs", "*.*", SearchOption.TopDirectoryOnly);
            var outfiles = Directory.GetFiles($"{ts.Path}\\{testName}\\output", "*.*", SearchOption.TopDirectoryOnly);

            if (reffiles.Length != outfiles.Length)
                noDiffs = false;

            foreach (var refFilePath in reffiles)
            {
                var refFileName = Path.GetFileName(refFilePath);
                var outFilePath = $"{ts.Path}\\{testName}\\output\\{refFileName}";

                if (!File.Exists(outFilePath))
                {
                    noDiffs = false;
                    continue;
                }

                var refArea = new Bitmap(refFilePath).ToArea();
                var outArea = new Bitmap(outFilePath).ToArea();

                if (!refArea.IsSameAs(outArea)) noDiffs = false;
            }

            noDiffs.ShouldBe(result);
        }

        private static void SaveToOutput(Area2D area, string fileName, string testName, string testPath)
        {
            var bmp = area.ToBitmap(Color.Red);
            var dir = $"{testPath}\\{testName}\\output";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            var path = $"{dir}\\{fileName}.png";
            bmp.Save(path, ImageFormat.Png);
        }
    }
}