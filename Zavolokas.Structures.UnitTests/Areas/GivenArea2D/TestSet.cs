using System;
using System.Collections.Generic;
using System.Drawing;

namespace Zavolokas.Structures.UnitTests.Areas.GivenArea2D
{
    internal class TestSet
    {
        public string Path;
        public Bitmap Picture;
        public Bitmap RemoveMarkup;
        public List<Bitmap> Donors;

        private TestSet(){}

        public static TestSet Init(string size)
        {
            var ts = new TestSet
            {
                Path = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\..\Data\images\{size}"))
            };
            ts.Picture = new Bitmap($"{ts.Path}\\picture.png");
            ts.RemoveMarkup = new Bitmap($"{ts.Path}\\inpaintarea.bmp");
            var donors = new List<Bitmap>
            {
                new Bitmap($"{ts.Path}\\donor0.bmp"),
                new Bitmap($"{ts.Path}\\donor1.bmp"),
                new Bitmap($"{ts.Path}\\donor2.bmp"),
                new Bitmap($"{ts.Path}\\donor3.bmp"),
                new Bitmap($"{ts.Path}\\donor4.bmp")
            };
            ts.Donors = donors;
            return ts;
        }
    }
}