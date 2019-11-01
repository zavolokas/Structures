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
    public class WhenJoin
    {
        [Fact]
        public void Result_Should_Throw_ArgumentNullException_When_Area_Is_Null()
        {
            var areaA = Area2D.Create(14, 10, 44, 15);
            Should.Throw<ArgumentNullException>(() => areaA.Join(null));
        }

        [Fact]
        public void Result_Should_Be_Equal_To_Initial_Area_When_Join_With_Empty_Area()
        {
            var area = Area2D.Create(14, 10, 44, 15);
            var emptyArea = Area2D.Create(0, 0, 0, 0);
            var joinedArea = area.Join(emptyArea);
            joinedArea.IsSameAs(area).ShouldBeTrue();
        }

        [Fact]
        public void Result_Should_Be_Equal_To_Joined_Area_When_Join_With_Empty_Area()
        {
            var area = Area2D.Create(14, 10, 44, 15);
            var emptyArea = Area2D.Create(0, 0, 0, 0);
            var joinedArea = emptyArea.Join(area);
            joinedArea.IsSameAs(area).ShouldBeTrue();
        }

        [Fact]
        public void Result_Should_Contain_Points_From_Both_Areas()
        {
            var areaA = Area2D.Create(0, 0, 10, 10);
            var areaB = Area2D.Create(15, 0, 10, 10);

            var result = areaA.Join(areaB);

            foreach (var point in areaA.Points)
            {
                result.GetPointIndex(point).ShouldBeGreaterThan(-1);
            }

            foreach (var point in areaB.Points)
            {
                result.GetPointIndex(point).ShouldBeGreaterThan(-1);
            }
        }

        [Theory]
        [InlineData("256x128", "AreaJoinTest", true, Skip = "Don't know yet how to handle this properly on Travis CI")]
        public void Should_Intersect_Areas(string size, string testName, bool result)
        {
            var noDiffs = true;
            var ts = TestSet.Init(size);

            var areas = new List<Area2D>
            {
                ts.Donors[0].ToArea(),
                ts.Donors[1].ToArea(),
                ts.Donors[3].ToArea(),
                ts.Donors[4].ToArea(),
                ts.RemoveMarkup.ToArea()
            };

            var testCases = areas.GetAllCombinations()
                .OrderBy(x => x.Count())
                .ToArray();

            for (var i = 0; i < testCases.Length; i++)
            {
                var testCase = testCases[i].ToArray();
                var resultArea = testCase[0];
                for (var j = 1; j < testCase.Length; j++)
                {
                    var area = testCase[j];
                    resultArea = resultArea.Join(area);
                }

                if (!resultArea.IsEmpty)
                {
                    SaveToOutput(resultArea, $"jointarea{i}", testName, ts.Path);
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
