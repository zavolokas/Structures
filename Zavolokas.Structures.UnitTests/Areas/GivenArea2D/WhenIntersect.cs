using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Zavolokas.Math.Combinatorics;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    [TestFixture]
    public class WhenIntersect
    {
        [Test]
        public void Result_Should_Contain_Only_Points_That_Present_In_Both_Areas()
        {
            var areaA = Area2D.Create(10, 15, 24, 35);
            var areaB = Area2D.Create(-5, -6, 30, 30);
            var areaC = areaA.Intersect(areaB);

            for (var i = 0; i < areaC.ElementsCount; i++)
            {
                var p = areaC[i];
                Assert.That(areaA.GetPointIndex(p) > -1);
                Assert.That(areaB.GetPointIndex(p) > -1);
            }
        }

        [Test]
        public void Result_Should_Contain_All_Points_That_Present_In_Both_Areas()
        {
            var areaA = Area2D.Create(10, 15, 24, 35);
            var areaB = Area2D.Create(-5, -6, 30, 30);
            var areaC = areaA.Intersect(areaB);

            for (var i = 0; i < areaA.ElementsCount; i++)
            {
                var p = areaA[i];
                if (areaB.GetPointIndex(p) != -1)
                {
                    Assert.That(areaC.GetPointIndex(p) > -1);
                }
            }
        }

        [Test]
        public void Result_Should_Be_Equal_To_AreaB()
        {
            var a = Area2D.Create(0, 0, 
                new []
                {
                    new byte[] {1},
                    new byte[] {0},
                    new byte[] {1},
                });
            var b = Area2D.Create(0, 2, 1, 1);

            var res = b.Intersect(a);
            
            Assert.That(res.IsSameAs(b));
        }

        [Test]
        public void Result_Should_Be_Empty_When_Areas_Have_No_Common_Elements()
        {
            var areaA = Area2D.Create(-5, -5, 5, 5);
            var areaB = Area2D.Create(3, 3, 5, 5);
            var areaC = areaA.Intersect(areaB);

            Assert.That(areaC.IsEmpty);
        }

        [Test]
        public void Result_Should_Be_Empty_When_Area_Is_Empty()
        {
            var areaA = Area2D.Create(14, 10, 44, 15);
            var areaB = Area2D.Create(14, 10, 0, 0);
            var areaC = areaA.Intersect(areaB);

            Assert.That(areaB.IsEmpty && areaC.IsEmpty && !areaA.IsEmpty);
        }

        [Test]
        public void Result_Should_Be_Empty_When_Both_Areas_Are_Empty()
        {
            var areaA = Area2D.Create(14, 10, 0, 0);
            var areaB = Area2D.Create(14, 10, 0, 0);
            var areaC = areaA.Intersect(areaB);

            Assert.That(areaA.IsEmpty && areaB.IsEmpty && areaC.IsEmpty);
        }

        [Test]
        public void Result_Should_Throw_ArgumentNullException_When_Area_Is_Null()
        {
            var areaA = Area2D.Create(14, 10, 44, 15);
            Assert.Throws<ArgumentNullException>(() => areaA.Intersect(null));
        }

        [TestCase("256x128", "AreaIntersectionTest", ExpectedResult = true)]
        //[TestCase("1280x720", "AreaIntersectionTest", ExpectedResult = true)]
        public bool Should_Intersect_Areas(string size, string testName)
        {
            var noDiffs = true;
            var ts = TestSet.Init(size);

            var areas = new List<Area2D>
            {
                ts.Donors[0].ToArea(),
                ts.Donors[2].ToArea(),
                ts.Donors[3].ToArea(),
                ts.Donors[4].ToArea(),
                ts.RemoveMarkup.ToArea()
            };

            var testCases = areas.GetAllCombinations()
                .OrderBy(x => x.Count())
                .ToArray();

            for (var i = 0; i < testCases.Length; i++)
            {
                var resultArea = Area2D.Create(0, 0, ts.Picture.Width, ts.Picture.Height);
                var testCase = testCases[i].ToArray();

                resultArea = testCase.Aggregate(resultArea, (current, area) => current.Intersect(area));

                if (!resultArea.IsEmpty)
                {
                    SaveToOutput(resultArea, $"intersection{i}", testName, ts.Path);
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

            return noDiffs;
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
